using Abp.AutoMapper;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Models.Catalogs;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class CatalogController : ArtSolutionControllerBase
    {

        #region ctor && Fields
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;


        public CatalogController(ICategoryService categoryService,
            IProductService productService,
            IBrandService brandService)
        {
            this._categoryService = categoryService;
            this._productService = productService;
            this._brandService = brandService;
        }
        #endregion

        #region Utilities

        [NonAction]
        private List<CatalogModel> GetChildsByParentEntity(Category category,List<Category> allCategories)
        {
            var childs = category.GetAllChildsCategoriesRecursive(allCategories);
            var list = childs.Select(c =>
             {
                 var childItem = c.MapTo<CatalogModel>();

                 childItem.Childs = GetChildsByParentEntity(c, allCategories);

                 return childItem;
             }).ToList();
            return list;
        }
        #endregion

        #region Method
        [ChildActionOnly]
        public ActionResult TopCatalog()
        {
            var hotCatalogs = _categoryService.GetAllCategories(parentId: 0, pageIndex: 0, pageSize: 8);
            var model = new TopCatalogModel();
            model.SubCategories = hotCatalogs.Items.Select(c => c.MapTo<CategoryModel>()).ToList();
            return PartialView(model);
        }

        public ActionResult Detail(int catalogId)
        {
            CatalogDetailModel model = null;
            if (catalogId == 0)
            {
                var childsCatagories = _categoryService.GetCategoriesByParentId(catalogId);
                var products = _productService.GetAllProducts();
                model = new CatalogDetailModel
                {
                    Id =0,
                    Description = "全部商品",
                    MetaDescription = "全部商品",
                    MetaKeywords = "全部商品",
                    MetaTitle = "全部商品",
                    Name = "全部商品",
                    SubCatagoryies = childsCatagories.MapTo<IList<CategoryModel>>(),
                    SubProducts = products.Items.MapTo<IList<SimpleProductModel>>(),
                    TotalProductCount = products.TotalCount,

                };
            }
            else
            {
                var category = _categoryService.GetCategoryById(catalogId);
                var allCatagories = category.GetAllCategoriesRecursive(_categoryService);
                var products = _productService.GetAllProducts(categoryIds: allCatagories);
                var childsCatagories = _categoryService.GetCategoriesByParentId(catalogId);


                model = new CatalogDetailModel
                {
                    Id = category.Id,
                    Description = category.Description,
                    MetaDescription = category.MetaDescription,
                    MetaKeywords = category.MetaKeywords,
                    MetaTitle = category.MetaTitle,
                    Name = category.Name,
                    SubCatagoryies = childsCatagories.MapTo<IList<CategoryModel>>(),
                    SubProducts = products.Items.MapTo<IList<SimpleProductModel>>(),
                    TotalProductCount = products.TotalCount,

                };
            }
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult HomeCategory(int categoryId)
        {
            var parentId = 0;
            var model = new TopCatalogModel();
            if (categoryId > 0)
            {
                var category = _categoryService.GetCategoryById(categoryId);
                model.IsTopCategory = category.ParentId == 0;
                if (model.IsTopCategory)
                    parentId = categoryId;
                else
                    parentId = category.ParentId;
            }
            var hotCatalogs = _categoryService.GetAllCategories(parentId: parentId, pageIndex: 0, pageSize: 4);
            model.SubCategories = hotCatalogs.Items.Select(c => c.MapTo<CategoryModel>()).ToList();
            model.ActiveId = categoryId;
            return PartialView(model);
        }

        public ActionResult Catalogs()
        {
            var allCategories = _categoryService.GetAllCategories();
            var subCatalogs = allCategories.Items.Where(c => c.ParentId == 0).Select(c =>
            {
                var item = c.MapTo<CatalogModel>();
                item.Childs = GetChildsByParentEntity(c, allCategories.Items.ToList());
                return item;
            }).ToList();
            
            return View(subCatalogs);
        }

        public ActionResult Brands()
        {
            var brands = _brandService.GetAllBrands();
            var models = brands.Items.Select(b => new BrandModel
            {
                Description = b.Description,
                DisplayOrder = b.DisplayOrder,
                Id = b.Id,
                Name = b.Name,
                BrandImage = b.BrandImage
            }).ToList();
            return View(models);

        }

        public ActionResult BrandProducts(int brandId)
        {
            var brand = _brandService.GetBrandById(brandId);
            var products = _productService.GetAllProducts(brand: brandId);
            var model = new BrandDetailModel
            {
                Id = brand.Id,
                Description = brand.Description,
                Name = brand.Name,
                SubProducts = products.Items.MapTo<IList<SimpleProductModel>>(),
                TotalProductCount = products.TotalCount,
            };

            return View(model);
        }

        public ActionResult All()
        {
            var model = new CatalogListModel();
            var parentsCatalogs = _categoryService.GetAllCategories(parentId: 0);

            model.SubListCatalog = parentsCatalogs.Items.Select(p => new CatalogModel
            {
                Childs = _categoryService.GetCategoriesByParentId(p.Id).MapTo<List<CatalogModel>>(),
                Id = p.Id,
                Description = p.Description,
                MetaDescription = p.MetaDescription,
                MetaKeywords = p.MetaKeywords,
                MetaTitle = p.MetaTitle,
                Name = p.Name

            }).ToList();
            return View(model);
        }
        #endregion

    }
}