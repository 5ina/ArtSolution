using Abp.AutoMapper;
using ArtSolution.Discount;
using ArtSolution.Domain.Discounts;
using ArtSolution.Web.Areas.Admin.Models.Discount;
using ArtSolution.Web.Extensions;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 优惠券控制器
    /// </summary>
    public class CouponController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            this._couponService = couponService;
        }
        #endregion

        #region Method
        public ActionResult CreateCoupon()
        {
            var model = new CouponModel();
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult CreateCoupon(CouponModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Coupon>();
                entity.DiscountCode = CommonHelper.GenerateCouponCode();
                entity.Id = _couponService.InsertCoupon(entity);
                return continueEditing ? RedirectToAction("EditCoupon", new { id = entity.Id }) : RedirectToAction("List");
            }            
            return View(model);
        }

        public ActionResult EditCoupon(int id)
        {
           var entity = _couponService.GetCouponById(id);
            var model = entity.MapTo<CouponModel>();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult EditCoupon(CouponModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Coupon>();
                if (String.IsNullOrWhiteSpace(entity.DiscountCode))
                {
                    entity.DiscountCode = CommonHelper.GenerateCouponCode();
                }
                _couponService.UpdateCoupon(entity);
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }
            return View(model);
        }

        public ActionResult List()
        {
            var model = new CouponListModel();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult List(DataSourceRequest command, CouponListModel model)
        {
            var coupons = _couponService.GetAllCoupons(
                                                        keywords: model.Keywords,
                                                        used:model.Used,
                                                        pageIndex: command.Page - 1,
                                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = coupons.Items.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Effective = c.Effective.HasValue ? c.Effective.Value.ToString("yyyy-MM-dd") : "-- / --",
                    Amount = c.Amount,
                    Used = c.Used,
                    CreationTime = c.CreationTime,
                }).ToList(),
                Total = coupons.TotalCount
            };

            return AbpJson(jsonData);
        }
        
        
        #endregion
    }
}