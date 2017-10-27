using Abp.AutoMapper;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Mvc;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 组合套餐控制器
    /// </summary>
    public class ComBoProductController : ArtSolutionControllerAdminBase
    {

        #region ctor && Fields
        private readonly IComBoProductService _comboService;
        private readonly IProductService _productService;

        public ComBoProductController(IComBoProductService comboService, IProductService productService)
        {
            this._comboService = comboService;
            this._productService = productService;
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command ,string keywords = "")
        {
            var list = _comboService.GetComBoProducts(keywords: keywords,
                                        pageIndex: command.Page - 1,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = list.Items.Select(b => new
                {
                    Id = b.Id,
                    Name = b.Name,
                    Price = b.Price,
                    Published = b.Published
                }),
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new ComBoProductModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ComBoProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<ComBoProduct>();
                _comboService.InsertComBoProduct(entity);
                return RedirectToAction("List");
            }
            return View(model);

        }

        public ActionResult Edit(int id)
        {
            var combo = _comboService.GetComBoProductById(id);
            if (combo != null)
            {
                var model = combo.MapTo<ComBoProductModel>();
                var productIds = _comboService.GetComBoProductMappings(combo.Id);
                var products = _productService.GetProductByIds(productIds);
                model.ComBoProducts = products.Select(p => new ComBoProductModel.ProductMapping
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                }).ToList();
                return View(model);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(ComBoProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _comboService.GetComBoProductById(model.Id);
                entity = model.MapTo<ComBoProductModel, ComBoProduct>(entity);
                _comboService.UpdateComBoProduct(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteComBoProduct(int comBoId)
        {
            _comboService.DeleteComBoProduct(comBoId);
            return new NullJsonResult();
        }


        [HttpPost]
        public ActionResult DeleteComBoProductMapping(int comBoId,int productId)
        {
            _comboService.DeleteMapping(comBoId, productId);
            return AbpJson("ok");
        }
        
        [HttpPost]
        public ActionResult AddComBoProductMapping(int comBoId, int productId)
        {
            _comboService.InsertMapping(comBoId, productId);
            return AbpJson("ok");
        }
        #endregion
    }
}