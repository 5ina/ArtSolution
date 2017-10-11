using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Common
{
    public class AdvertListModel
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        [DisplayName("开始时间")]
        [UIHint("DateNullable")]
        public DateTime? ShowFrom { get; set; }

        [DisplayName("结束时间")]
        [UIHint("DateNullable")]
        public DateTime? ShowTo { get; set; }
    }
}