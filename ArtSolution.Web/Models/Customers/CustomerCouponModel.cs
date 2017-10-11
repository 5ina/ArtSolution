﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Discounts;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Customers
{
    /// <summary>
    /// 用户优惠券
    /// </summary>
    [AutoMap(typeof(Coupon))]
    public class CustomerCouponModel:EntityDto
    {
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 有效时间（不填写表示永久有效）
        /// </summary>
        public DateTime? Effective { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 是否使用（未生成List显示为False）
        /// </summary>
        public bool Used { get; set; }


        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 所用订单
        /// </summary>
        public int OrderId { get; set; }


        /// <summary>
        /// 优惠券码
        /// </summary>
        public string DiscountCode { get; set; }
        
        /// <summary>
        /// 来源订单
        /// </summary>
        public int SourceOrderId { get; set; }


        public DateTime? LastModificationTime { get; set; }

        public DateTime CreationTime { get; set; }
    }
    
}