using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Messages
{
    public class Notice:Entity, IHasCreationTime
    {
        /// <summary>
        /// 发送人（0为系统管理员）
        /// </summary>
        public int FromId { get; set; }

        /// <summary>
        /// 接收人（0为系统管理员）
        /// </summary>
        public int ToId { get; set; }

        public bool IsRead { get; set; }

        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }


        [Required, MaxLength(500)]
        public string Message { get; set; }
    }
}
