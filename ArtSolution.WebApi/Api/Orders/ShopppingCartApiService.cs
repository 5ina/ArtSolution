using Abp.WebApi.Controllers;
using System;
using System.Collections.Generic;
using ArtSolution.Api.Models.Orders;
using ArtSolution.Catalog;
using ArtSolution.Orders;
using ArtSolution.Customers;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Api.Orders
{
    public class ShopppingCartApiService : AbpApiController, IShopppingCartApiService
    {
        #region Ctor && Field

        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IShopppingCartService _cartService;        

        public ShopppingCartApiService(IProductService productService,
                                        ICustomerService customerService,
                                        IShopppingCartService cartService)
        {
            this._cartService = cartService;
            this._productService = productService;
            this._customerService = customerService;
        }

        #endregion

        #region Method

        public void ClearCart(int customerId)
        {
            var customer = _customerService.GetCustomerId(customerId);
            if (customer == null)
                throw new Exception("用户不存在");

            _cartService.ClearCart(customerId);
        }

        public void DeleteCart(int cartId)
        {
            _cartService.DeleteCart(cartId);
        }

        public ResultMessage<ShoppingCartItemModel> GetAllShoppingItems(int customerId = 0, int productId = 0, int pageIndex = 0, int pageSize = 0)
        {
            var carts = _cartService.GetAllShoppingItems(customerId: customerId,
                productId: productId,
                pageIndex: pageIndex,
                pageSize: pageSize);

            var models = carts.Items.MapTo<List<ShoppingCartItemModel>>();
            return new ResultMessage<ShoppingCartItemModel>(models);
        }

        public ResultMessage<ShoppingCartItemModel> GetCartById(int cartId)
        {
            var cart = _cartService.GetCartById(cartId);
            if (cart == null)
                return new ResultMessage<ShoppingCartItemModel>("数据不存在");
            var model = cart.MapTo<ShoppingCartItemModel>();
            return new ResultMessage<ShoppingCartItemModel>(model);
        }

        public void InsertCart(ShoppingCartItemModel cart)
        {
            var entity = cart.MapTo<ShoppingCartItem>();
            var product = _productService.GetProductById(entity.ProductId);
            entity.SpecialPrice = product.SpecialPrice;
            entity.SpecialPriceEndDateTime = product.SpecialPriceEndDateTime;
            entity.SpecialPriceStartDateTime = product.SpecialPriceStartDateTime;
            entity.Price = product.Price;
            entity.PreSell = product.PreSell;
            _cartService.InsertCart(entity);
        }

        public void UpdateCart(ShoppingCartItemModel cart)
        {
            var entity = cart.MapTo<ShoppingCartItem>();
            var product = _productService.GetProductById(entity.ProductId);
            entity.SpecialPrice = product.SpecialPrice;
            entity.SpecialPriceEndDateTime = product.SpecialPriceEndDateTime;
            entity.SpecialPriceStartDateTime = product.SpecialPriceStartDateTime;
            entity.Price = product.Price;
            _cartService.UpdateCart(entity);
        }
        #endregion
    }
}
