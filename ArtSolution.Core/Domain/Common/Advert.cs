using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Common
{
    /// <summary>
    /// 首页广告模块
    /// </summary>
    public class Advert : Entity, ICreationAudited
    {
        /// <summary>
        /// 广告名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 广告图片
        /// </summary>
        [Required, MaxLength(500)]
        public string AdvertImage { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        [Required, MaxLength(500)]
        public string AdvertUrl { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public int DisplayOrder { get; set; }


        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
