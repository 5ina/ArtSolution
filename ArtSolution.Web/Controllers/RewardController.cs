using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.UI;
using ArtSolution.Catalog;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Domain.Catalog;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.Orders;
using ArtSolution.Names;
using ArtSolution.Orders;
using ArtSolution.Web.Framework.WeChat;
using ArtSolution.Web.Models.Catalogs;
using ArtSolution.Web.Models.Customers;
using ArtSolution.Web.Models.Orders;
using ArtSolution.Web.Models.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class RewardController : ArtSolutionControllerBase
    { 
        #region ctor && Fields
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly ICustomerService _customerService;
        private readonly IShopppingCartService _cartService;
        private readonly ICustomerAddressService _addressService;
        private readonly IProductAttributeService _attributeService;
        private readonly IPaymentRecordService _recordService;
        private readonly ICustomerRewardService _rewardService;
        private readonly IProductImagesService _imageService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public RewardController(IOrderService orderService,
                                ICacheManager cacheManager,
                                IProductService productService,
                                ISettingService settingService,
                                ICustomerAttributeService customerAttributeService,
                                ICustomerService customerService,
                                IShopppingCartService cartService,
                                ICustomerAddressService addressService,
                                 IProductAttributeService attributeService,
                                 IPaymentRecordService recordService,
                                 ICustomerRewardService rewardService,
                                 IProductImagesService imageService,
                                IUnitOfWorkManager unitOfWorkManager)
        {
            this._orderService = orderService;
            this._cacheManager = cacheManager;
            this._productService = productService;
            this._settingService = settingService;
            this._customerAttributeService = customerAttributeService;
            this._cartService = cartService;
            this._customerService = customerService;
            this._addressService = addressService;
            this._attributeService = attributeService;
            this._recordService = recordService;
            this._rewardService = rewardService;
            this._imageService = imageService;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// 商品属性
        /// </summary>
        /// <param name="model"></param>
        [NonAction]
        protected void PrepareProductAttribute(RewardProductModel model)
        {
            var attributes = _attributeService.GetProductAttributes(model.Id);
            if (attributes.Count > 0)
            {
                model.SubProductAttributes = attributes.Select(a => a.MapTo<RewardProductModel.ProductAttributeModel>()).ToList();
            }
        }

        /// <summary>
        /// 获取用户的收货地址
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [NonAction]
        private CustomerAddressModel GetCustomerAddress(Customer customer)
        {
            var model = new CustomerAddressModel();
            model.userName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName);
            model.telNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber);
            model.provinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName);
            model.cityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName);
            model.countryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName);
            model.detailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo);
            model.Id = customer.Id;
            return model;
        }

        /// <summary>
        /// 获取订单配置信息
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private OrderSettingsModel GetOrderSettings()
        {

            var config = new OrderSettingsModel
            {
                AppId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId),
                AppSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret),
                MchId = _settingService.GetSettingByKey<string>(WeChatSettingNames.MchId),
                Notify_Url = _settingService.GetSettingByKey<string>(WeChatSettingNames.NotifyUrl),
                Key = _settingService.GetSettingByKey<string>(WeChatSettingNames.Key),
                OrderFailureTime = _settingService.GetSettingByKey<int>(OrderSettingNames.OrderFailureTime),
                ShipFee = _settingService.GetSettingByKey<int>(OrderSettingNames.ShipFee),
                OrderFreeShip = _settingService.GetSettingByKey<int>(OrderSettingNames.OrderFreeShip),
                Token = _settingService.GetSettingByKey<string>(WeChatSettingNames.Token),
            };
            return config;
        }

        /// <summary>
        /// 获取JsApi的参数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public string GetJsApiParameters(RequestHandler result, string key)
        {
            RequestHandler data = new RequestHandler(null);
            data.SetParameter("appId", result.GetParameter("appid"));
            data.SetParameter("timeStamp", CommonHelper.GetTimeStamp());
            data.SetParameter("nonceStr", CommonHelper.GenerateNonceStr());
            data.SetParameter("package", "prepay_id=" + result.GetParameter("prepay_id"));

            data.SetParameter("signType", "MD5");
            data.SetParameter("paySign", data.CreateMd5Sign(key, false)); //result.GetParameter("sign"));

            string parameters = data.FromJson();
            return parameters;
        }

        /// <summary>
        /// 获取JsApi的返回参数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        public RequestHandler GetJsApiParametersRequest(RequestHandler result, string key)
        {
            RequestHandler data = new RequestHandler(null);
            data.SetParameter("appId", result.GetParameter("appid"));
            data.SetParameter("timeStamp", CommonHelper.GetTimeStamp());
            data.SetParameter("nonceStr", CommonHelper.GenerateNonceStr());
            data.SetParameter("package", "prepay_id=" + result.GetParameter("prepay_id"));

            data.SetParameter("signType", "MD5");
            data.SetParameter("paySign", data.CreateMd5Sign(key, false)); //result.GetParameter("sign"));

            return data;
        }
        #endregion

        #region Method

        public ActionResult Exchange(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return Redirect("/");

            var model = product.MapTo<RewardProductModel>();            

            //属性
            PrepareProductAttribute(model);
           
            if (model.AllowReward)
            {
                var rate = _settingService.GetSettingByKey<int>(RewardSettingNames.PaymentRate);
                model.RewardExchange = Convert.ToInt32(model.Price * rate);
            }

            #region 用户积分
            var customer = _customerService.GetCustomerId(this.CustomerId);
            if (customer != null)
                model.CustomerReward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
            #endregion
            #region 运费计算
            var orderSetting = GetOrderSettings();
            if (orderSetting.ShipFee == 0)
                model.ShipFree = 5;
            else
                model.ShipFree = orderSetting.ShipFee;
            #endregion

            return View(model);
        }

        //生成订单
        [HttpPost]
        public ActionResult RewardToOrder(int productId, int item_num, int billing)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new UserFriendlyException("该商品不存在");
            if (!product.Published)
                throw new UserFriendlyException("该商品未上架");
            if (!product.AllowReward)
                throw new UserFriendlyException("该商品不能用于积分兑换");

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var orderSettings = GetOrderSettings();
                var order = new Order
                {
                    CreationTime = DateTime.Now,
                    Billing = billing,
                    CustomerId = this.CustomerId,
                    Freight = orderSettings.ShipFee == 0 ? 5 : orderSettings.ShipFee,
                    OrderTotal = orderSettings.ShipFee == 0 ? 5 : orderSettings.ShipFee,
                    Subtotal = orderSettings.ShipFee == 0 ? 5 : orderSettings.ShipFee,
                    IsDeleted = false,
                    IsRewardOrder = true,
                    OrderRemarks = "积分兑换",
                    OrderSn = CommonHelper.GenerateOrderSN(),
                    OrderStatusId = (int)OrderStatus.Pending,
                    CouponId = 0,
                    Preferential = 0
                };

                var orderId = _orderService.InsertOrder(order);

                _orderService.InsertOrderItems(new OrderItem
                {
                    OrderId = orderId,
                    OrderItemGuid = Guid.NewGuid(),
                    PreSell = product.PreSell,
                    Price = product.Price,
                    ProductId = product.Id,
                    ProductImage = product.ProductImage,
                    ProductName = product.Name,
                    Quantity = item_num,
                    TotalPrice = product.Price * item_num
                });
                

                unitOfWork.Complete();
            }



            return View();
        }

        /// <summary>
        /// 订单生成统一下单
        /// </summary>
        /// <param name="cartIds"></param>
        /// <returns></returns>
        public ActionResult Payment(int orderId)
        {

            return View();
        }
        #endregion
    }
}