using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;
using System;
using System.Collections.Generic;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 订单服务接口
    /// </summary>
    public interface IOrderService : IApplicationService
    {
        #region Order
        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        int InsertOrder(Order order);

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrder(Order order);

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderId"></param>
        void DeleteOrder(int orderId);

        /// <summary>
        /// 根据主键获取订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order GetOrderById(int orderId);

        /// <summary>
        /// 根据订单编号获取订单信息
        /// </summary>
        /// <param name="orderSn"></param>
        /// <returns></returns>
        Order GetOrderByOrderSn(string orderSn);
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="keywords"></param>
        /// <param name="showHidden"></param>
        /// <param name="orderStatus"></param>        
        /// <param name="orderStatusIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Order> GetAllOrders(int customerId = 0, DateTime? createdFrom = null,
            DateTime? createdTo = null, string keywords = null,
            bool showHidden = false, int orderStatus = 0, List<int> orderStatusIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion


        #region Order Items
        /// <summary>
        /// 新增订单商品
        /// </summary>
        /// <param name="item"></param>
        void InsertOrderItems(OrderItem item);

        /// <summary>
        /// 更新订单商品
        /// </summary>
        /// <param name="item"></param>
        void UpdateOrderItems(OrderItem item);

        /// <summary>
        /// 删除订单商品
        /// </summary>
        /// <param name="itemId"></param>
        void DeleteOrderItem(int itemId);

        /// <summary>
        /// 根据主键获取订单商品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        OrderItem GetOrderItemById(int itemId);

        /// <summary>
        /// 获取所有订单商品
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<OrderItem> GetOrderItems(int orderId = 0, int productId = 0);


        /// <summary>
        /// 获取总金额
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        decimal GetAmountByCustomerIds(int[] customerIds);
        #endregion
    }
}
