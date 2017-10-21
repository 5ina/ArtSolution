using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Orders
{
    public class Order :Entity,ISoftDelete, IHasCreationTime, IModificationAudited
    {
        [Required, MaxLength(50)]
        public string OrderSn { get; set; }
        
        public int CustomerId { get; set; }
                
        /// <summary>
        /// 配送地址
        /// </summary>
        public int Billing { get; set; }
        public int OrderStatusId { get; set; }
        
        /// <summary>
        /// 商品小计
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// 运输费用
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal OrderTotal { get; set; }

        [MaxLength(500)]
        public string OrderRemarks { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        public int CouponId { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal Preferential { get; set; }
        /// <summary>
        /// 是否积分订单
        /// </summary>
        public bool IsRewardOrder { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }


    }
}
