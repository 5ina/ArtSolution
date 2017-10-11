using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.Catalog
{
    public class Category:Entity,ISoftDelete, ICreationAudited,IModificationAudited
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }


        [MaxLength(500)]
        public string MetaKeywords { get; set; }
        [MaxLength(500)]
        public string MetaTitle { get; set; }
        [MaxLength(500)]
        public string MetaDescription { get; set; }

        public int ParentId { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
