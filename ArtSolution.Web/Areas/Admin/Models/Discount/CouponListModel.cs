using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Areas.Admin.Models.Discount
{
    public class CouponListModel
    {
        [DisplayName("关键字")]
        public string Keywords { get; set; }

        [DisplayName("是否使用")]
        public bool? Used { get; set; }
    }
}