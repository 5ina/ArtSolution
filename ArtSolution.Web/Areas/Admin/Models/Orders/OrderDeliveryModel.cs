using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// 发货
    /// </summary>
    public class OrderDeliveryModel
    {
        public OrderDeliveryModel()
        {
            this.AvailableDeliveries = new List<SelectListItem>();
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        [DisplayName("配送方式")]
        public int DeliveryId { get; set; }

        /// <summary>
        /// 快递号
        /// </summary>
        [DisplayName("快递号")]
        public string DeliveryCode { get; set; }

        public IList<SelectListItem> AvailableDeliveries { get; set; }
    }
}