using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 提现记录申请表
    /// </summary>
    public class ApplyCash : Entity, IHasCreationTime
    {
        public int CustomerId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal Allowance { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Audit { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        [MaxLength(80)]
        public string AuditReason { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
