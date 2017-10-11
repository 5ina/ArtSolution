using ArtSolution.News;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class LoanController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            this._loanService = loanService;
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
            var loans = _loanService.GetAllLoans(pageIndex: command.Page, pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                ExtraData = loans.Items.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Mobile = x.Mobile,
                    CreationTime = x.CreationTime.ToString("yyyy/MM/dd hh:mm")
                }).ToList(),
            };
            return AbpJson(jsonData);
        }
        #endregion
    }
}