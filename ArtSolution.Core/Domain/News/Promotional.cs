using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.News
{
    /// <summary>
    /// 促销专题
    /// </summary>
    public class Promotional : Entity, IHasCreationTime
    {

        [Required, MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(500)]
        public string PromotionalImage { get; set; }

        [MaxLength(200)]
        public string Keywords { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }


        public bool Published { get; set; }
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        [Required,MaxLength(20)]
        public string PromotionalPath { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
