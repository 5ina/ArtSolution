using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Orders
{
    public class OrderListModel
    {
        public OrderListModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            this.OrderStatusIds = new List<int>();
        }

        [DisplayName("关键字搜索")]
        public string Keywords { get; set; }

        [DisplayName("开始时间")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [DisplayName("结束时间")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [DisplayName("订单状态")]
        [UIHint("KendoMultipleSelect")]
        public List<int> OrderStatusIds { get; set; }
        
        public IList<SelectListItem> AvailableOrderStatuses { get; set; }
    }
}