using Abp.AutoMapper;
using ArtSolution.Domain.News;
using ArtSolution.News;
using ArtSolution.Web.Areas.Admin.Models.News;
using ArtSolution.Web.Framework.DataGrids;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class PromotionalController : ArtSolutionControllerAdminBase
    {
        #region Fields && Ctor

        private readonly IPromotionalService _promotionalService;

        public PromotionalController(IPromotionalService promotionalService)
        {
            this._promotionalService = promotionalService;
        }

        #endregion

        #region Method
        public ActionResult List()
        {
            var model = new PromotionalListModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, PromotionalListModel model)
        {
            var promotionals = _promotionalService.GetAllPromotionals(model.Keywords, true, model.StartDate, model.EndDate, command.Page, command.PageSize);
            
            var jsonData = new DataSourceResult
            {
                ExtraData = promotionals.Items.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    StartDate = c.StartDate.ToString("yyyy/MM/dd"),
                    EndDate = c.EndDate.ToString("yyyy/MM/dd"),
                    Published = c.Published,
                    CreationTime = c.CreationTime.ToString("yyyy/MM/dd"),
                }).ToList(),
            };

            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new PromotionalModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PromotionalModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Promotional>();
                _promotionalService.InsertPromotional(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _promotionalService.GetPromotionalById(id);
            var model = entity.MapTo<PromotionalModel>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PromotionalModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _promotionalService.GetPromotionalById(model.Id);
                entity = model.MapTo<PromotionalModel, Promotional>(entity);
                _promotionalService.UpdatePromotional(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entity = _promotionalService.GetPromotionalById(id);
            if (entity != null)
            {
                _promotionalService.DeletePromotional(id);
                return AbpJson("ok");
            }
            else {
                return AbpJson("error");
            }
        }
        #endregion
    }
}