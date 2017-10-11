using Abp.AutoMapper;
using ArtSolution.Common;
using ArtSolution.Domain.Common;
using ArtSolution.Web.Areas.Admin.Models.Common;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class AdvertController : ArtSolutionControllerAdminBase
    {

        #region ctor && Fields
        private readonly IAdvertService _advertService;


        public AdvertController(IAdvertService advertService)
        {
            this._advertService = advertService;
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new AdvertListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, AdvertListModel model)
        {
            var adverts = _advertService.GetAllAdverts(
                keywords: model.Keywords,
                showFrom: model.ShowFrom,
                showTo: model.ShowTo,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = adverts.Items.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShowFrom = x.StartTime.ToString("yyyy/MM/dd"),
                    ShowTo = x.EndTime.ToString("yyyy/MM/dd"),
                    DisplayOrder = x.DisplayOrder,
                    Url = x.AdvertUrl,
                    CreationTime = x.CreationTime.ToString("yyyy/MM/dd")
                }).ToList(),
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new AdvertModel();
            model.DisplayOrder = 999;
            model.StartTime = DateTime.Now;
            model.EndTime = DateTime.Now.AddDays(30);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(AdvertModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Advert>();
                _advertService.InsertAdvert(entity);
                return RedirectToAction("List");
            }
            
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var advert = _advertService.GetAdvertById(id);
            if (advert == null)
                return RedirectToAction("List");
            var model = advert.MapTo<AdvertModel>();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(AdvertModel model)
        {
            if (ModelState.IsValid)
            {
                var advert = _advertService.GetAdvertById(model.Id);
                advert = model.MapTo<AdvertModel, Advert>(advert);
                _advertService.UpdateAdvert(advert);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _advertService.DeleteAdvert(id);
                return AbpJson("true");
            }
            catch (Exception e)
            {
                return AbpJson(e.Message);
            }

        }
        #endregion
    }
}