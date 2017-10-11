using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.News
{
    public class PromotionalListModel
    {        

        public string Keywords { get; set; }

        [DisplayName("开始时间")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [DisplayName("结束时间")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
    }
}