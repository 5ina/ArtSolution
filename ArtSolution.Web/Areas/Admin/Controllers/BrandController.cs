using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.UI;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class BrandController : ArtSolutionControllerAdminBase
    {
        #region Fields && Ctor

        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BrandController(ICategoryService categoryService,
                            IBrandService brandService,
                            IUnitOfWorkManager unitOfWorkManager)
        {
            this._brandService = brandService;
            this._categoryService = categoryService;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        #endregion

        #region Utilities

        protected void PrepareBrandModel(BrandModel model)
        {
            if (model == null)
                throw new UserFriendlyException("brand");

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
        #endregion

        #region method

        public ActionResult List()
        {
            var model = new BrandListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command , BrandListModel model)
        {
            var brands = _brandService.GetAllBrands(keywords: model.Keywords,
                                        pageIndex: command.Page,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = brands.Items.Select(b => new
                {
                    Id = b.Id,
                    Name = b.Name,
                    DisplayOrder = b.DisplayOrder,
                    CreationTime = b.CreationTime
                }),
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new BrandModel();
            PrepareBrandModel(model);
            model.DisplayOrder = 999;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BrandModel model)
        {
            if (ModelState.IsValid)

            {
                var entity = model.MapTo<Brand>();
                _brandService.InsertBrand(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var brand = _brandService.GetBrandById(id);
            var model = brand.MapTo<BrandModel>();
            PrepareBrandModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _brandService.GetBrandById(model.Id);
                entity = model.MapTo<BrandModel, Brand>(entity);
                _brandService.UpdateBrand(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int brandId)
        {
            if (brandId <= 0)
                throw new Exception("brand");

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                _brandService.DeleteBrand(brandId);

                unitOfWork.Complete();
                return Json("ok");
            }
        }
        #endregion
    }
}