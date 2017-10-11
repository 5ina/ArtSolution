using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    public class Brand : Entity, ISoftDelete, ICreationAudited
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [ MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int DisplayOrder { get; set; }

        [MaxLength(500)]
        public string BrandImage { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
