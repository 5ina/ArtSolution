using Abp.Domain.Uow;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using ArtSolution.Web.Areas.Admin.Models.Customers;
using ArtSolution.Web.Extensions;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 提现申请控制器
    /// </summary>
    public class ApplyCashController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly IApplyCashService _cashService;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ApplyCashController(IApplyCashService cashService,
            ICustomerService customerService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this._cashService = cashService;
            this._customerService = customerService;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new ApplyCashListModel();
            model.AvailableAudits =  AuditStatus.None.EnumToDictionary(o => o.GetDescription(), false).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ApplyCashListModel model)
        {

            var cashs = _cashService.GetAllApplyCashs(
                                                audit: model.AuditId,
                                                createdFrom: model.StartDate,
                                                createdTo: model.EndDate,
                                                pageIndex: command.Page,
                                                pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = cashs.Items.Select(b => new
                {
                    Id = b.Id,
                    Audit = ((AuditStatus)b.Audit).GetDescription(),
                    Amount = b.Amount,
                    Allowance = b.Allowance,
                    CreationTime = b.CreationTime.ToString("yyyy/mm/dd"),
                    NickName = GetCustomerInfo(b.CustomerId).NickName,
                    Mobile = GetCustomerInfo(b.CustomerId).Mobile
                }),
            };
            return AbpJson(jsonData);
        }

        private Customer GetCustomerInfo(int customerId)
        {
            return _customerService.GetCustomerId(customerId);
        }
        #endregion
    }
}