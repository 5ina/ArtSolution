using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 推广人列表
    /// </summary>
    public class ApplyPromoter : Entity, IHasCreationTime
    {

        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }

        public int CustomerId { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 审核是否通过
        /// </summary>
        public bool Audit { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        [MaxLength(200)]
        public string AuditReason { get; set; }

    }
}