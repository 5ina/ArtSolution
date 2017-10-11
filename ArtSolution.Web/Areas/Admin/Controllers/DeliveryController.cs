using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using ArtSolution.Orders;
using ArtSolution.Web.Areas.Admin.Models.Orders;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class DeliveryController : ArtSolutionControllerAdminBase
    {

        #region ctor && Fields
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            this._deliveryService = deliveryService;
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
            var deliveries = _deliveryService.GetAllDeliveries(showHidden: true,
                pageIndex: command.Page,
                pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                ExtraData = deliveries.Items.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayOrder = x.DisplayOrder,
                    Active = x.Active
                }).ToList(),
            };
            return AbpJson(jsonData);
        }


        public ActionResult Create()
        {
            var model = new DeliveryModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(DeliveryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Delivery>();
                _deliveryService.InsertDelivery(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var delivery = _deliveryService.GetDelivery(id);
            var model = delivery.MapTo<DeliveryModel>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DeliveryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _deliveryService.GetDelivery(model.Id);
                entity = model.MapTo<DeliveryModel, Delivery>(entity);
                _deliveryService.UpdateDelivery(entity);
                return RedirectToAction("List");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int deliveryId)
        {
            try
            {
                _deliveryService.DeleteDelivery(deliveryId);
                return Json("ok");
            }
            catch {
                return Json("error");
            }
        }

        #endregion
    }
}