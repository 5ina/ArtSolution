using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.News
{
    [AutoMap(typeof(Topic))]
    public class TopicModel : EntityDto
    {
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DisplayName("内容")]
        [UIHint("RichEditor")]
        public string Content { get; set; }

        [DisplayName("系统名称")]
        public string SystemName { get; set; }

        /// <summary>
        /// 短URL
        /// </summary>
        [DisplayName("URL")]
        public string ShortUrl { get; set; }
    }
}