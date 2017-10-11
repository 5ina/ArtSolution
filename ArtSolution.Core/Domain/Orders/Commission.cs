using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtSolution.Domain.Orders
{
    /// <summary>
    /// 返佣列表
    /// </summary>
    public class Commission : Entity, IHasCreationTime
    {
        /// <summary>
        /// 返佣的用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 返佣关联的订单
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 订单总价（不含运费）
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 返佣的金额
        /// </summary>
        public decimal ReturnAmount { get; set; }
        
        /// <summary>
        /// 返佣率
        /// </summary>
        public decimal Rate { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
