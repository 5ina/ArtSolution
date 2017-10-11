using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Domain.official
{
    public class MessageBoard:Entity, IHasCreationTime
    {
        [Required, MaxLength(6)]
        public string Name { get; set; }
        [Required, MaxLength(60)]
        public string Mobile { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
