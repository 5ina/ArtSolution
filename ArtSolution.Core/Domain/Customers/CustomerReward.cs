using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 用户积分历史
    /// </summary>
    public class CustomerReward :Entity, IHasCreationTime
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 增加/删除的积分（操作的）
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 总积分（操作前的）
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 积分说明
        /// </summary>
        [MaxLength(80)]
        public string Message { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}
