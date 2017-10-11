using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.News
{
    public class NewItem:Entity,IHasCreationTime, ISoftDelete
    {
        [Required, MaxLength(60)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string ShortDescription { get; set; }


        [Required, MaxLength(200)]
        public string MetaKeywords { get; set; }
        [Required, MaxLength(500)]
        public string MetaDescription { get; set; }
        [Required, MaxLength(200)]
        public string MetaTitle { get; set; }
        
        public string FullDescription { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Required]
        public DateTime PublishDateTime { get; set; }
        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
