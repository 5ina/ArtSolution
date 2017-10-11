using Abp.AutoMapper;
using Abp.Runtime.Caching;
using ArtSolution.Catalog;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Domain.Orders;
using ArtSolution.Names;
using ArtSolution.Orders;
using ArtSolution.Web.Areas.Admin.Models.Orders;
using ArtSolution.Web.Extensions;
using ArtSolution.Web.Framework;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Mvc;
using ArtSolution.Web.Framework.WeChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    public class OrderController : ArtSolutionControllerAdminBase
    {
        #region WxChat Url
        /// <summary>
        /// 微信发送消息
        /// {0} :Access_Token
        /// </summary>
        string msgUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        #endregion

        #region ctor && Fields
        private readonly IOrderService _orderService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerService _customerService;
        private readonly ICustomerAddressService _addressService;
        private readonly ISettingService _settingService;
        private readonly IProductAttributeService _attributeService;

        public OrderController(IOrderService orderService,
            ICacheManager cacheManager,
            ICustomerService customerService,
            ICustomerAddressService addressService,
            
            IProductAttributeService attributeService,
            ISettingService settingService)
        {
            this._orderService = orderService;
            this._cacheManager = cacheManager;
            this._customerService = customerService;
            this._addressService = addressService;
            this._settingService = settingService;
            this._attributeService = attributeService;
        }
        #endregion

        #region Utilities
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
                Specifications = p.ProductAttributeId > 0 ? _attributeService.GetProductAttributeById(p.ProductAttributeId).ValueName : ""
            }).ToList();
            return model;
        }

        [NonAction]
        private void SendOrderMessage(string msg)
        {
            WeChatDefault wxDefault =new WeChatDefault();
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            var token = wxDefault.GetAccessToken(_cacheManager, appId, appSecret);
            string url = string.Format(msgUrl, token.access_token);
            HttpWebResponseUtility client = new HttpWebResponseUtility();
            client.CreatePostHttpResponse(url: url, data: msg);
        }
        #endregion

        #region List
        public ActionResult List([ModelBinder(typeof(CommaSeparatedModelBinder))] List<string> orderStatusIds = null)
        {
            //order statuses
            var model = new OrderListModel();
            model.AvailableOrderStatuses = OrderStatus.Pending.EnumToDictionary(o => o.GetDescription(), false).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, OrderListModel model)
        {
            
            var orderStatusIds = !model.OrderStatusIds.Contains(0) ? model.OrderStatusIds : null;
            var list = _orderService.GetAllOrders(createdFrom: model.StartDate,
                                    createdTo: model.EndDate,
                                    keywords: model.Keywords,
                                    showHidden: true,
                                    orderStatusIds: orderStatusIds,
                                    //orderStatus: model.OrderStatusId,
                                    pageIndex: command.Page - 1,
                                    pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                Data = list.Items.Select(o => new
                {
                    Id = o.Id,
                    OrderSn = o.OrderSn,
                    CreateTime = o.CreationTime.ToString("MM月dd日 hh:mm"),
                    OrderStatus = ((OrderStatus)o.OrderStatusId).GetDescription(),
                    Status = o.OrderStatusId,
                    Total = o.OrderTotal,
                    CreationTime = o.CreationTime
                }),
            };
            return AbpJson(jsonData);
        }
        #endregion

        #region Statistical Report
        public ActionResult OrderStatisticalOverview()
        {
            var model = _cacheManager.GetCache(OrderCacheNames.CACHE_ORDER_StatisticalOverview)
                .Get("", () => {

                    var orders = _orderService.GetAllOrders(showHidden: true);
                    var view = new OrderStatisticalOverviewModel();
                    view.OrderTotalPrice = orders.Items.Sum(x => x.OrderTotal);
                    view.OrderTotalCount = orders.Items.Count();
                    view.OrderWeekTotalCount = orders.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-7)).Count();
                    view.OrderTotalPrice = orders.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-7)).Sum(o => o.OrderTotal);
                    view.OrderMonthTotalCount = orders.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-30)).Count();
                    view.OrderWeekTotalPrice = orders.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-30)).Sum(o => o.OrderTotal);
                    return view;
                });
            return PartialView(model);
        }
        #endregion

        #region Backlog Job

        [ChildActionOnly]
        public ActionResult PendingOrder()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult PendingOrderList()
        {
            var orders = _orderService.GetAllOrders(pageIndex: 0, pageSize: 5);
            var jsonData = new DataSourceResult
            {
                ExtraData = orders.Items.Select(o => new
                {
                    Id = o.Id,
                    OrderSn = o.OrderSn,
                    CreateTime = o.CreationTime.ToString("MM月dd日 hh:mm"),                    
                    OrderStatus = ((OrderStatus)o.OrderStatusId).GetDescription(),
                    Total = o.OrderTotal
                }),
            };
            return AbpJson(jsonData);
        }
        #endregion

        #region Detail

        public ActionResult Detail(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return RedirectToAction("List");

            var model = PrepareOrderDetailModel(order);

            return View(model);
        }
        #endregion             

        #region PrintOrder
        [HttpPost]
        public ActionResult PrintOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            var address = _addressService.GetAddressById(order.Billing);
            var billingAddress = string.Empty;

            if (address != null)
            {
                billingAddress = string.Format("{0}{1}{2}{3}{4}{5}{6}", address.ProvinceName, address.CityName, address.CountryName, address.DetailInfo, address.UserName, address.TelNumber);
            }
            return Content("ok");
        }
        #endregion

        #region Line Chart
        [ChildActionOnly]
        public ActionResult LineChart()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult LineChartData()
        {
            var orders = _orderService.GetAllOrders(createdFrom: DateTime.Now.AddDays(-7), createdTo: DateTime.Now);

            var data = new
            {
                Count = new[] { 7, 25, 33, 29, 74, 11, 22 },
                Total = new[] { 199, 821, 1024, 992, 3354, 229, 775 },
            };

            return AbpJson(data);

        }

        [HttpPost]
        public ActionResult LoadOrderStatistics(string period)
        {
            var result = new List<object>();

            var nowDt = DateTime.Now;
            switch (period)
            {
                case "year":
                    //年报
                    var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
                    var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);

                    for (int i = 0; i <= 12; i++)
                    {
                        result.Add(new
                        {
                            date = searchYearDateUser.Date.ToString("Y"),
                            value = _orderService.GetAllOrders(
                                createdFrom: searchYearDateUser,
                                createdTo: searchYearDateUser.AddMonths(1),
                                pageIndex: 0,
                                pageSize: 1).TotalCount.ToString()
                        });

                        searchYearDateUser = searchYearDateUser.AddMonths(1);
                    }
                    break;

                case "month":
                    //month statistics
                    var monthAgoDt = nowDt.AddDays(-30);
                    var searchMonthDateUser = new DateTime(monthAgoDt.Year, monthAgoDt.Month, monthAgoDt.Day);

                    for (int i = 0; i <= 30; i++)
                    {
                        result.Add(new
                        {
                            date = searchMonthDateUser.Date.ToString("M"),
                            value = _orderService.GetAllOrders(
                                createdFrom: searchMonthDateUser,
                                createdTo: searchMonthDateUser.AddDays(1),
                                pageIndex: 0,
                                pageSize: 1).TotalCount.ToString()
                        });

                        searchMonthDateUser = searchMonthDateUser.AddDays(1);
                    }
                    break;

                case "week":
                default:
                    //week statistics
                    var weekAgoDt = nowDt.AddDays(-7);
                    var searchWeekDateUser = new DateTime(weekAgoDt.Year, weekAgoDt.Month, weekAgoDt.Day);

                    for (int i = 0; i <= 7; i++)
                    {
                        result.Add(new
                        {
                            date = searchWeekDateUser.Date.ToString("d dddd"),
                            value = _orderService.GetAllOrders(
                                createdFrom: searchWeekDateUser,
                                createdTo: searchWeekDateUser.AddDays(1),
                                pageIndex: 0,
                                pageSize: 1).TotalCount.ToString()
                        });

                        searchWeekDateUser = searchWeekDateUser.AddDays(1);
                    }
                    break;
            }

            return AbpJson(data: result);
        }

        #endregion
    }
}