using Abp.Web.Security.AntiForgery;
using ArtSolution.Catalog;
using ArtSolution.Domain.Orders;
using ArtSolution.Orders;
using ArtSolution.Web.Models.ShoppingCart;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ArtSolution.Common;
using ArtSolution.Names;
using ArtSolution.Customers;
using ArtSolution.Web.Framework.Security;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Web.Controllers
{
    public class ShoppingCartController : ArtSolutionControllerBase
    {
        #region ctor && Fields
        private readonly IShopppingCartService _cartService;
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly IProductAttributeService _attributeService;
        private readonly ICustomerService _customerService;


        public ShoppingCartController(IShopppingCartService cartService,
            ISettingService settingService,
            IProductService productService,
            IProductAttributeService attributeService,
        ICustomerService customerService)
        {
            this._cartService = cartService;
            this._productService = productService;
            this._attributeService = attributeService;
            this._settingService = settingService;
            this._customerService = customerService;
        }
        #endregion

        #region Utilities
        [NonAction]
        public void PrepareShoppingCartModel(ShoppingCartModel model,
            IList<ShoppingCartItem> cart, bool isEditable = true)
        {

            if (cart == null)
                throw new ArgumentNullException("cart");

            if (model == null)
                throw new ArgumentNullException("model");
            
            if (cart.Count == 0)
                return;

            #region 属性
            model.IsEditable = isEditable;
            model.OrderFreeShip = _settingService.GetSettingByKey<decimal>(OrderSettingNames.OrderFreeShip);
            #endregion

            #region Cart items

            foreach (var sci in cart)
            {
                var product = _productService.GetProductById(sci.ProductId);
                ProductAttribute attribute = null;

                var cartItemModel = new ShoppingCartModel.ShoppingCartItemModel
                {
                    Id = sci.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Picture = product.ProductImage,
                    Quantity = sci.Quantity,
                    SubTotal = sci.Quantity * sci.Price,
                    UnitPrice = sci.Price,
                    ProductImage = product.ProductImage,
                    MaxStockQuantity = product.StockQuantity,
                };
                model.Items.Add(cartItemModel);
            }

            #endregion
        }

        
        [NonAction]
        protected virtual OrderTotalsModel PrepareOrderTotalsModel(IList<ShoppingCartItem> cart, bool isEditable)
        {
            var model = new OrderTotalsModel();
            model.IsEditable = isEditable;

            if (cart.Count > 0)
            {
                //subtotal

                model.SubTotal = cart.Sum(c => c.Price * c.Quantity);
                model.Shipping = _settingService.GetSettingByKey<decimal>(OrderSettingNames.ShipFee);
                var minTotalFreeShip = _settingService.GetSettingByKey<decimal>(OrderSettingNames.OrderFreeShip);
                if (model.SubTotal > minTotalFreeShip)
                    model.OrderTotalDiscount = model.Shipping;

                //total
                model.OrderTotal = model.SubTotal + model.Shipping - model.OrderTotalDiscount;
            }
            return model;
        }


        #endregion

        #region method
        /// <summary>
        /// 我的购物车
        /// </summary>
        /// <returns></returns>
        [CustomerAntiForgery]
        public ActionResult MyCarts()
        {
            var currentId = Convert.ToInt32(AbpSession.UserId);
            var carts = _cartService.GetAllShoppingItems(currentId);
            
            var model = new ShoppingCartModel();
            PrepareShoppingCartModel(model, carts.Items.ToList());
            return View(model);
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="qty">更新的数量</param>
        /// <param name="CartId">购物车Id</param>
        /// <returns></returns>
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult UpdateQuantity(int qty, int CartId)
        {
            var cart = _cartService.GetCartById(CartId);
            cart.Quantity = qty;
            return AbpJson(null);
        }
        

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult AddToCart(int productId,int qty)
        {
            var customerId = AbpSession.UserId.HasValue ? Convert.ToInt32(AbpSession.UserId) : 0;
            if (customerId == 0)
                return AjaxResult(Framework.UIEnums.AjaxResultStatus.Error, "请在微信中操作");

            var product = _productService.GetProductById(productId);

            if(product == null)
                return AjaxResult(Framework.UIEnums.AjaxResultStatus.Error, "该商品不存在");

            var carts = _cartService.GetAllShoppingItems(customerId: customerId,
                    productId: productId);
            ShoppingCartItem cart = null;

            if (carts.TotalCount == 0)
            {
                cart = new ShoppingCartItem
                {
                    CustomerId = customerId,
                    Price = product.Price,
                    ProductId = product.Id,
                    Quantity = qty,
                    SpecialPrice = product.SpecialPrice,
                    SpecialPriceEndDateTime = product.SpecialPriceEndDateTime,
                    SpecialPriceStartDateTime = product.SpecialPriceStartDateTime,
                    CreationTime = DateTime.Now,
                    PreSell = product.PreSell,
                };
                _cartService.InsertCart(cart);
            }
            else
            {
                cart.Quantity = cart.Quantity + qty;
                cart.CreationTime = DateTime.Now;
                _cartService.UpdateCart(cart);
            }
            return AjaxResult(Framework.UIEnums.AjaxResultStatus.Success, "上仙！您的商品已经到了购物车了！");
            
        }

        [HttpPost]
        public ActionResult ToCart(int productId, int qty)
        {
            var product = _productService.GetProductById(productId);
            try
            {
                var products = _cartService.GetAllShoppingItems(customerId: Convert.ToInt32(AbpSession.UserId),
                                                 productId: productId);
                
               
                if (products.Items.Count > 0)
                {
                    var cart = products.Items.FirstOrDefault(x => x.ProductId == productId);
                    cart.Quantity += 1;
                    _cartService.UpdateCart(cart);
                    return JavaScript(Framework.HtmlMessageTypeEnum.Timer, "加入购物车", "您的购物车已经更新", 1500);
                }

                _cartService.InsertCart(new ShoppingCartItem
                {
                    CustomerId = Convert.ToInt32(AbpSession.UserId),
                    Price = product.Price,
                    ProductId = product.Id,
                    Quantity = qty,
                    SpecialPrice = product.SpecialPrice,
                    SpecialPriceEndDateTime = product.SpecialPriceEndDateTime,
                    SpecialPriceStartDateTime = product.SpecialPriceStartDateTime,
                    CreationTime = DateTime.Now,
                    PreSell = product.PreSell,
                });
                return JavaScript(Framework.HtmlMessageTypeEnum.Timer, "加入购物车", "该商品已经添加到购物车", 1500); ;
            }
            catch (Exception ex){
                return Content(ex.Message);
            }
        }
        

        [ChildActionOnly]
        public ActionResult OrderTotals(bool isEditable)
        {
            var currentId = Convert.ToInt32(AbpSession.UserId);
            var carts = _cartService.GetAllShoppingItems(currentId);
            var model = PrepareOrderTotalsModel(carts.Items.ToList(), isEditable);
            return PartialView(model);
        }


        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult DeleteCartItem(int cartId)
        {
            var cart = _cartService.GetCartById(cartId);
            if (cart.CustomerId == this.CustomerId)
            {
                _cartService.DeleteCart(cartId);
                return Content("success");
            }
            return AjaxResult(Framework.UIEnums.AjaxResultStatus.Error, "不能删除该项");
        }
        
        #endregion
    }
}