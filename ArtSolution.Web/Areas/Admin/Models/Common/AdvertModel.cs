using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Common
{
    [AutoMap(typeof(Advert))]
    public class AdvertModel : EntityDto
    {
        [DisplayName("广告标题")]
        public string Name { get; set; }
        
        /// <summary>
        /// 广告图片
        /// </summary>
        [DisplayName("广告图片")]
        [UIHint("PictureUrl")]
        public string AdvertImage { get; set; }

        [DisplayName("链接地址")]
        public string AdvertUrl { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("开始时间")]
        [UIHint("DateNullable")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("结束时间")]
        [UIHint("DateNullable")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        [DisplayName("权重")]
        [UIHint("DisplayOrder")]
        public int DisplayOrder { get; set; }        
    }
}