using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Discounts;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Discount
{

    [AutoMap(typeof(Coupon))]
    public class CouponModel : EntityDto
    {

        /// <summary>
        /// 优惠券名称
        /// </summary>
        [DisplayName("优惠券名称")]
        public string Name { get; set; }

        /// <summary>
        /// 有效时间（不填写表示永久有效）
        /// </summary>
        [DisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime Effective { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DisplayName("金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 是否使用（未生成List显示为False）
        /// </summary>
        public bool Used { get; set; }


        /// <summary>
        /// 所属用户
        /// </summary>
        [DisplayName("所属用户")]
        public int CustomerId { get; set; }
        /// <summary>
        /// 所用订单
        /// </summary>
        public int OrderId { get; set; }


        /// <summary>
        /// 优惠券码
        /// </summary>
        public string DiscountCode { get; set; }        

    }
}