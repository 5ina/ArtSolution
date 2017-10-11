using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.News
{
    /// <summary>
    /// 愿望单
    /// </summary>
    public class WishOrder : Entity, IHasCreationTime
    {
        /// <summary>
        /// 填写用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        [Required, MaxLength(60)]
        public string BrandName { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required, MaxLength(160)]
        public string ProductName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [Required, MaxLength(500)]
        public string ProductImages { get; set; }

        public DateTime CreationTime { get; set; }

    }
}