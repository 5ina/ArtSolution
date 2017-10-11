using Abp.AutoMapper;
using Abp.Runtime.Caching;
using ArtSolution.Domain.News;
using ArtSolution.News;
using ArtSolution.Web.Areas.Admin.Models.News;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class NewsController : ArtSolutionControllerAdminBase
    {
        #region Ctor && Fields
        private readonly INewsService _newsService;
        private readonly ICacheManager _cacheManager;


        public NewsController(INewsService newsService,
                                    ICacheManager cacheManager)
        {
            this._newsService = newsService;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            var news = _newsService.GetAllNews(true, command.Page, command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = news.Items.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishDateTime = x.PublishDateTime.ToString("yyyy/MM/dd hh:mm"),                    
                }).ToList(),
            };
            return AbpJson(jsonData);
        }


        public ActionResult Create()
        {
            var model = new NewItemModel();
            model.PublishDateTime = DateTime.Now;
            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(NewItemModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<NewItem>();
                model.Id = _newsService.InsertNews(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var newItem = _newsService.GetNewsById(id);
            var model = newItem.MapTo<NewItemModel>();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(NewItemModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = _newsService.GetNewsById(model.Id);
                entity = model.MapTo<NewItemModel, NewItem>(entity);
                _newsService.UpdateNews(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            return View(model);
        }

        #endregion
    }
}
