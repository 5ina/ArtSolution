using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using System.Linq;
using System.Web.Mvc;
using ArtSolution.Web.Extensions;
using ArtSolution.Web.Framework.DataGrids;
using System;
using ArtSolution.Security;
using Abp.Runtime.Caching;
using ArtSolution.Names;
using Abp.AutoMapper;
using Abp.UI;
using ArtSolution.Web.Areas.Admin.Models.Customers;
using ArtSolution.Common;
using ArtSolution.Orders;
using System.Collections.Generic;

namespace ArtSolution.Web.Areas.Admin.Controllers
{

    public class CustomerController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptService;
        private readonly IOrderService _orderService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;


        public CustomerController(ICustomerService customerService,
                                    IEncryptionService encryptService,
                                    IOrderService orderService,
                                    ISettingService settingService,
                                    ICacheManager cacheManager)
        {
            this._customerService = customerService;
            this._encryptService = encryptService;
            this._orderService = orderService;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
        }
        #endregion

        #region Utilities
        [NonAction]
        protected void PrepareCustomerListModel(CustomerListModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");


        }

        [NonAction]
        protected void PrepareCustomerModel(CustomerModel model)
        {
            if (model == null)
                throw new UserFriendlyException("model");

            model.AvailableCustomerRoles = CustomerRole.Buyer.EnumToDictionary(e => e.GetDescription()).ToList();
        }
        #endregion

