using System;
using System.Collections.Generic;
using System.Linq;
using ArtSolution.Domain.Orders;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;

namespace ArtSolution.Orders
{
    public class ShopppingCartService : ArtSolutionAppServiceBase, IShopppingCartService
    {
        #region Ctor && Field

        private readonly IRepository<ShoppingCartItem> _cartRepository;

        public ShopppingCartService(IRepository<ShoppingCartItem> cartRepository)
        {
            this._cartRepository = cartRepository;
        }


        #endregion

        #region Method
        public void ClearCart(int customerId)
        {
            if (customerId > 0)
                _cartRepository.Delete(x => x.CustomerId == customerId);
        }

        public void DeleteCart(int cartId)
        {
            try
            {
                _cartRepository.Delete(cartId);
            }
            catch { }
        }

        public IPagedResult<ShoppingCartItem> GetAllShoppingItems(int customerId = 0,
            int productId = 0, 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _cartRepository.GetAll();
            if (customerId > 0)
                query = query.Where(c => c.CustomerId == customerId);

            if (productId > 0)
                query = query.Where(c => productId == c.ProductId);
            
            query = query.OrderByDescending(c => c.CreationTime);

            return new PagedResult<ShoppingCartItem>(query, pageIndex, pageSize);
            throw new NotImplementedException();
        }

        public ShoppingCartItem GetCartById(int cartId)
        {
            try
            {
                return _cartRepository.Get(cartId);
            }
            catch
            {
                return null;
            }
        }

        public void InsertCart(ShoppingCartItem cart)
        {
            try
            {
                _cartRepository.Insert(cart);
            }
            catch
            {
            }
        }

        public void UpdateCart(ShoppingCartItem cart)
        {
            try
            {
                _cartRepository.Update(cart);
            }
            catch
            {
            }
        }

        #endregion
    }
}
