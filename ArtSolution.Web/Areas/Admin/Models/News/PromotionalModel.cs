using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.News
{
    [AutoMap(typeof(Promotional))]
    public class PromotionalModel : EntityDto
    {
        [DisplayName("专题名称")]
        public string Name { get; set; }
        [DisplayName("专题图片")]
        [UIHint("PictureUrl")]
        public string PromotionalImage { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("关键字")]
        public string Keywords { get; set; }
        [DisplayName("说明")]
        public string Description { get; set; }


        [DisplayName("是否发布")]
        public bool Published { get; set; }
        [DisplayName("发布时间")]
        [UIHint("Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("结束时间")]
        [UIHint("Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("促销专题路径")]
        public string PromotionalPath { get; set; }
    }
}