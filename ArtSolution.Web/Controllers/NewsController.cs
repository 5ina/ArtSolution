using Abp.AutoMapper;
using Abp.Runtime.Caching;
using ArtSolution.News;
using ArtSolution.Web.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class NewsController : ArtSolutionControllerBase
    {

        #region ctor && Fields
        private readonly INewsService _newsService;
        private readonly ICacheManager _cacheManager;


        public NewsController(INewsService newsService, ICacheManager cacheManager)
        {
            this._newsService = newsService;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Method
        // GET: News
        public ActionResult List(int pageIndex, int pageSize)
        {
            var news = _newsService.GetAllNews(showHidden: false,
                                                pageIndex: pageIndex,
                                                pageSize: pageSize);

            var model = new ShopNewItemListModel();
            model.Items = news.Items.MapTo<List<SimpleNewItemModel>>();
            model.TotalCount = news.TotalCount;
            model.HasNext = pageIndex != 0;
            model.HasPrev = pageIndex < ((model.TotalCount / pageSize));
            model.Pages = (news.TotalCount / pageSize) + (news.TotalCount % pageSize > 0 ? 1 : 0);
            model.CurrentPage = pageIndex;
            return View(model);
        }


        public ActionResult Detail(int newsId)
        {
            var news = _newsService.GetNewsById(newsId);
            var model = news.MapTo<NewItemModel>();
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult HotNews()
        {
            var news = _newsService.GetAllNews(showHidden: false,
                                               pageIndex: 0,
                                               pageSize: 5);
            
            var list = news.Items.MapTo<List<SimpleNewItemModel>>();
            return PartialView(list);
        }

        #endregion
    }
}