using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ArtSolution.Orders
{
    public class OrderService : ArtSolutionAppServiceBase, IOrderService
    {

        #region Ctor && Field

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _itemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public OrderService(IRepository<Order> orderRepository, 
            IRepository<OrderItem> itemRepository,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._orderRepository = orderRepository;
            this._itemRepository = itemRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }


        #endregion

        #region Method

        #region Order

        public void DeleteOrder(int orderId)
        {
            _orderRepository.Delete(orderId);
        }
        public IPagedResult<Order> GetAllOrders(int customerId = 0,
            DateTime? createdFrom = null,
            DateTime? createdTo = null,
            string keywords = null,
            bool showHidden = false,
            int orderStatus = 0, List<int> orderStatusIds = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (_unitOfWorkManage.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = _orderRepository.GetAll();
                if (customerId > 0)
                    query = query.Where(o => o.CustomerId == customerId);

                if (createdFrom.HasValue)
                    query = query.Where(c => createdFrom.Value <= c.CreationTime);
                if (createdTo.HasValue)
                    query = query.Where(c => createdTo.Value >= c.CreationTime);

                if (!String.IsNullOrWhiteSpace(keywords))
                    query = query.Where(o => o.OrderSn.Contains(keywords) || o.OrderRemarks.Contains(keywords));

                if (!showHidden)
                    query = query.Where(o => !o.IsDeleted);

                if (orderStatus > 0)
                    query = query.Where(o => o.OrderStatusId == orderStatus);

                if (orderStatusIds != null && orderStatusIds.Any())
                    query = query.Where(o => orderStatusIds.Contains(o.OrderStatusId));


                query = query.OrderByDescending(o => o.Id);
                return new PagedResult<Order>(query, pageIndex, pageSize);
            }
        }

        public Order GetOrderById(int orderId)
        {
            return _orderRepository.Get(orderId);
        }

        public int InsertOrder(Order order)
        {
            try
            {
                return _orderRepository.InsertAndGetId(order);
            }
            catch {
                return 0;
            }
        }
        public void UpdateOrder(Order order)
        {
            try
            {
                _orderRepository.Update(order);
            }
            catch
            {
            }
        }
        public Order GetOrderByOrderSn(string orderSn)
        {
            return _orderRepository.FirstOrDefault(o => o.OrderSn == orderSn);
        }

        #endregion

        #region Order Items

        public void DeleteOrderItem(int itemId)
        {
            _itemRepository.Delete(itemId);
        }


        public OrderItem GetOrderItemById(int itemId)
        {
            if (itemId > 0)
                return _itemRepository.Get(itemId);
            return null;
        }

        public IList<OrderItem> GetOrderItems(int orderId = 0, int productId = 0)
        {
            var query = _itemRepository.GetAll();
            if (orderId > 0)
                query = query.Where(x => x.OrderId == orderId);
            
            if(productId>0)
                query = query.Where(x => x.ProductId == productId);

            query = query.OrderByDescending(i => i.Id);

            return query.ToList();
        }

        public void InsertOrderItems(OrderItem item)
        {
            try
            {
                if (item.OrderId > 0)
                    _itemRepository.Insert(item);
            }
            catch
            {
            }
        }


        public void UpdateOrderItems(OrderItem item)
        {
            _itemRepository.Update(item);
        }

        public decimal GetAmountByCustomerIds(int[] customerIds)
        {
            var query = _orderRepository.GetAll();

            query = query.Where(o => customerIds.Contains(o.CustomerId));

            return query.Sum(o => (decimal?)o.Subtotal) ?? 0;
        }

        #endregion

        #endregion
    }
}
