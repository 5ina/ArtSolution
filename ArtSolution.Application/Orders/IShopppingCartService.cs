using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Orders
{
    public interface IShopppingCartService: IApplicationService
    {
        /// <summary>
        /// 新增购物车
        /// </summary>
        /// <param name="cart"></param>
        void InsertCart(ShoppingCartItem cart);

        /// <summary>
        /// 更新购物车
        /// </summary>
        /// <param name="cart"></param>
        void UpdateCart(ShoppingCartItem cart);

        /// <summary>
        /// 删除购物车商品
        /// </summary>
        /// <param name="cartId"></param>
        void DeleteCart(int cartId);

        /// <summary>
        /// 清空用户购物车
        /// </summary>
        /// <param name="customerId"></param>
        void ClearCart(int customerId);

        /// <summary>
        /// 根据主键获取购物车
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        ShoppingCartItem GetCartById(int cartId);

        /// <summary>
        /// 查询所有购物车
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="productId"></param>
        /// <param name="attributeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ShoppingCartItem> GetAllShoppingItems(int customerId = 0,
            int productId = 0, 
            int attributeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue);
             
    }
}
