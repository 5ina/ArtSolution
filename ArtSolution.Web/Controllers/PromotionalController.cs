using Abp.AutoMapper;
using ArtSolution.News;
using ArtSolution.Web.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    /// <summary>
    /// 促销专题控制器
    /// </summary>
    public class PromotionalController : ArtSolutionControllerBase
    {
        #region ctor && Fields
        private readonly IPromotionalService _promotionalService;

        public PromotionalController(IPromotionalService promotionalService)
        {
            this._promotionalService = promotionalService;
        }
        #endregion

        #region Method

        public ActionResult List(int page = 0)
        {
            var promotionals = _promotionalService.GetAllPromotionals(pageIndex: page, pageSize: 15);

            var model = new PromotionalListModel();
            model.Items = promotionals.Items.Select(
                p => p.MapTo<PromotionalModel>()).ToList();

            model.Count = promotionals.TotalCount;
            model.Pages = promotionals.TotalCount / 15;
            model.Current = page;
            return View(model);
        }
        #endregion

    }
}