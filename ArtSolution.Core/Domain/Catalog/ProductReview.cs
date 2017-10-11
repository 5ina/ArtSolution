using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    public class ProductReview :Entity , ICreationAudited
    {
       
        /// <summary>
        /// 所属订单子表ID
        /// </summary>
        public int OrderItemId { get; set; }
        [MaxLength(30)]
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 所属商品
        /// </summary>
        public int ProductId { get; set; }

        [MaxLength(500)]
        public string Images { get; set; }

        [MaxLength(300)]
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

    }
}