        #region Method

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, CustomerListModel model)
        {
            CustomerRole? role = null;
            if (model.RoleId.HasValue)
                role = (CustomerRole)model.RoleId;
            var customer = _customerService.GetAllCustomers(keywords: model.Keywords,
                roleId: role,
                isSub: model.Sub,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                Data = customer.Items.Select(c => new
                {
                    Id = c.Id,
                    Name = c.NickName,
                    Mobile = c.Mobile,
                    CreationTime = c.CreationTime,
                    IsSubscribe = c.IsSubscribe,
                    CustomerRole = ((CustomerRole)c.CustomerRoleId).GetDescription(),
                    WidthOrder = _orderService.GetAllOrders(customerId: c.Id).TotalCount,
                    Promoter = GetPromoter(c.Promoter)
                }).ToList(),
                Total = customer.TotalCount
            };
            return AbpJson(jsonData);
        }

        private string GetPromoter(int promoterId)
        {
            if (promoterId == 0)
                return "无";
            var customer = _customerService.GetCustomerId(promoterId);
            return customer.NickName;
        }

        [HttpPost]
        public ActionResult ModifyPassword(ChangePasswordModel model)
        {
            var currentCustomerId = Convert.ToInt32(AbpSession.UserId.Value);
            var customer = _customerService.GetCustomerId(currentCustomerId);
            var password = _encryptService.CreatePasswordHash(model.OldPassword, customer.PasswordSalt);
            if (password == customer.Password && (model.ConfirmNewPassword == model.NewPassword))
            {
                customer.Password = _encryptService.CreatePasswordHash(model.NewPassword, customer.PasswordSalt);
                _customerService.UpdateCustomer(customer);
                return AbpJson("修改成功");
            }
            return AbpJson("密码错误");
        }


        public ActionResult Create()
        {
            var model = new CustomerModel();
            PrepareCustomerModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CustomerModel model)
        {
            var hasCustomer = _customerService.GetCustomerByMobile(model.Mobile);
            if (hasCustomer != null)
                ModelState.AddModelError("", "该手机号已经被注册");
            if (ModelState.IsValid)
            {
                var customer = model.MapTo<Customer>();
                var code = CommonHelper.GenerateCode(6);
                customer.PasswordSalt = code;
                customer.Password = _encryptService.CreatePasswordHash("123456", code);
                _customerService.CreateCustomer(customer);
                return RedirectToAction("List");
            }
            PrepareCustomerModel(model);
            return View(model);
        }


        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerId(id);
            var model = customer.MapTo<CustomerModel>();
            PrepareCustomerModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerService.GetCustomerId(model.Id);
                customer = model.MapTo<CustomerModel, Customer>(customer);
                _customerService.UpdateCustomer(customer);
                return RedirectToAction("List");
            }
            PrepareCustomerModel(model);
            return View(model);
        }

        public ActionResult CustomerPromoter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CustomerPromoter(DataSourceRequest command)
        {
            var customer = _customerService.GetAllCustomers(isPromoter: true,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                Data = customer.Items.Select(c => new
                {
                    Id = c.Id,
                    Name = c.NickName,
                    Mobile = c.Mobile,
                    CreationTime = c.CreationTime,
                    IsSubscribe = c.IsSubscribe,
                    PromoterCount = GetProtomerCount(c.Id),
                    Total = GetAmountByProtomer(c.Id),
                }).ToList(),
                Total = customer.TotalCount
            };
            return AbpJson(jsonData);
        }

        public ActionResult PromoterList()
        {
            return View();

        }

        #region 推广人相关
        /// <summary>
        /// 推广人数量
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private int GetProtomerCount(int customerId)
        {
            return _customerService.GetAllCustomers(promoter: customerId).TotalCount;
        }

        /// <summary>
        /// 获取总金额
        /// </summary>
        /// <param name="promoterId"></param>
        /// <returns></returns>
        private decimal GetAmountByProtomer(int promoterId)
        {
            var customerIds = _customerService.GetAllCustomers(promoter: promoterId).Items.Select(o => o.Id).ToArray();
            var total = _orderService.GetAmountByCustomerIds(customerIds);
            return total;
        }
        #endregion
        #endregion

        #region Method wechat Customer

        #endregion

        #region Backlog Job
        [ChildActionOnly]
        public ActionResult NewCustomer()
        {
            return PartialView();
        }

        #endregion

        #region Statistical Report

        public ActionResult CustomerStatisticalOverview()
        {
            var model = _cacheManager.GetCache(CustomerCacheNames.CACHE_CUSTOMER_StatisticalOverview)
                .Get("", () => {

                    var customer = _customerService.GetAllCustomers(showHidden: true);
                    var view = new CustomerStatisticalOverviewModel();
                    view.CustomerTotalCount = customer.TotalCount;
                    view.CustomerWeekNewCount = customer.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-7)).Count();
                    view.CustomerWeekNewCount = customer.Items.Where(c => c.CreationTime < DateTime.Now.AddDays(-30)).Count();
                    return view;
                });
            return PartialView(model);
        }


        [HttpPost]
        public ActionResult LoadCustomerStatistics(string period)
        {
            var result = new List<object>();

            var nowDt = DateTime.Now;

            switch (period)
            {
                case "year":
                    //year statistics
                    var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
                    var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);
                    for (int i = 0; i <= 12; i++)
                    {
                        result.Add(new
                        {
                            date = searchYearDateUser.Date.ToString("Y"),
                            value = _customerService.GetAllCustomers(
                                createdFrom: searchYearDateUser,
                                createdTo: searchYearDateUser.AddMonths(1),
                                roleId: CustomerRole.Buyer,
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
                            value = _customerService.GetAllCustomers(
                                createdFrom: (searchMonthDateUser),
                                createdTo: searchMonthDateUser.AddDays(1),
                                roleId: CustomerRole.Buyer,
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
                            value = _customerService.GetAllCustomers(
                                createdFrom: searchWeekDateUser,
                                createdTo: searchWeekDateUser.AddDays(1),
                                roleId: CustomerRole.Buyer,
                                pageIndex: 0,
                                pageSize: 1).TotalCount.ToString()
                        });

                        searchWeekDateUser = searchWeekDateUser.AddDays(1);
                    }
                    break;
            }

            return Json(result);
        }
        #endregion

        #region Box Customer

        public ActionResult BoxCustomerList()
        {
            var model = new CustomerListModel();
            PrepareCustomerListModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetCustomerNameById(int customerId)
        {
            var customerName = "NaNull";
            var customer = _customerService.GetCustomerId(customerId);
            if (customer != null)
                customerName = customer.NickName;

            return AbpJson(customerName);

        }
        #endregion

    }
}