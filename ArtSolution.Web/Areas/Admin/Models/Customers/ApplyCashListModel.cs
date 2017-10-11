using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// 提现申请表
    /// </summary>
    public class ApplyCashListModel
    {
        public ApplyCashListModel() {

            AvailableAudits = new List<SelectListItem>();
        }

        [DisplayName("开始时间")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [DisplayName("结束时间")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [DisplayName("审核状态")]
        public int? AuditId { get; set; }
        public IList<SelectListItem> AvailableAudits { get; set; }
    }
}