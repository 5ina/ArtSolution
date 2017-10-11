using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Models.Common
{
    public class AdvertModel
    {
        /// <summary>
        /// 广告名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ProductImage { get; set; }
        
        /// <summary>
        /// 广告链接
        /// </summary>
        public string AdvertUrl { get; set; }
    }
}