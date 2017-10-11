using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Orders
{
    public class ReturnOrder : Entity, IHasCreationTime
    {
        /// <summary>
        /// 所属订单
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>
        [Required, MaxLength(200)]
        public string ReasonForReturn { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public int AuditStatus { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        [MaxLength(50)]
        public string AuditReason { get; set; }
    }
}