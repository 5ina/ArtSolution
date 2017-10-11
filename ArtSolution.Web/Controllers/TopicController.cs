using Abp.AutoMapper;
using Abp.Runtime.Caching;
using ArtSolution.News;
using ArtSolution.Web.Models.News;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ArtSolution.Web.Framework.CacheNames;

namespace ArtSolution.Web.Controllers
{
    public class TopicController : ArtSolutionControllerBase
    {
        #region ctor && Fields
        private readonly ITopicService _topicService;
        private readonly ICacheManager _cacheManager;

        

        public TopicController(ITopicService topicService, ICacheManager cacheManager)
        {
            this._topicService = topicService;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region 
        public ActionResult Detail(string systemname)
        {
            var topic = _cacheManager.GetCache(string.Format(Topics.TOPIC_SYSTEM, systemname)).Get(systemname, () => _topicService.GetTopicBySystemName(systemname));
            var model = topic.MapTo<TopicModel>();
            return View(model);
        }
        #endregion
    }
}