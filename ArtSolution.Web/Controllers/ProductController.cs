using Abp.AutoMapper;
using Abp.Runtime.Caching;
using Abp.Web.Security.AntiForgery;
using ArtSolution.Catalog;
using ArtSolution.Common;
using ArtSolution.CommonSettings;
using ArtSolution.Customers;
using ArtSolution.Media;
using ArtSolution.Names;
using ArtSolution.Orders;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.WeChat;
using ArtSolution.Web.Models.Catalogs;
using ArtSolution.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static ArtSolution.Web.Framework.CacheNames;

namespace ArtSolution.Web.Controllers
{
    public class ProductController : ArtSolutionControllerBase
    {
        #region ctor && Fields

        /// <summary>
        /// 商品评论缓存kery
        /// </summary>
        private const string PRDOUCT_REVIEW_BY_PRODUCTID = "store.product.review.by.productid-{0}";

        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductImagesService _imageService;
        private readonly ICacheManager _cacheManager;
        private readonly IImageService _mediaService;
        private readonly IFavoriteService _favoriteService;
        private readonly IProductReviewService _reviewService;
        private readonly IProductAttributeService _attributeService;
        private readonly IBrandService _brandService;
        private readonly ISettingService _settingService;

        public ProductController(ICategoryService categoryService,
            ICustomerService customerService,
            IProductService productService,
            IProductImagesService imageService,
            ICacheManager cacheManager,
            IImageService mediaService,
            IFavoriteService favoriteService, 
            IProductReviewService reviewService,
            IBrandService brandService,
            IProductAttributeService attributeService,
            ISettingService settingService)
        {
            this._categoryService = categoryService;
            this._productService = productService;
            this._imageService = imageService;
            this._cacheManager = cacheManager;
            this._mediaService = mediaService;
            this._favoriteService = favoriteService;
            this._reviewService = reviewService;
            this._attributeService = attributeService;
            this._settingService = settingService;
            this._customerService = customerService;
            this._brandService = brandService;
        }
        #endregion

        #region Utilities
        [NonAction]
        protected void PrepareProductAttribute(ProductModel model)
        {
                var attributes = _attributeService.GetProductAttributes(model.Id);
                if (attributes.Count > 0)
                {
                    model.SubProductAttributes = attributes.Select(a => a.MapTo<ProductModel.ProductAttributeModel>()).ToList();
                }
        }
        [NonAction]
        protected void PrepareProductRelated(ProductModel model)
        {
            if (!String.IsNullOrWhiteSpace(model.RelatedProductIds))
            {
                var relatedIds = model.RelatedProductIds;
                var ids = relatedIds.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                var relateds = _productService.GetProductByIds(ids);
                if (relateds != null)
                    model.ProductRelateds = relateds.Select(r => r.MapTo<SimpleProductModel>()).ToList();
            }
        }
        
        #endregion

        #region Method
        public ActionResult Detail(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return Redirect("/");

            var model = product.MapTo<ProductModel>();
            var images = _imageService.GetProductImagesByProductId(productId);
            model.SubPictures = images.Select(i => new ProductModel.ProductPictureModel
            {
                Id = i.Id,
                IsDefault = i.Url == product.ProductImage,
                PictureUrl = i.Url,
                ProductId = i.ProductId
            }).ToList();
                        
            var brand = _brandService.GetBrandById(model.BrandId);
            if (brand != null)
            {
                var brandToProductCount = _cacheManager.GetCache(Products.CACHE_PRODUCT_TO_BRAND_COUNT).Get(brand.Id.ToString(), () =>
                 {
                     var products = _productService.GetAllProducts(brand: brand.Id);
                     return products.TotalCount;
                 });
                model.BrandName = brand.Name;
                model.BrandImage = brand.BrandImage;
                model.BrandToProductCount = brandToProductCount;
            }

            //属性
            PrepareProductAttribute(model);
            //关联商品
            PrepareProductRelated(model);
            //收藏
            if (CustomerId ==0)
                model.IsFavorites = false;
            else {
                var favorites = _favoriteService.GetAllFavorites(customerId: this.CustomerId);
                model.IsFavorites = favorites.Items.FirstOrDefault(f => f.ProductId == productId) != null;
            }

            if (model.AllowReward)
            {
                var rate = _settingService.GetSettingByKey<int>(RewardSettingNames.PaymentRate);
                model.RewardExchange = Convert.ToInt32(model.Price * rate);
            }

            ViewData["CustomerId"] = AbpSession.UserId;
                        
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string Keyword)
        {
            var query = _productService.GetAllProducts(keywords: Keyword, pageIndex: 0, pageSize: 10);

            var list = query.Items.MapTo<List<SimpleProductModel>>();
            ViewData["Keywords"] = Keyword;
            return View(list);
        }

