using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.News
{
    /// <summary>
    /// 借款表单
    /// </summary>
    public class Loan : Entity, ICreationAudited
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required, MaxLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required, MaxLength(11)]
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        [Required, MaxLength(30)]
        public string SchoolName { get; set; }

        /// <summary>
        /// 定位位置
        /// </summary>
        [MaxLength(200)]
        public string Location { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        public string IdCardPositive { get; set; }

        public string IdCardSide { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        public int Quota { get; set; }

        public int Cycle { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public int Audit { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        [MaxLength(200)]
        public string AuditReason { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
