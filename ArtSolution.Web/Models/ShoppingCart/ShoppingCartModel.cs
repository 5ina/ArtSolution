using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Models.ShoppingCart
{

    public partial class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
            Items = new List<ShoppingCartItemModel>();
        }

        public bool OnePageCheckoutEnabled { get; set; }
        
        /// <summary>
        /// 是否允许比编辑
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// 配送费
        /// </summary>
        public decimal OrderFreeShip { get; set; }
        public IList<ShoppingCartItemModel> Items { get; set; }
                        
        #region Nested Classes

        /// <summary>
        /// 购物车项
        /// </summary>
        public partial class ShoppingCartItemModel : EntityDto
        {
            public string Picture { get; set; }

            public int ProductId { get; set; }

            public string ProductImage { get; set; }

            public string ProductName { get; set; }
            
            public decimal UnitPrice { get; set; }

            public decimal SubTotal { get; set; }
            
            public int Quantity { get; set; }
            /// <summary>
            /// 最大购买量
            /// </summary>
            public int MaxStockQuantity { get; set; }

        }
        
        #endregion
    }
}