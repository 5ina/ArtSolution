using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace ArtSolution.Web.Models.Orders
{
    public class ProductToOrderModel
    {
        public ProductToOrderModel()
        {
            this.AvailableCoupons = new List<SelectListItem>();
        }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductAttributeName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }


        [DisplayName("订单备注")]
        public string OrderRemarks { get; set; }

        public IList<SelectListItem> AvailableCoupons { get; set; }
        [DisplayName("优惠券")]
        public int CouponId { get; set; }

        /// <summary>
        /// 商品小计
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// 运输费用
        /// </summary>
        public decimal Freight { get; set; }
        public decimal OrderTotal { get; set; }

        public bool PreSell { get; set; }



        public int Billing { get; set; }
    }
}