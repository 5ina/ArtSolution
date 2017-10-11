using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ArtSolution.Web.Models.News
{

    [AutoMap(typeof(Domain.News.Topic))]
    public class TopicModel : EntityDto
    {
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public string SystemName { get; set; }
        
        public string ShortUrl { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}