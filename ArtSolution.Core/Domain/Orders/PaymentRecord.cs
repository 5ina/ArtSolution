using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Orders
{
    public class PaymentRecord : Entity, IHasCreationTime
    {
        public int CustomerId { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public int Audit { get; set; }
        [MaxLength(400)]
        public string AuditReason { get; set; }

        /// <summary>
        /// 微信支付ID
        /// </summary>
        [MaxLength(40)]
        public string Transaction { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [MaxLength(200)]
        public string OpenId { get; set; }
    }
}
