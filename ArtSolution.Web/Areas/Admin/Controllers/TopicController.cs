using Abp.AutoMapper;
using ArtSolution.Domain.News;
using ArtSolution.News;
using ArtSolution.Web.Areas.Admin.Models.News;
using ArtSolution.Web.Framework.DataGrids;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 主题控制器
    /// </summary>
    public class TopicController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly ITopicService _topicService;


        public TopicController(ITopicService topicService)
        {
            this._topicService = topicService;
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
            var news = _topicService.GetAllTopics(pageIndex: command.Page -1,
                pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = news.Items.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    SystemName = x.SystemName,
                    CreationTime = x.CreationTime,
                }).ToList(),
                Total= news.TotalCount
            };
            return AbpJson(jsonData);
        }

        public ActionResult Create()
        {
            var model = new TopicModel();
            return View(model);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TopicModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Topic>();
                _topicService.InsertTopic(entity);
                return List();
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _topicService.GetTopicById(id);
            var model = entity.MapTo<TopicModel>();
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(TopicModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _topicService.GetTopicById(model.Id);
                entity = model.MapTo<TopicModel, Topic>(entity);
                _topicService.UpdateTopic(entity);
                return List();
            }
            return View(model);
        }
        #endregion


    }
}