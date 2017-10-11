using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.News
{

    [AutoMap(typeof(NewItem))]
    public class NewItemModel : EntityDto
    {
        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("短介绍")]
        public string ShortDescription { get; set; }

        [DisplayName("Meta关键字")]
        public string MetaKeywords { get; set; }
        [DisplayName("Meta说明")]
        public string MetaDescription { get; set; }
        [DisplayName("Meta标题")]
        public string MetaTitle { get; set; }

        [DisplayName("内容")]
        [UIHint("RichEditor")]
        public string FullDescription { get; set; }

        [DisplayName("发布时间")]
        [UIHint("DateNullable")]
        public DateTime PublishDateTime { get; set; }
    }
}