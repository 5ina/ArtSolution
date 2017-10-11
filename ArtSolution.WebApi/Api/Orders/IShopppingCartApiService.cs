using Abp.Application.Services.Dto;
using ArtSolution.Api.Models.Orders;
using System.Collections.Generic;
using System.Web.Http;

namespace ArtSolution.Api.Orders
{
    public interface IShopppingCartApiService :IApiService
    {
        /// <summary>
        /// 新增购物车
        /// </summary>
        /// <param name="cart"></param>
        [HttpPost]
        void InsertCart([FromBody] ShoppingCartItemModel cart);

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="cart"></param>
        [HttpPost]
        void UpdateCart([FromBody] ShoppingCartItemModel cart);

        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="cartId"></param>
        [HttpPost]
        void DeleteCart([FromBody] int cartId);

        /// <summary>
        /// 清空用户购物车
        /// </summary>
        /// <param name="customerId"></param>
        [HttpPost]
        void ClearCart([FromBody] int customerId);

        /// <summary>
        /// 根据主键获取购物车
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<ShoppingCartItemModel> GetCartById(int cartId);

        /// <summary>
        /// 查询所有购物车
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<ShoppingCartItemModel> GetAllShoppingItems(int customerId = 0, int productId = 0, int pageIndex = 0, int pageSize = 0);

    }
}
