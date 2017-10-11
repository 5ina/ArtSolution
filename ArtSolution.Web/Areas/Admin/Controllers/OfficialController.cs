using ArtSolution.Official;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 企业查看
    /// </summary>
    public class OfficialController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly IMessageBoardService _messageService;


        public OfficialController(IMessageBoardService messageService)
        {
            this._messageService = messageService;
        }
        #endregion

        public ActionResult MessageList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MessageList(DataSourceRequest command)
        {
            var messages = _messageService.GetAlls(pageIndex: command.Page,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                ExtraData = messages.Items.Select(b => new
                {
                    Id = b.Id,
                    Name = b.Name,
                    Mobile = b.Mobile,
                    CreationTime = b.CreationTime.ToString("yyyy/mm/dd")
                }),
            };
            return AbpJson(jsonData);
        }

        [HttpPost]
        public ActionResult MessageDelete(int id)
        {
            _messageService.Delete(id);
            return AbpJson("ok");
        }
    }
}