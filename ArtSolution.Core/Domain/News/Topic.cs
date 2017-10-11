using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.News
{
    public class Topic : Entity, ICreationAudited
    {
        [Required, MaxLength(60)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        [MaxLength(50)]
        public string SystemName { get; set; }

        /// <summary>
        /// 短URL
        /// </summary>
        public string ShortUrl { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
