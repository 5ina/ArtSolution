using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Web.Security.AntiForgery;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Media;
using ArtSolution.Names;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class ProductController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields

        private const string ATTRIBUTECACHE = "stroe.attribute.all";

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductImagesService _productImageService;
        private readonly IOssService _imageService;
        private readonly ICacheManager _cacheManager;
        private readonly IProductAttributeService _attributeService;
        private readonly IBrandService _brandService;
        private readonly IProductTagService _tagService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProductController(ICategoryService categoryService,
            IProductService productService,
            IProductImagesService productImageService,
            ICacheManager cacheManager,
            IOssService imageService,
            IProductAttributeService attributeService,
            IBrandService brandService,
            IProductTagService tagService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this._categoryService = categoryService;
            this._productService = productService;
            this._productImageService = productImageService;
            this._cacheManager = cacheManager;
            this._attributeService = attributeService;
            this._imageService = imageService;
            this._brandService = brandService;
            this._tagService = tagService;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region Utilities
        [NonAction]
        protected void PrepareProductListModel(ProductListModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");
            
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories.Items)
            {
                model.AvailableCategories.Add(new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(categories.Items.ToList()),
                    Value = c.Id.ToString()
                });
            }
        }

        [NonAction]
        protected void PrepareProductModel(ProductModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");

            model.AvailableCategories.Add(new SelectListItem
            {
                Text = "[请选择类别]",
                Value = "0"
            });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories.Items)
            {
                model.AvailableCategories.Add(new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(categories.Items.ToList()),
                    Value = c.Id.ToString()
                });
            }
        }

        [NonAction]
        protected void PrepareProductImagesModel(ProductModel model)
        {

            if (model == null)
                throw new UserFriendlyException("model");
            var images = _productImageService.GetProductImagesByProductId(model.Id);

            foreach (var item in images)
            {
                model.AvailablePictures.Add(new ProductModel.ProductPictureModel
                {
                    Id = item.Id,
                    PictureUrl = item.Url,
                    ProductId = item.ProductId
                });
            }
        }

        [NonAction]
        protected void PrepareProductAttribute(ProductModel model)
        {
            if (model.Id > 0)
            {
                var attributes = _attributeService.GetProductAttributes(model.Id);
                if (attributes.Count > 0)
                {
                    model.AvailableAttributes = attributes.Select(a => a.MapTo<ProductAttributeModel>()).ToList();
                }
            }
        }

        [NonAction]
        protected void PrepareProductRelated(ProductModel model)
        {
            if (model.Id > 0)
            {
                if (!String.IsNullOrWhiteSpace(model.RelatedProductIds))
                {
                    var relatedIds = model.RelatedProductIds;
                    var ids = relatedIds.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                    var relateds = _productService.GetProductByIds(ids);
                    if (relateds != null)
                        model.ProductRelateds = relateds.Select(r => r.MapTo<ProductReviewModel>()).ToList();
                }
            }
        }


        [NonAction]
        protected void PrepareProductBrands(ProductModel model)
        {
            var allBrands = _brandService.GetAllBrands();
            model.AvailableBrands = allBrands.Items.Select(b => new SelectListItem
            {
                Text = b.Name,
                Selected = model.Id == b.Id,
                Value = b.Id.ToString(),
            }).ToList();
        }

        [NonAction]
        protected int GetProductStockQuantity(int productId)
        {
            var attributes = _attributeService.GetProductAttributes(productId);

            return attributes.Sum(a => a.Stock);
        }


        [NonAction]
        protected void PreparProductTags(ProductModel model)
        {
            var productTags = _tagService.GetAllTags();

            model.AvailableProductTag = productTags.Items.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            if (model.Id > 0)
            {
                var tags = _tagService.GetAllProductTagMappings(productId: model.Id);
                model.ProductTagIds = tags.Select(m => m.TagId).ToList();
            }
        }

        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new ProductListModel();
            PrepareProductListModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ProductListModel model)
        {
            var products = _productService.GetAllProducts(keywords: model.Keywords,
                                                        categoryIds: model.CategoryIds,
                                                        showHidden: true,
                                                        brand: model.BrandId,
                                                        isPre: model.IsPreSell,
                                                        pageIndex: command.Page - 1,
                                                        pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                Data = products.Items,
                Total = products.TotalCount
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new ProductModel();
            PrepareProductModel(model);
            PrepareProductBrands(model);
            PreparProductTags(model);
            model.DisplayOrder = 999;
            model.Published = true;
            model.AllowReward = true;
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(ProductModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {                
                var entity = model.MapTo<Product>();              
                model.Id = _productService.InsertProduct(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            PrepareProductModel(model);
            PrepareProductBrands(model);
            PreparProductTags(model);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return RedirectToAction("List");

            var model = product.MapTo<ProductModel>();
            PrepareProductModel(model);
            PrepareProductImagesModel(model);
            PrepareProductBrands(model);
            PrepareProductRelated(model);
            PreparProductTags(model);
            return View(model);
        }
        

        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(ProductModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var relateds = Request.Form["RelatedProductIds"];
                model.RelatedProductIds = relateds;
                var product = _productService.GetProductById(model.Id);
                product = model.MapTo<ProductModel, Product>(product);
                _productService.UpdateProduct(product);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            PrepareProductModel(model);
            PrepareProductBrands(model);
            //PrepareProductAttribute(model);
            PrepareProductRelated(model);
            PreparProductTags(model);
            return View(model);
        }

        public ActionResult CopyProduct(int productId)
        {
            var product = _productService.GetProductById(productId);
            var model = product.MapTo<ProductModel>();
            PrepareProductModel(model);
            PrepareProductImagesModel(model);
            PrepareProductBrands(model);
            PreparProductTags(model);
            PrepareProductRelated(model);
            model.DisplayOrder = 999;
            model.Published = true;
            model.AllowReward = true;
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CopyProduct(ProductModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Product>();
                var modelId = 0;
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var product = _productService.GetProductById(model.Id);
                    var images = _productImageService.GetProductImagesByProductId(model.Id);
                    entity.ProductImage = product.ProductImage;
                    entity.Id = 0;
                    modelId = _productService.InsertProduct(entity);
                    //增加图片
                    images.ToList().ForEach(e => _productImageService.InsertImage(new ProductImage {
                        ProductId = modelId,
                        Url = e.Url
                    }));
                    unitOfWork.Complete();
                }

                return continueEditing ? RedirectToAction("Edit", new { id = modelId }) : RedirectToAction("List");
            }
            PrepareProductModel(model);
            PrepareProductBrands(model);
            PreparProductTags(model);
            return View(model);
        }

        public ActionResult Delete(int productId)
        {
            try
            {
                _productService.DeleteProduct(productId);
                return AbpJson("true");
            }
            catch
            {
                return AbpJson("fail");
            }
        }
        #endregion

        #region product Image

        [HttpPost]
        public ActionResult SetDefaultImage(int Id, int imageId)
        {
            var image = _productImageService.GetProductImageById(imageId);
            var product = _productService.GetProductById(Id);
            product.ProductImage = image.Url;
            return AbpJson("");
        }
        
        [HttpPost]
        public ActionResult ProductPictureDelete(int id)
        {
            var image = _productImageService.GetProductImageById(id);
            _imageService.DeleteImage(image.Url);
            _productImageService.DeleteImage(image.Id);
            return Content("true");
        }


        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult AsyncUploadImage()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["image"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("文件不存在");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla-		Request	{System.Web.HttpRequestWrapper}	System.Web.HttpRequestBase {System.Web.HttpRequestWrapper}

                stream = Request.InputStream;
                fileName = Request["image"];
            }

            var productId = Convert.ToInt32(Request["productId"]);

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var url = _imageService.UploadImage(images: fileBinary, isBuildThumbnail: true);

            _productImageService.InsertImage(new ProductImage
            {
                ProductId = productId,
                Url = url
            });

            return Json(new
            {
                success = true,
                Url = url,
            });
        }
        #endregion

        #region Attribute
        [HttpPost]
        public ActionResult CreateAttribute(ProductAttributeModel model)
        {
            var entity = model.MapTo<ProductAttribute>();
            var attributeId = _attributeService.InsertProductAttribute(entity);


            var stock = GetProductStockQuantity(model.ProductId);
            var product = _productService.GetProductById(model.ProductId);
            product.StockQuantity = stock;
            product.Price = product.Price < model.Price ? product.Price : model.Price;
            _productService.UpdateProduct(product);
            var jsonData = new
            {
                attributeId = attributeId,
                stock = stock
            };
            return AbpJson(jsonData);
        }
        
        [HttpPost]
        public ActionResult UpdateAttribute(ProductAttributeModel model)
        {
            var entity = model.MapTo<ProductAttribute>();
            _attributeService.UpdateProductAttribute(entity);
            var stock = GetProductStockQuantity(model.ProductId);
            var product = _productService.GetProductById(model.ProductId);
            product.StockQuantity = stock;
            if (product.Price != 0 && model.Price > 0)
            {
                product.Price = product.Price < model.Price ? product.Price : model.Price;
            }
            else
            {
                product.Price = model.Price;
            }
            _productService.UpdateProduct(product);
            var jsonData = new
            {
                attributeId = entity.Id,
                stock = stock,
                price = product.Price
            };
            return AbpJson(jsonData);
        }

        public ActionResult DeleteAttribute(int attributeId)
        {
            var attribute = _attributeService.GetProductAttributeById(attributeId);
            if (attribute != null)
            {

                var productId = attribute.ProductId;
                _attributeService.DeleteProductAttributeById(attributeId);
                var stock = GetProductStockQuantity(productId);
                var product = _productService.GetProductById(productId);
                product.StockQuantity = stock;
                _productService.UpdateProduct(product);
                var jsonData = new
                {
                    attributeId = attributeId,
                    stock = stock,
                    price = product.Price
                };
                return AbpJson(jsonData);
            }

            return AbpJson("true");

        }
        #endregion

        #region Statistical Report

        public ActionResult ProductStatisticalOverview()
        {
            var model = _cacheManager.GetCache(ProductCacheNames.CACHE_PRODUCT_StatisticalOverview)
                .Get("", () => {

                    var products = _productService.GetAllProducts(showHidden: true);
                    var view = new ProductStatisticalOverviewModel();
                    view.ProductTotal = products.Items.Count();
                    view.PulishedTotal = products.Items.Where(x => x.Published).Count();
                    return view;
                });
            return PartialView(model);
        }
        #endregion

        #region BoxSearch
        public ActionResult BoxProduct(ProductListModel model)
        {
            PrepareProductListModel(model);
            return View(model);
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult GetProductInfo(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return null;
            else
            {
                return Json(product);
            }
        }
        #endregion

        #region Update Product Price

        [HttpPost]
        public ActionResult UpdateProductPrice(UpdateProductPriceModel model)
        {
            var product = _productService.GetProductById(model.Id);

            if (product != null)
            {
                _attributeService.SetProductAttributePrice(product.Id, model.Price);

                product.Price = model.Price;
            }
            return new NullJsonResult();
        }
        #endregion
    }
}