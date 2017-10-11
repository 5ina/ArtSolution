using Abp.AutoMapper;
using ArtSolution.Catalog;
using ArtSolution.Domain.Catalog;
using ArtSolution.Web.Areas.Admin.Models.Catalog;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class ProductTagController : ArtSolutionControllerAdminBase
    {

        #region ctor && Fields
        private readonly IProductTagService _tagService;


        public ProductTagController(IProductTagService tagService)
        {
            this._tagService = tagService;
        }
        #endregion

        #region Method
        public ActionResult List()
        {
            return View();
        }



        [HttpPost]
        public ActionResult List(DataSourceRequest command,string Keywords)
        {
            var brands = _tagService.GetAllTags(keywords: Keywords,
                                        pageIndex: command.Page,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                ExtraData = brands.Items.Select(b => new
                {
                    Id = b.Id,
                    Name = b.Name,
                    DisplayOrder = b.DisplayOrder,
                }),
            };
            return AbpJson(jsonData);
        }


        public ActionResult Create()
        {
            var model = new ProductTagModel();
            model.DisplayOrder = 999;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductTagModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<ProductTag>();
                _tagService.InsertTag(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var brand = _tagService.GetTag(id);
            var model = brand.MapTo<ProductTagModel>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductTagModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _tagService.GetTag(model.Id);

                if (entity.TagImage != model.TagImage)
                {

                }

                entity = model.MapTo<ProductTag>();
                _tagService.UpdateTag(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int brandId)
        {
            if (brandId <= 0)
                throw new Exception("brand");

            //using (var unitOfWork = _unitOfWorkManager.Begin())
            //{
            //    _brandService.DeleteBrand(brandId);

            //    unitOfWork.Complete();
            //    return Json("ok");
            //}
            return Json("ok");
        }
        #endregion
    }
}