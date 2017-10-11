using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 签到表
    /// </summary>
    public class SignLog : Entity, IHasCreationTime
    {
        /// <summary>
        /// 签到日期
        /// </summary>
        [Required]
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 签到人
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 连续签到次数(包含当前次数）
        /// </summary>
        public int NumberEntries { get; set; }

        /// <summary>
        /// 奖励值
        /// </summary>
        public int Reward { get; set; }
    }
}
