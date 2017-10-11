using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;
using Abp.Domain.Repositories;

namespace ArtSolution.News
{
    public class WishOrderService : ArtSolutionAppServiceBase, IWishOrderService
    {
        #region Ctor && Field

        private readonly IRepository<WishOrder> _wishRepository;
        public WishOrderService(IRepository<WishOrder> wishRepository)
        {
            this._wishRepository = wishRepository;
        }

        #endregion

        #region Method

        public void DeleteWishOrder(int orderId)
        {
            _wishRepository.Delete(orderId);
        }

        public IPagedResult<WishOrder> GetAllOrders(string keywords = null, 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _wishRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(w => w.BrandName.Contains(keywords) || w.ProductName.Contains(keywords));

            query = query.OrderByDescending(w => w.CreationTime);

            return new PagedResult<WishOrder>(query, pageIndex, pageSize); 
        }

        public WishOrder GetWishOrderById(int orderId)
        {
            if (orderId > 0)
                return _wishRepository.Get(orderId);
            return null;
        }
        
        public void InsertWishOrder(WishOrder order)
        {
            if (order != null)
                _wishRepository.Insert(order);
        }

        public void UpdateWishOrder(WishOrder order)
        {
            if (order != null)
                _wishRepository.Update(order);
        }
        #endregion
    }
}
