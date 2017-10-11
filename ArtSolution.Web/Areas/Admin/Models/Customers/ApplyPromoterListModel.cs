using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    public class ApplyPromoterListModel
    {
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
        

        [DisplayName("审核状态")]
        [UIHint("MinimalSelect")]
        public bool? Audit { get; set; }

        [DisplayName("关键字")]
        public string Keywords { get; set; }

    }
}