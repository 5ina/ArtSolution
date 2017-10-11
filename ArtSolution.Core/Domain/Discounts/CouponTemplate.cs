using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Discounts
{
    public class CouponTemplate:Entity
    {
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        public int EffectiveDays { get; set; }

        /// <summary>
        /// 是否当前日期
        /// </summary>
        public bool IsCurrentDate { get; set; }

        /// <summary>
        /// 开始启用日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束使用日期（不填写为永久性)
        /// </summary>
        public DateTime? EndTime { get; set; }


        [Required, MaxLength(20)]
        public string CouponTemplateName { get; set; }

    }
}
