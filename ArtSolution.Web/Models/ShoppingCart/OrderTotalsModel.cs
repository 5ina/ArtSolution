using System.Collections.Generic;

namespace ArtSolution.Web.Models.ShoppingCart
{
    public partial class OrderTotalsModel 
    {
        public bool IsEditable { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Shipping { get; set; }
                
        /// <summary>
        /// 总计
        /// </summary>
        public decimal OrderTotal { get; set; }
        
        public decimal OrderTotalDiscount { get; set; }
    }
}