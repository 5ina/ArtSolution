using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Models.Customers
{

    [AutoMap(typeof(WishOrder))]
    public class WishOrderModel : EntityDto
    {
        /// <summary>
        /// 填写用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        [DisplayName("品牌名称")]
        [Required, MaxLength(60)]
        public string BrandName { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [DisplayName("商品名称")]
        [Required, MaxLength(160)]
        public string ProductName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [DisplayName("商品图片")]
        [UIHint("Camera")]
        public string ProductImages { get; set; }

        public DateTime CreationTime { get; set; }

    }
}