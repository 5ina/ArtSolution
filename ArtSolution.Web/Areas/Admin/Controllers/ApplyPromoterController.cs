using Abp.AutoMapper;
using Abp.Domain.Uow;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using ArtSolution.Web.Areas.Admin.Models.Customers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class ApplyPromoterController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly IApplyPromoterService _applyService;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ApplyPromoterController(IApplyPromoterService applyService,
            ICustomerService customerService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this._applyService = applyService;
            this._customerService = customerService;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            var model = new ApplyPromoterListModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, ApplyPromoterListModel model)
        {
            var promoters = _applyService.GetAllList(keywords: model.Keywords,
                                        audit:model.Audit,
                                        createdFrom:model.StartDate,
                                        createdTo:model.EndDate,
                                        pageIndex: command.Page,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                ExtraData = promoters.Items.Select(b => new
                {
                    Id = b.Id,
                    NickName = b.NickName,
                    Mobile = b.Mobile,
                    CreationTime = b.CreationTime.ToString("yyyy/mm/dd")
                }),
            };
            return AbpJson(jsonData);
        }


        public ActionResult Edit(int id)
        {
            var promoter = _applyService.GetPromoterById(id);
            var model = promoter.MapTo<ApplyPromoterModel>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ApplyPromoterModel model)
        {
            var promoter = _applyService.GetPromoterById(model.Id);
            promoter = model.MapTo<ApplyPromoterModel, ApplyPromoter>(promoter);
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                _applyService.Update(promoter);
                if (promoter.Audit)
                {
                    var customer = _customerService.GetCustomerId(promoter.CustomerId);
                    customer.SaveCustomerAttribute<bool>(CustomerAttributeNames.IsPromoter, true);
                }

                unitOfWork.Complete();
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}