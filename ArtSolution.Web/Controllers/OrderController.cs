using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Web.Security.AntiForgery;
using ArtSolution.Catalog;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Discount;
using ArtSolution.Domain.Catalog;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.Orders;
using ArtSolution.Names;
using ArtSolution.Orders;
using ArtSolution.Web.Areas.Admin.Models.WeChat;
using ArtSolution.Web.Extensions;
using ArtSolution.Web.Framework;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Mvc;
using ArtSolution.Web.Framework.Security;
using ArtSolution.Web.Framework.WeChat;
using ArtSolution.Web.Models.Customers;
using ArtSolution.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using static ArtSolution.Web.Framework.CacheNames;

namespace ArtSolution.Web.Controllers
{
    //[CustomerAntiForgery]
    public class OrderController : ArtSolutionControllerBase
    {
        #region ctor && Fields
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly ICustomerService _customerService;
        private readonly IShopppingCartService _cartService;
        private readonly ICouponService _couponService;
        private readonly ICustomerAddressService _addressService;
        private readonly IProductAttributeService _attributeService;
        private readonly IPaymentRecordService _recordService;
        private readonly ICustomerRewardService _rewardService;
        private readonly IComBoProductService _comboProductService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public OrderController(IOrderService orderService,
                                ICacheManager cacheManager,
                                IProductService productService,
                                ISettingService settingService,
                                ICustomerAttributeService customerAttributeService,
                                ICustomerService customerService,
                                IShopppingCartService cartService,
                                ICustomerAddressService addressService,
                                 ICouponService couponService,
                                 IProductAttributeService attributeService, 
                                 IPaymentRecordService recordService, 
                                 ICustomerRewardService rewardService,
                                 IComBoProductService comboProductService,
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
            this._couponService = couponService;
            this._attributeService = attributeService;
            this._recordService = recordService;
            this._rewardService = rewardService;
            this._comboProductService = comboProductService;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region Utilities

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
        

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        [NonAction]
        public string GetJsApiParameters(RequestHandler result,string key)
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


        [NonAction]
        public RequestHandler GetModelParameters(RequestHandler result, string key)
        {
            RequestHandler data = new RequestHandler(null);
            data.SetParameter("appId", result.GetParameter("appid"));
            data.SetParameter("timeStamp", CommonHelper.GetTimeStamp());
            data.SetParameter("nonceStr", CommonHelper.GenerateNonceStr());
            data.SetParameter("package", "prepay_id=" + result.GetParameter("prepay_id"));

            data.SetParameter("signType", "MD5");
            data.SetParameter("paySign", data.CreateMd5Sign(key)); //result.GetParameter("sign"));

            return data;
        }


        [NonAction]
        private OrderDtailModel PrepareOrderDetailModel(Order order)
        {
            OrderDtailModel model = order.MapTo<OrderDtailModel>();
            var customer = _customerService.GetCustomerId(order.CustomerId);
            model.CustomerName = customer.NickName;
            var address = _addressService.GetAddressById(order.Billing);
            if (address != null)
                model.BillingAddress = string.Format("{0}{1}{2}{3}-{4}：{5}", address.ProvinceName, address.CityName, address.CountryName, address.DetailInfo, address.UserName, address.TelNumber);
            model.OrderStatus = ((OrderStatus)order.OrderStatusId).GetDescription();

            var products = _orderService.GetOrderItems(order.Id);
            model.Products = products.Select(p => new OrderDtailModel.OrderItemsModel
            {
                Id = p.Id,
                OrderItemGuid = p.OrderItemGuid,
                Price = p.Price,
                OrderId = p.OrderId,
                ProductId = p.ProductId,
                ProductImage = p.ProductImage,
                ProductName = p.ProductName,
                Quantity = p.Quantity,
                TotalPrice = p.TotalPrice,
            }).ToList();
            return model;
        }


        [NonAction]
        private List<string> GetAdmins()
        {
            var allAdmins = _cacheManager.GetCache("store.customer.admins").Get("store.customer.admins", () => {
                return _customerService.GetAllCustomers(roleId: CustomerRole.System);
            });

            return allAdmins.Items.Select(c => c.OpenId).ToList();
        }

        #endregion

        #region Method        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartItemId"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult ConvertToOrder(int[] shoppingCartItemId)
        {
            var customerId = Convert.ToInt32(AbpSession.UserId);
            var allCarts = _cartService.GetAllShoppingItems(customerId: customerId);
            var selectedCarts = allCarts.Items.Where(c => shoppingCartItemId.Contains(c.Id)).ToList();

            #region Save Address
            var customer = _customerService.GetCustomerId(customerId);

            var userName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName);
            var tel = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber);
            var province = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName);
            var country = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName);
            var detail = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo);

            #endregion

            #region Amount

            var failureTime = _settingService.GetSettingByKey<int>(OrderSettingNames.OrderFailureTime);

            decimal OrderFreeShip = _settingService.GetSettingByKey<decimal>(OrderSettingNames.OrderFreeShip);
            decimal ShipFee = _settingService.GetSettingByKey<decimal>(OrderSettingNames.ShipFee);

            #endregion
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var total = selectedCarts.Sum(c => c.Quantity * c.Price);
                if (total < OrderFreeShip)
                    total = total + ShipFee;
                var order = new Order
                {                    
                    CreationTime = DateTime.Now,
                    CustomerId = customer.Id,
                    IsDeleted = false,
                    OrderRemarks = "",
                    OrderSn = CommonHelper.GenerateOrderSN(),
                    OrderStatusId = (int)OrderStatus.Pending,
                    OrderTotal = total,
                };
                order.Id = _orderService.InsertOrder(order);

                selectedCarts.ForEach(c =>
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        OrderItemGuid = Guid.NewGuid(),
                        Price = c.Price,
                        Quantity = c.Quantity,
                        ProductId = c.ProductId,
                        TotalPrice = c.Price * c.Quantity
                    };
                    _orderService.InsertOrderItems(orderItem);
                });


                unitOfWork.Complete();

                return View("Order");
            }
        }


        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public void SaveAddress(CustomerAddressModel model)
        {
            var customer = _customerService.GetCustomerId(Convert.ToInt32(AbpSession.UserId));
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.UserName, model.userName);
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.TelNumber, model.telNumber);
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.CityName, model.cityName);
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.ProvinceName, model.provinceName);
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.CountryName, model.countryName);
            customer.SaveCustomerAttribute<string>(CustomerAttributeNames.DetailInfo, model.detailInfo);
        }
        
        #endregion

        #region Payment 支付回调
        

        public ActionResult Notify()
        {
            Logger.Debug("这是回调接口Notify");
            var config = GetOrderSettings();
            var requestString = GetNotifyData(config);
            
            var res = XDocument.Parse(requestString);

            //通信成功
            if (res.Element("xml").Element("return_code").Value == "SUCCESS")
            {
                if (res.Element("xml").Element("result_code").Value == "SUCCESS")
                {
                    //交易成功
                    Logger.Debug("return_Code:SUCCESS");

                    string orderSn = res.Element("xml").Element("out_trade_no").Value;
                    //处理定单到数据库///////////////////////////////////////////////////////
                    
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var order = _orderService.GetOrderByOrderSn(orderSn);
                        order.OrderStatusId = (int)OrderStatus.Paid;
                        _orderService.UpdateOrder(order);

                        #region  优惠券操作
                        var coupon = _couponService.GetCouponByOrderId(order.Id);
                        if (coupon != null)
                        {
                            coupon.Used = true;
                            _couponService.UpdateCoupon(coupon);
                        }
                        #endregion

                        #region 支付列表
                        var customer = _customerService.GetCustomerId(order.CustomerId);
                        _recordService.InsertPaymentRecord(new PaymentRecord
                        {
                            CustomerId = this.CustomerId,
                            Audit = (int)AuditStatus.Audited,
                            CreationTime = DateTime.Now,
                            OpenId = customer.OpenId,
                            OrderId = order.Id,
                            PaymentAmount = Convert.ToDecimal(res.Element("xml").Element("total_fee").Value),
                            Transaction = res.Element("xml").Element("transaction_id").Value,
                            AuditReason = ""
                        });
                        //返佣操作
                        var amount = order.OrderCommission(customer, _orderService);
                        #endregion

                        //发送消息
                        SendMessageToUser(order, amount, customer);

                        //积分操作
                        #region 返还积分
                         
                        var enabledReward = _settingService.GetSettingByKey<bool>(RewardSettingNames.Enabled);
                        if (enabledReward)
                        {
                            int reward = 0;
                            //是否首次下单
                            var customerAllOrders = _orderService.GetAllOrders(customerId: this.CustomerId);
                            if (customerAllOrders.TotalCount == 1)
                            {
                                var rewardForFirstOrder = _settingService.GetSettingByKey<int>(RewardSettingNames.PointsForFirstOrder);
                                reward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
                                _rewardService.InsertRewardHistory(new CustomerReward
                                {
                                    CreationTime = DateTime.Now,
                                    CustomerId = customer.Id,
                                    Message = "首次下单额外赠送积分",
                                    Total = reward,
                                    Points = rewardForFirstOrder,
                                });

                                customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Reward, reward + rewardForFirstOrder);
                            }

                            var rate = _settingService.GetSettingByKey<int>(RewardSettingNames.ExchangeRate);

                            var rawardOrder = Convert.ToInt32((rate / 100) * order.Subtotal);
                            reward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
                            _rewardService.InsertRewardHistory(new CustomerReward
                            {
                                CreationTime = DateTime.Now,
                                CustomerId = customer.Id,
                                Message = "订单支付成功赠送积分",
                                Total = reward,
                                Points = rawardOrder,
                            });
                            customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Reward, reward + rawardOrder);

                        }
                        #endregion

                        unitOfWork.Complete();
                    }
                }
                else
                {
                    Logger.Debug("return_Code:FAIL");
                }
            }
            else
            {
                Logger.Debug("return_Code:FAIL,签名失败");
            }
            return new NullJsonResult();
        }
        [NonAction]
        public string GetNotifyData(OrderSettingsModel config)
        {
            //接收从微信后台POST过来的数据

            int requestLength = Convert.ToInt32(Request.InputStream.Length);

            byte[] buffer = new byte[requestLength];
            Request.InputStream.Read(buffer, 0, requestLength);
            var requestString = System.Text.Encoding.UTF8.GetString(buffer);
            return requestString;
        }

        public ActionResult PaySuccess()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            
            return View();
        }
                
        #endregion

        #region unifiedorder 统一下单

        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <param name="nonce">随即字符串</param>
        /// <param name="sgin">签名</param>
        /// <param name="body">说明</param>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="total_fee">总金额</param>
        [NonAction]
        public RequestHandler UnifiedOrder(string nonce,string body,string out_trade_no ,int total_fee)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            RequestHandler request = new RequestHandler(null);
            var customer = _customerService.GetCustomerId(Convert.ToInt32(AbpSession.UserId));

            var config = GetOrderSettings();

            request.SetParameter("appid", config.AppId);
            request.SetParameter("mch_id", config.MchId);
            request.SetParameter("nonce_str", nonce);
            request.SetParameter("out_trade_no", out_trade_no);
            request.SetParameter("total_fee", total_fee.ToString());
            //request.SetParameter("total_fee", "1");
            request.SetParameter("body", body);
            request.SetParameter("notify_url", config.Notify_Url);
            request.SetParameter("spbill_create_ip", Request.UserHostAddress);
            request.SetParameter("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            request.SetParameter("trade_type", "JSAPI");

            
            request.SetParameter("openid", customer.OpenId);
            //设置签名
            //var sign = request.CreateMd5Sign(config.Key, Logger);
            var sign = request.CreateMd5Sign(config.Key);
            Logger.Debug("json:" + request.FromJson());
            string response = HttpService.Post(request.ParseXML(), url, false, 6);
            var result = request.FormatSorted(response, sign);
            Logger.Debug("result:" + response);
            if (result["return_code"].ToString().ToUpper() == "SUCCESS")
            {
                request.SetParameter("prepay_id", result["prepay_id"].ToString());
            }
            else
            {
                request.SetParameter("prepay_id", result["return_code"].ToString());
            }
            
            return request;
        }
        
        #endregion

        #region  购物车 订单        

        /// <summary>
        /// 购物车生成订单
        /// </summary>
        /// <param name="cartIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateOrder(int[] cartIds)
        {
            var customer = _customerService.GetCustomerId(Convert.ToInt32(AbpSession.UserId));
            var carts = _cartService.GetAllShoppingItems(customerId: customer.Id);
            var selectedCarts = carts.Items.Where(c => cartIds.Contains(c.Id)).ToList();
            if (carts.TotalCount == 0)
                return Redirect("/");
            var orderSettigns = GetOrderSettings();
            //小计
            var subTotal = selectedCarts.Sum(c => c.SpecialPrice.HasValue ? c.SpecialPrice.Value * c.Quantity : c.Price * c.Quantity);
            var freight = orderSettigns.OrderFreeShip > subTotal ? orderSettigns.ShipFee : 0;

            var sn = CommonHelper.GenerateOrderSN();

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var order = new Order
                {
                    Billing = 0,
                    CustomerId = this.CustomerId,
                    OrderRemarks = "",
                    OrderStatusId = 10,
                    OrderTotal = subTotal + freight,
                    Freight = freight,
                    Subtotal = subTotal,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    OrderSn = sn,
                };

                order.Id = _orderService.InsertOrder(order);

                selectedCarts.ForEach(s => _orderService.InsertOrderItems(new OrderItem
                {
                    OrderId = order.Id,
                    OrderItemGuid = Guid.NewGuid(),
                    Price = s.Price,
                    ProductAttributeId = s.ProductAttributeId,
                    ProductId = s.ProductId,
                    ProductImage = _productService.GetProductById(s.ProductId).ProductImage,
                    ProductName = _productService.GetProductById(s.ProductId).Name,
                    Quantity = s.Quantity,
                    Review = false,
                    TotalPrice = s.Quantity * s.Price
                }));

                var model = order.MapTo<CreateOrderModel>();
                var items = selectedCarts.Select(i => new OrderItemModel
                {
                    Id = i.Id,
                    Price = i.Price,
                    ProductAttributeId = i.ProductAttributeId,
                    ProductAttributeName = i.ProductAttributeId > 0 ? _attributeService.GetProductAttributeById(i.ProductAttributeId).ValueName : "",
                    ProductId = i.ProductId,
                    ProductImage = _productService.GetProductById(i.ProductId).ProductImage,
                    ProductName = _productService.GetProductById(i.ProductId).Name,
                    Quantity = i.Quantity,
                    TotalPrice = i.Quantity * i.Price
                }).ToList();
                model.Items.AddRange(items);
                selectedCarts.ForEach(c => _cartService.DeleteCart(c.Id));
                unitOfWork.Complete();

                var couponLists = _couponService.GetAllCoupons(customerId: this.CustomerId, used: false);

                if (couponLists.TotalCount > 0)
                {

                    model.AvailableCoupons = couponLists.Items.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                    }).ToList();
                }
                model.AvailableCoupons.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "请选择优惠券",
                });
                return View(model);
            }
        }

        /// <summary>
        /// 订单生成统一下单
        /// </summary>
        /// <param name="cartIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CartSaveToOrder(int orderId,int CouponId,string OrderRemarks)
        {
            var order = _orderService.GetOrderById(orderId);

            #region 优惠券
            //优惠券
            if (CouponId > 0)
            {
                var coupon = _couponService.GetCouponById(CouponId);            
                coupon.OrderId = order.Id;
                _couponService.UpdateCoupon(coupon);

                order.CouponId = coupon.Id;
                order.Preferential = coupon.Amount;
                _orderService.UpdateOrder(order);
            }
            #endregion

            #region  收货地址
            if (order.Billing == 0)
            {
                //用户地址
                var customer = _customerService.GetCustomerId(this.CustomerId);
                var Billing = _addressService.InsertAddress(new CustomerAddress
                {
                    UserName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName),
                    CountryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName),
                    CityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName),
                    DetailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo),
                    ProvinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName),
                    TelNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber),
                    CustomerId = this.CustomerId,
                });
                order.Billing = Billing;
                order.OrderRemarks = OrderRemarks;
                _orderService.UpdateOrder(order);
            }
            #endregion

            
            var orderSettigns = GetOrderSettings();
            var nonceStr = CommonHelper.GenerateNonceStr();

            var total = FormatTotal(order.OrderTotal - order.Preferential);
            //调用统一下单  
            var result = UnifiedOrder(nonceStr, "备注", order.OrderSn, total);
            var jsApiString = GetJsApiParameters(result, orderSettigns.Key);
            var handler = GetJsApiParametersRequest(result, orderSettigns.Key);

            var jsonData = new
            {
                appId = handler.GetParameter("appId"),
                timeStamp = handler.GetParameter("timeStamp"),
                nonceStr = handler.GetParameter("nonceStr"),
                package = handler.GetParameter("package"),
                signType = handler.GetParameter("signType"),
                paySign = handler.GetParameter("paySign"),
            };

            return AbpJson(jsonData);
        }
        #endregion

        #region My Order
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyOrder()
        {            
            return View();
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult OrderDatas(int status, int pageIndex = 0, int pageSize = 10)
        {
            var customer = _customerService.GetCustomerId(Convert.ToInt32(AbpSession.UserId));

            var orders = _cacheManager.GetCache(string.Format(CACHE_ORDERS.CACHE_ORDER_BY_CUSTOMER, customer.Id))
                        .Get(customer.Id.ToString(), () => _orderService.GetAllOrders(customerId: customer.Id));

            var query = orders.Items.ToList();

            if (status > 0)
            {
                query = query.Where(o => o.OrderStatusId == status).ToList();
            }
            query = query.Skip(pageIndex).Take(pageSize).ToList();

            var jsonData = new DataSourceResult
            {
                Data = query.Select(x => new
                {
                    Id = x.Id,
                    CreateTime = x.CreationTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    Total = x.OrderTotal,
                    OrderStatus = ((OrderStatus)x.OrderStatusId).GetDescription(),
                    Status = x.OrderStatusId,
                    Products = _orderService.GetOrderItems(x.Id).Select(p => new
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        Name = p.ProductName,
                        Image = p.ProductImage,
                    }).ToList(),

                }).ToList(),
            };
            return AbpJson(jsonData);
        }
        #endregion

        #region  Create Order // 购物车到订单
        [CustomerAntiForgery]
        public ActionResult CartToOrder(int[] cartIds)
        {
            var allCarts = _cartService.GetAllShoppingItems(Convert.ToInt32(AbpSession.UserId));
            var orders = allCarts.Items.Where(c => cartIds.Contains(c.Id));

            if (orders == null || orders.Count() == 0)
                throw new UserFriendlyException("该商品不存在");

            var orderSettigns = GetOrderSettings();

            var freight = orderSettigns.OrderFreeShip > orders.Sum(o => o.Quantity * o.Price) ?
                               orderSettigns.ShipFee : 0;

            var subTotal = orders.Sum(o => o.Quantity * o.Price);
            var model = new CreateOrderModel()
            {
                OrderStatusId = 10,
                PaymentStatusId = 10,
                Billing = 0,
                OrderRemarks = "",
                Freight = freight,
                Subtotal = subTotal,
                OrderTotal = freight + subTotal,
                OrderSn = CommonHelper.GenerateOrderSN(),
            };
            
            model = _cacheManager
                .GetCache(string.Format("ordersn.{0}", model.OrderSn))
                .Get(model.OrderSn, () => model);

            WeChatDefault wechat = new WeChatDefault();
            var config = wechat.WxConfig(_settingService, _cacheManager, this.Request.Url.Host + this.Request.Url.PathAndQuery);

            ViewData["config"] = config;

            #region 用户地址

            CustomerAddressModel address = new CustomerAddressModel();
            var addresses = _addressService.GetAllAddress(customerId: this.CustomerId);
            if (addresses.TotalCount > 0)
            {
                var customer = _customerService.GetCustomerId(this.CustomerId);
                address.cityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName);
                address.countryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName);
                address.detailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo);
                address.provinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName);
                address.telNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber);
                address.userName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName);

                ViewData["address"] = address;
            }
            #endregion

            return View("CreateOrder", model);
        }


        #endregion

        #region 支付

        [HttpPost]
        public ActionResult CreateOrder(ProductToOrderModel model)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var product = _productService.GetProductById(model.ProductId);
                var price = product.SpecialPrice.HasValue &&
                            product.SpecialPriceEndDateTime > DateTime.Now &&
                            product.SpecialPriceStartDateTime < DateTime.Now ?
                            product.SpecialPrice.Value :
                            product.Price;
                var productAttribute = _attributeService.GetProductAttributeById(model.ProductAttributeId);

                //用户地址
                var customer = _customerService.GetCustomerId(this.CustomerId);
                var Billing = _addressService.InsertAddress(new CustomerAddress
                {
                    UserName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName),
                    CountryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName),
                    CityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName),
                    DetailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo),
                    ProvinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName),
                    TelNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber),
                    CustomerId = this.CustomerId,
                });

                var order = new Order
                {
                    CreationTime = DateTime.Now,
                    Billing = Billing,
                    CustomerId = this.CustomerId,
                    Freight = model.Freight,
                    OrderTotal = model.Freight + (model.Price * model.Quantity),
                    Subtotal = model.Price * model.Quantity,
                    IsDeleted = false,
                    OrderRemarks = model.OrderRemarks,
                    OrderSn = CommonHelper.GenerateOrderSN(),
                    OrderStatusId = (int)OrderStatus.Pending,
                    CouponId = 0,
                    Preferential = 0
                };

                #region 优惠券
                //优惠券
                if (model.CouponId > 0)
                {
                    var coupon = _couponService.GetCouponById(model.CouponId);
                    order.Preferential = coupon.Amount;
                    order.CouponId = coupon.Id;
                    coupon.OrderId = order.Id;
                    _couponService.UpdateCoupon(coupon);
                }
                order.Id = _orderService.InsertOrder(order);

                #endregion
                _orderService.InsertOrderItems(new OrderItem
                {
                    ProductAttributeId = model.ProductAttributeId,
                    OrderId = order.Id,
                    OrderItemGuid = Guid.NewGuid(),
                    Price = model.Price,
                    ProductId = model.ProductId,
                    ProductImage = product.ProductImage,
                    ProductName = product.Name,
                    Quantity = model.Quantity,
                    TotalPrice = model.Price * model.Quantity
                });


                _cacheManager
                    .GetCache(string.Format(CACHE_ORDERS.CACHE_ORDER_BY_CUSTOMER, order.CustomerId))
                    .Remove(string.Format(CACHE_ORDERS.CACHE_ORDER_BY_CUSTOMER, order.CustomerId));

                unitOfWork.Complete();

                var orderSettigns = GetOrderSettings();
                var nonceStr = CommonHelper.GenerateNonceStr();

                var total = FormatTotal(order.OrderTotal - order.Preferential);
                //调用统一下单
                var result = UnifiedOrder(nonceStr, "备注", order.OrderSn, (int)(total));
                var handler = GetJsApiParametersRequest(result, orderSettigns.Key);

                var jsonData = new
                {
                    appId = handler.GetParameter("appId"),
                    timeStamp = handler.GetParameter("timeStamp"),
                    nonceStr = handler.GetParameter("nonceStr"),
                    package = handler.GetParameter("package"),
                    signType = handler.GetParameter("signType"),
                    paySign = handler.GetParameter("paySign"),
                };

                return AbpJson(jsonData);
            }
        }
        #endregion

        #region  Order Detail

        public ActionResult Detail(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.CustomerId != this.CustomerId)
            {
                var result = new OrderDtailModel();                
                return View(result);
            }
            var model = PrepareOrderDetailModel(order);

            return View(model);
        }
        #endregion
        
        #region Order 商品单品到订单

        [HttpPost]
        [CustomerAntiForgery]
        public ActionResult ProductToOrder(int productId, int qty, int attribute)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                throw new UserFriendlyException("该商品不存在");

            var orderSettigns = GetOrderSettings();
            var freight = orderSettigns.OrderFreeShip > qty * product.Price ? orderSettigns.ShipFee : 0;

            //价格体系
            var price = decimal.Zero;
            ProductAttribute productAttribute = null;
            if (attribute == 0)
            {
                price = product.SpecialPrice.HasValue &&
                            product.SpecialPriceEndDateTime > DateTime.Now &&
                            product.SpecialPriceStartDateTime < DateTime.Now ?
                            product.SpecialPrice.Value :
                            product.Price;
            }
            else
            {
                productAttribute = _attributeService.GetProductAttributeById(attribute);
                price = productAttribute.Price;
            }            

            var model = new ProductToOrderModel
            {
                ProductId = product.Id,
                Freight = freight,
                Quantity = qty,
                ProductName = product.Name,
                ProductImage = product.ProductImage,
                Billing = 0,
                ProductAttributeId = attribute,
                ProductAttributeName = attribute == 0 ? "" : productAttribute.ValueName,
                OrderRemarks = "",
                Price = price,
                OrderTotal = freight + (qty * price),
                Subtotal = qty * price
            };

            #region 优惠券

            if (!product.SpecialPrice.HasValue)
            {
                var couponLists = _couponService.GetAllCoupons(customerId: this.CustomerId, used: false);

                if (couponLists.TotalCount > 0)
                {

                    model.AvailableCoupons = couponLists.Items.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                    }).ToList();
                }

                model.AvailableCoupons.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "请选择优惠券",
                });
            }
            else
            {
                model.AvailableCoupons.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "没有可用的优惠券",
                });
            }

            #endregion

            #region 用户地址

            CustomerAddressModel address = new CustomerAddressModel();
            var addresses = _addressService.GetAllAddress(customerId: this.CustomerId);
            if (addresses.TotalCount > 0)
            {
                var customer = _customerService.GetCustomerId(this.CustomerId);
                address.cityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName);
                address.countryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName);
                address.detailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo);
                address.provinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName);
                address.telNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber);
                address.userName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName);

                ViewData["address"] = address;
            }
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult JsApiGet(string orderId)
        {
            WeChatDefault wechat = new WeChatDefault();
            var config = wechat.WxConfig(_settingService, _cacheManager, this.Request.Url.Host + this.Request.Url.PathAndQuery);

            return AbpJson(config);
        }
        #endregion

        #region Message WeChat

        //发送通知短信（管理员）
        [NonAction]
        private void SendOrderMessage(string msg)
        {
            string msgUrl = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
            WeChatDefault wxDefault = new WeChatDefault();
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            var token = wxDefault.GetAccessToken(_cacheManager, appId, appSecret);
            string url = string.Format(msgUrl, token.access_token);
            HttpWebResponseUtility client = new HttpWebResponseUtility();
            client.CreatePostHttpResponse(url: url, data: msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="amount"></param>
        /// <param name="customer"></param>
        private void SendMessageToUser(Order order, decimal amount, Customer customer)
        {
            var model = new SendMessageModel();
            model.touser = customer.OpenId;
            model.msgtype = "news";

            StringBuilder desc = new StringBuilder();
            desc.Append("您推广的用户产生了一笔订单");
            desc.Append("\n");
            desc.Append("订单金额：" + order.Subtotal);
            desc.Append("\n");
            desc.Append("您的佣金：" + amount);

            model.news.articles.Add(new SendMessageModel.ArticlesItem
            {
                url = string.Format("{0}/{1}", HttpContext.Request.Url.Host, "/Customer/MyCustomer"),
                title = "您有了新的推广订单",
                picurl = "",
                description = desc.ToString(),
            });

            SendOrderMessage(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }
        #endregion

        #region  Return Order 退款
        public ActionResult ReturnOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order.CustomerId != this.CustomerId)
                return RedirectToAction("MyOrder");

            var model = new ReturnOrderModel {
                OrderId = order.Id,
                CustomerId = this.CustomerId,
                AuditStatus = (int)AuditStatus.None,                
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ReturnOrder(ReturnOrderModel model)
        {
            var order = _orderService.GetOrderById(model.Id);
            if (order.CustomerId != this.CustomerId)
                return RedirectToAction("MyOrder");

            ReturnRun("", order.OrderSn, Convert.ToInt32(order.OrderTotal * 100), Convert.ToInt32(order.OrderTotal * 100));
            var orderModel = order.MapTo<OrderModel>();
            return View(orderModel);

        }

        /***
        * 申请退款完整业务流程逻辑
        * @param transaction_id 微信订单号（优先使用）
        * @param out_trade_no 商户订单号
        * @param total_fee 订单总金额
        * @param refund_fee 退款金额
        * @return 退款结果（xml格式）
        */
        [NonAction]
        private bool ReturnRun(string transaction_id, string out_trade_no, int total_fee, int refund_fee)
        {
            RequestHandler data = new RequestHandler(null);

            if (!string.IsNullOrEmpty(transaction_id))//微信订单号存在的条件下，则已微信订单号为准
            {
                data.SetParameter("transaction_id", transaction_id);
            }
            else//微信订单号不存在，才根据商户订单号去退款
            {
                data.SetParameter("out_trade_no", out_trade_no);
            }

            var orderSettings = GetOrderSettings();

            data.SetParameter("total_fee", total_fee.ToString());//订单总金额
            data.SetParameter("refund_fee",refund_fee.ToString());//退款金额
            data.SetParameter("out_refund_no", CommonHelper.GenerateReturnOrderSN());//随机生成商户退款单号
            data.SetParameter("op_user_id", orderSettings.MchId);//操作员，默认为商户号

            data.SetParameter("appid", orderSettings.AppId);//公众账号ID
            data.SetParameter("mch_id", orderSettings.MchId);//商户号
            data.SetParameter("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            data.CreateMd5Sign(orderSettings.Key);//签名并且添加
            var xml = data.ParseXML();

            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            string response = HttpService.Post(xml, url, true, 6);
            Logger.Debug("退款业务：" + response);
            return true;
        }
        #endregion

        #region  继续支付订单
        public ActionResult GoOnPay(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var items =_orderService.GetOrderItems(order.Id); 
            var orderSettigns = GetOrderSettings();
            //小计
            var subTotal = items.Sum(i => i.TotalPrice);
            var freight = orderSettigns.OrderFreeShip > subTotal ? orderSettigns.ShipFee : 0;

            order.OrderTotal = subTotal + freight;
            _orderService.UpdateOrder(order);

            var model = order.MapTo<CreateOrderModel>();
            model.Items = _orderService.GetOrderItems(order.Id).Select(i => new OrderItemModel
            {
                Id = i.Id,
                Price = i.Price,
                ProductAttributeId = i.ProductAttributeId,
                ProductAttributeName = i.ProductAttributeId > 0 ? _attributeService.GetProductAttributeById(i.ProductAttributeId).ValueName : "",
                ProductId = i.ProductId,
                ProductImage = i.ProductImage,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            }).ToList();


            #region 优惠券

            var itemsCount = model.Items.Count;
            var item = items.FirstOrDefault();
            var product = _productService.GetProductById(item.ProductId);
            if (itemsCount == 1&& product.SpecialPrice.HasValue)
            {
                model.AvailableCoupons.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "没有可用的优惠券",
                });
            }
            else
            {
                var couponLists = _couponService.GetAllCoupons(customerId: this.CustomerId, used: false);

                if (couponLists.TotalCount > 0)
                {

                    model.AvailableCoupons = couponLists.Items.Select(c => new SelectListItem
                    {
                        Selected = c.Id == model.CouponId,
                        Value = c.Id.ToString(),
                        Text = c.Name,
                    }).ToList();
                }

                model.AvailableCoupons.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "请选择优惠券",
                });
            }
            #endregion
            order.OrderSn = CommonHelper.GenerateOrderSN();
            _orderService.UpdateOrder(order);

            #region 用户地址

            CustomerAddressModel address = new CustomerAddressModel();
            var addresses = _addressService.GetAllAddress(customerId: this.CustomerId);
            if (addresses.TotalCount > 0)
            {
                var customer = _customerService.GetCustomerId(this.CustomerId);
                address.cityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName);
                address.countryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName);
                address.detailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo);
                address.provinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName);
                address.telNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber);
                address.userName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName);

                ViewData["address"] = address;
            }
            #endregion

            return View("GenerateOrder", model);
        }
        #endregion

        #region 订单操作 取消/退单等
        public ActionResult CancelOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.CustomerId != this.CustomerId || order.OrderStatusId != (int)OrderStatus.Pending)
                return RedirectToAction("MyOrders");

            order.OrderStatusId = (int)OrderStatus.Cancel;
            return RedirectToAction("Detail", new { orderId = orderId });

        }
        #endregion

        #region CouponTotal
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult GetCouponTotalById(int couponId,decimal total)
        {
            var coupon = _couponService.GetCouponById(couponId);

            var data = new
            {
                Amount = coupon.Amount,
                Total = total - coupon.Amount,
            };

            return AbpJson(data);
        }
        #endregion

        #region 减少商品库存
        private void InventoryProduct(int productId, int productAttributeId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
               var product = _productService.GetProductById(productId);

                product.StockQuantity -= 1;
                _productService.UpdateProduct(product);
                if (productAttributeId > 0)
                {

                }
            }
        }
        #endregion

        #region 格式化 Total

        private int FormatTotal(decimal orderTotal)
        {
            if (orderTotal <= 0)
                return 100;

            var returnValue = orderTotal * 100;
            return Convert.ToInt32(returnValue);
        }
        #endregion

        #region ComboProduct-Order
        public ActionResult ComBoProductOrder(int comboProductId)
        {
            var comboProduct = _comboProductService.GetComBoProductById(comboProductId);
            var model = comboProduct.MapTo<ComBoProductOrderModel>();

            var productIds = _comboProductService.GetComBoProductMappings(comboProduct.Id);
            var produsts = _productService.GetProductByIds(productIds);
            model.Items = produsts.Select(p => new ComBoProductOrderModel.ProductModel
            {
                Id = p.Id,
                Price = p.Price,
                ProductImage = p.ProductImage,
                ProductName = p.Name,
                Quantity = 1
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ComBoProductOrderSave(int comboProductId)
        {
            var comboProduct = _comboProductService.GetComBoProductById(comboProductId);

            #region  收货地址
                //用户地址
                var customer = _customerService.GetCustomerId(this.CustomerId);
                var billing = _addressService.InsertAddress(new CustomerAddress
                {
                    UserName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.UserName),
                    CountryName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CountryName),
                    CityName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.CityName),
                    DetailInfo = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.DetailInfo),
                    ProvinceName = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.ProvinceName),
                    TelNumber = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.TelNumber),
                    CustomerId = this.CustomerId,
                });
            #endregion
            
            var order = new Order
            {
                CouponId = 0,
                CreationTime = DateTime.Now,
                CustomerId = this.CustomerId,
                Freight = 0,
                IsDeleted = false,
                IsRewardOrder = false,
                Billing = billing,
                OrderRemarks = "",
                OrderSn = CommonHelper.GenerateOrderSN(),
                OrderStatusId = (int)OrderStatus.Pending,
                OrderTotal = comboProduct.Price,
                Preferential = 0,
                Subtotal = comboProduct.Price
            };

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                order.Id = _orderService.InsertOrder(order);


                var productIds = _comboProductService.GetComBoProductMappings(comboProduct.Id);
                var produsts = _productService.GetProductByIds(productIds);
                produsts.ForEach(p => _orderService.InsertOrderItems(new OrderItem
                {
                    OrderId = order.Id,
                    OrderItemGuid = Guid.NewGuid(),
                    PreSell = p.PreSell,
                    Price = p.Price,
                    ProductAttributeId = 0,
                    ProductId = p.Id,
                    ProductImage = p.ProductImage,
                    ProductName = p.Name,
                    Quantity = 1,
                    Review = false,
                    TotalPrice = p.Price
                }));

                unitOfWork.Complete();

                var orderSettigns = GetOrderSettings();
                var nonceStr = CommonHelper.GenerateNonceStr();

                var total = FormatTotal(order.OrderTotal - order.Preferential);
                //调用统一下单  
                var result = UnifiedOrder(nonceStr, "备注", order.OrderSn, total);
                var jsApiString = GetJsApiParameters(result, orderSettigns.Key);
                var handler = GetJsApiParametersRequest(result, orderSettigns.Key);

                var jsonData = new
                {
                    appId = handler.GetParameter("appId"),
                    timeStamp = handler.GetParameter("timeStamp"),
                    nonceStr = handler.GetParameter("nonceStr"),
                    package = handler.GetParameter("package"),
                    signType = handler.GetParameter("signType"),
                    paySign = handler.GetParameter("paySign"),
                };

                return AbpJson(jsonData);
            }
            
        }
        #endregion
    }
}