        [HttpPost]
        public ActionResult SearchData(string Keyword, int pageIndex , int pageSize)
        {
            var query = _productService.GetAllProducts(keywords: Keyword, pageIndex: pageIndex, pageSize: pageSize);

            var list = query.Items.MapTo<List<SimpleProductModel>>();
            return AbpJson(list);
        }

        [HttpGet]
        public ActionResult Favorites(int productId)
        {
            return Content("已收藏");
        }

        [ChildActionOnly]
        public ActionResult RelatedProduct(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            var categories = category.GetAllCategoriesRecursive(_categoryService);
            var products = _productService.GetAllProducts(categoryIds: categories, pageIndex: 0, pageSize: 6);
            var models = products.Items.MapTo<IList<ProductModel>>();
            return PartialView(models);
        }


        [ChildActionOnly]
        public ActionResult HomeProducts(int maxCount) {

           var products = _productService.GetAllProducts(pageIndex: 0,
                                            pageSize: maxCount);
            var models = products.Items.MapTo<IList<SimpleProductModel>>();
            return PartialView(models);
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult GetProducts(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var products = _productService.GetAllProducts(pageIndex: pageIndex,
                                                pageSize: pageSize);

            var total = products.TotalCount / pageSize;
            total = products.TotalCount % pageSize > 0 ? total + 1 : total;
            var jsonData = new DataSourceResult
            {
                Total = total,
                Data = products.Items.MapTo<IList<SimpleProductModel>>(),
                ShowNext = total > pageIndex
            };

            return AbpJson(jsonData);

        }
        #endregion

        #region Product Review
        [ChildActionOnly]
        public ActionResult ProductReview(int productId)
        {
            var reviews = _reviewService.GetAllProductReviews(productId: productId, pageIndex: 0, pageSize: 5);

            var model = new ProductReviewListModel();
            model.Total = reviews.TotalCount;
            model.PageIndex = 0;
            model.PageSize = 5;
            model.ProductId = productId;
            model.Reviews = reviews.Items.MapTo<List<ProductReviewModel>>();
            return PartialView(model);
        }

        public ActionResult ReviewsAll(int productId)
        {
            var reviews = _reviewService.GetAllProductReviews(productId: productId, pageIndex: 0, pageSize: 20);

            var model = new ProductReviewListModel();
            model.Total = reviews.TotalCount;
            model.PageIndex = 0;
            model.PageSize = 20;
            model.ProductId = productId;
            model.Reviews = reviews.Items.MapTo<List<ProductReviewModel>>();
            return View(model);
        }

        [HttpPost]
        public ActionResult ReviewsAll(int productId,int pageIndex= 0,int pageSize = 20)
        {
            var reviews = _reviewService.GetAllProductReviews(productId: productId, pageIndex: 0, pageSize: 20);

            var model = new ProductReviewListModel();
            model.Total = reviews.TotalCount;
            model.PageIndex = 0;
            model.PageSize = 20;
            model.ProductId = productId;
            model.Reviews = reviews.Items.MapTo<List<ProductReviewModel>>();
            return Json(model);
        }

        public ActionResult Review(int productId,int orderItemId)
        {
            ProductReviewModel model = new ProductReviewModel();
            model.CustomerId = this.CustomerId;
            model.ProductId = productId;
            model.OrderItemId = orderItemId;
            var product = _productService.GetProductById(productId);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveReview(ProductReviewModel model)
        {
            var customer = _customerService.GetCustomerId(model.CustomerId);
            model.CustomerName = customer.NickName;
            return View();
        }
        #endregion

        #region  分享页面

        [NonAction]
        public WechatSetting GetWeChatSetting()
        {
            return _settingService.GetOrderSettings();
        }


        #endregion
    }
}