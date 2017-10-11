using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Catalog;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Orders
{

    [AutoMap(typeof(ProductReview))]
    public class ProductReviewModel : EntityDto
    {
        /// <summary>
        /// 所属订单子表ID
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// 所属商品
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 评分（最高5分）
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 评论人
        /// </summary>
        public string CustomerName { get; set; } 
        public int CustomerId { get; set; }
        public List<string> Images { get; set; }
        
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}