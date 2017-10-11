using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.Orders
{
    /// <summary>
    /// 收藏夹
    /// </summary>
    public class Favorite : Entity, IHasCreationTime
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 收藏的商品
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required, MaxLength(50)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string ProductImage { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
