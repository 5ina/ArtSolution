using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Web.Security.AntiForgery;
using ArtSolution.Catalog;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Discount;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.News;
using ArtSolution.Domain.Orders;
using ArtSolution.Domain.Settings;
using ArtSolution.Messages;
using ArtSolution.Models.WeChats;
using ArtSolution.Names;
using ArtSolution.News;
using ArtSolution.Orders;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Security;
using ArtSolution.Web.Framework.WeChat;
using ArtSolution.Web.Models.Customers;
using ArtSolution.Web.Models.News;
using ArtSolution.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class CustomerController : ArtSolutionControllerBase
    {
        #region ctor && Fields

        private const string PRODUCTREVIEW_BY_CUSTOMERID = "strore.product.review.by.customerid-{0}";
        private const string FAVORITE_BY_CUSTOMERID = "strore.favorite.by.customerid-{0}";

        /// <summary>
        /// 二维码请求URL
        /// </summary>
        private const string QRCode_Url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        private readonly IOrderService _orderService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly ICustomerService _customerService;
        private readonly IShopppingCartService _cartService;
        private readonly ICustomerRewardService _rewardService;
        private readonly IFavoriteService _favoriteService;
        private readonly IProductReviewService _reviewService;
        private readonly IApplyPromoterService _applyService;
        private readonly IApplyCashService _applyCashService;
        private readonly ISettingService _settingService;
        private readonly ICouponService _couponService;
        private readonly ISMSMessageService _messageService;
        private readonly ILoanService _loanService;
        private readonly ICommissionService _commissionServicer;
        private readonly IWishOrderService _wishService;
        private readonly ISignLogService _signService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public CustomerController(IOrderService orderService,
                                ICacheManager cacheManager,
                                ICustomerAttributeService customerAttributeService,
                                ICustomerService customerService,
                                IShopppingCartService cartService,
                                IFavoriteService favoriteService,
                                ICustomerRewardService rewardService,
                                IProductReviewService reviewService,
                                IApplyPromoterService applyService,
                                ISettingService settingService,
                                ICouponService couponService,
                                ISMSMessageService messageService,
                                ILoanService loanService,
                                IApplyCashService applyCashService,
                                ICommissionService commissionServicer,
                                IWishOrderService wishService,
                                ISignLogService signService,
                                IUnitOfWorkManager unitOfWorkManager)
        {
            this._orderService = orderService;
            this._cacheManager = cacheManager;
            this._customerAttributeService = customerAttributeService;
            this._cartService = cartService;
            this._customerService = customerService;
            this._messageService = messageService;
            this._favoriteService = favoriteService;
            this._reviewService = reviewService;
            this._rewardService = rewardService;
            this._settingService = settingService;
            this._couponService = couponService;
            this._applyService = applyService;
            this._loanService = loanService;
            this._applyCashService = applyCashService;
            this._commissionServicer = commissionServicer;
            this._wishService = wishService;
            this._signService = signService;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        #endregion

        #region Uni

        public AccessToken GetAccessToken(string appId, string appSecret)
        {

            AccessToken token = HttpUtility.Get<AccessToken>(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));

            return token;
        }
        #endregion

        #region Method

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            var key = string.Format(CustomerCacheNames.CACHE_CUSTOMER_TO_WECHAT, this.CustomerId);
            var customer = _customerService.GetCustomerId(this.CustomerId);
            WeChatDefault wxDefault = new WeChatDefault();
            var userInfo = _cacheManager.GetCache(key).Get(this.CustomerId.ToString(), () => wxDefault.GetWeChatUserInfo(_settingService, _cacheManager, customer.OpenId));

            var model = customer.MapTo<CustomerModel>();
            model.NickName = customer.NickName;
            model.CustomerAvatar = userInfo.headimgurl;
            return View(model);
        }

        public ActionResult Mobile()
        {
            var model = new CustomerMobleModel();
            model.CustomerId = this.CustomerId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Mobile(CustomerMobleModel model)
        {
            var customer = _customerService.GetCustomerId(model.CustomerId);
            if (model.Captcha != customer.VerificationCode)
            {
                model.Result = false;
                model.Error = "验证码错误";
                return View(model);
            }
            customer.Mobile = model.Mobile;
            _customerService.UpdateCustomer(customer);
            return RedirectToAction("Info");
        }

        [HttpGet]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult Captcha(string mobile)
        {
            var captcha = CommonHelper.GenerateNumber(4);
            var customer = _customerService.GetCustomerId(this.CustomerId);
            customer.VerificationCode = captcha;
            _customerService.UpdateCustomer(customer);
            _messageService.SendMobileCode(captcha, mobile);
            return Content("验证码已发送");
        }

        #endregion

        #region Customer Center
        /// <summary>
        /// 用户中心
        /// </summary>
        /// <returns></returns>
        [CustomerAntiForgery]
        public ActionResult Center()
        {
            var model = new CustomerCenterModel();
            WeChatDefault wxDefault = new WeChatDefault();
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var key = string.Format(CustomerCacheNames.CACHE_CUSTOMER_TO_WECHAT, this.CustomerId);
            var userInfo = _cacheManager.GetCache(key).Get(this.CustomerId.ToString(), () => wxDefault.GetWeChatUserInfo(_settingService, _cacheManager, customer.OpenId));
            model.CustomerId = this.CustomerId;
            model.CustomerAvatar = userInfo.headimgurl;
            model.CustomerName = userInfo.nickname;
            model.Promoter = customer.GetCustomerAttributeValue<bool>(CustomerAttributeNames.IsPromoter);
            model.BindMobile = String.IsNullOrWhiteSpace(customer.Mobile);

            return View(model);
        }
        /// <summary>
        /// 收藏夹
        /// </summary>
        /// <returns></returns>
        public ActionResult Favorites()
        {
            var key = string.Format(FAVORITE_BY_CUSTOMERID, this.CustomerId);

            var favorites = _cacheManager.GetCache(key).Get(key, () =>
            {
                return _favoriteService.GetAllFavorites(customerId: this.CustomerId);
            });
            var model = new FavoriteListModel();
            model.PageIndex = 0;
            model.PageSize = 20;
            model.Total = favorites.TotalCount;
            var favoriteList = favorites.Items.Skip(0).Take(20).ToList();
            model.Favorites = favoriteList.MapTo<List<FavoriteModel>>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Favorites(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var key = string.Format(FAVORITE_BY_CUSTOMERID, this.CustomerId);

            var favorites = _cacheManager.GetCache(key).Get(key, () =>
            {
                return _favoriteService.GetAllFavorites(customerId: this.CustomerId);
            });
            var model = new FavoriteListModel();
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            model.Total = favorites.TotalCount;
            var favoriteList = favorites.Items.Skip(pageIndex).Take(pageSize).ToList();
            model.Favorites = favoriteList.MapTo<List<FavoriteModel>>();

            return Json(model);
        }

        /// <summary>
        /// 我的评论
        /// </summary>
        /// <returns></returns>
        public ActionResult MyReview()
        {
            var key = string.Format(PRODUCTREVIEW_BY_CUSTOMERID, this.CustomerId);
            var reviews = _cacheManager.GetCache(key).Get(key, () =>
            {
                return _reviewService.GetAllProductReviews(customerId: this.CustomerId);
            });

            var model = new ProductReviewListModel();
            model.PageIndex = 0;
            model.PageSize = 10;
            var reviewList = reviews.Items.Skip(0).Take(10).ToList();
            model.Reviews = reviewList.MapTo<List<ProductReviewModel>>();
            return View(model);
        }

        [HttpPost]
        public ActionResult MyReviews(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var key = string.Format(PRODUCTREVIEW_BY_CUSTOMERID, this.CustomerId);
            var reviews = _cacheManager.GetCache(key).Get(key, () =>
            {
                return _reviewService.GetAllProductReviews(customerId: this.CustomerId);
            });

            var model = new ProductReviewListModel();
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            var reviewList = reviews.Items.Skip(pageIndex).Take(pageSize).ToList();
            model.Reviews = reviewList.MapTo<List<ProductReviewModel>>();
            return Json(model);
        }

        /// <summary>
        /// 积分
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Reward()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var reward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
            var model = new CustomerRewardModel();
            model.IsSign = _signService.IsSign(this.CustomerId);
            model.Reward = reward;
            model.CustomerId = customer.Id;
            return PartialView(model);
        }

        public ActionResult RewardHistory()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var reward = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
            var model = new CustomerRewardModel();
            model.Reward = reward;
            model.CustomerId = customer.Id;
            var histories = _rewardService.GetAllCustomerRewards(customer: this.CustomerId, pageSize: 10);
            model.Histories = histories.Items.Select(h => h.MapTo<RewardHistoryModel>()).ToList();
            _signService.SignIn(this.CustomerId);
            return PartialView(model);
        }

        #endregion

        #region Coupon

        [ChildActionOnly]
        public ActionResult Coupon()
        {
            var coupons = _couponService.GetAllCoupons(customerId: this.CustomerId, used: false);
            ViewData["CouponListCount"] = coupons.TotalCount;
            return PartialView();
        }

        public ActionResult CouponList()
        {
            var coupons = _couponService.GetAllCoupons(customerId: this.CustomerId);
            var model = coupons.Items.MapTo<List<CustomerCouponModel>>();
            return View(model);
        }

        public ActionResult Exchange()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Exchange(CustomerExchangeModel model)
        {
            if (Session["ValidateCode"].ToString() != model.VerifyCode)
            {
                ModelState.AddModelError("", "验证码错误");
            }
            if (ModelState.IsValid)
            {
                var code = string.Concat("COPB-", model.Code);

                var coupon = _couponService.GetCouponByCode(code);
                if (coupon == null)
                {
                    ModelState.AddModelError("", "优惠券代码错误");
                    return View();
                }
                coupon.CustomerId = this.CustomerId;

                _couponService.UpdateCoupon(coupon);
                return CouponList();
            }
            return View();
        }
        #endregion

        #region QR_Code 推广人
        [CustomerAntiForgery]
        public ActionResult MyQRCode()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var expireTime = customer.GetCustomerAttributeValue<DateTime>(CustomerAttributeNames.QRCodeExpireTime);

            var expire = _settingService.GetSettingByKey<int>(WeChatSettingNames.Expire);
            var model = new CustomerQRModel();
            model.CustomerID = this.CustomerId;

            if (expireTime > DateTime.Now)
            {
                model.QR_Url = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.MyQR_Code);
            }
            else
            {
                WeChatDefault wx = new WeChatDefault();

                var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
                var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

                var accessToken = GetAccessToken(appId, appSecret);
                //var accessToken = _cacheManager.GetCache(ArtSolutionConsts.CACHE_ACCESS_TOKEN)
                //    .Get(ArtSolutionConsts.CACHE_ACCESS_TOKEN, () => GetAccessToken(appId, appSecret));
                
                string url = string.Format(QRCode_Url, accessToken.access_token);
                expireTime = DateTime.Now.AddSeconds(expire * 24 * 60 * 60);

                var result = wx.QRCode(_cacheManager, appId, appSecret, expire * 24 * 60 * 60, false, this.CustomerId);
                customer.SaveCustomerAttribute<DateTime>(CustomerAttributeNames.QRCodeExpireTime, expireTime);
                customer.SaveCustomerAttribute<string>(CustomerAttributeNames.MyQR_Code, result);

                model.QR_Url = result;
            }
            model.CreateTime = expireTime;
            model.Expire = expire;

            WeChatDefault wechat = new WeChatDefault();
            var config = wechat.WxConfig(_settingService, _cacheManager, this.Request.Url.Host + this.Request.Url.PathAndQuery);
            ViewData["Settings"] = config;
            return View(model);

        }

        public ActionResult Share(int customerId)
        {
            var customer = _customerService.GetCustomerId(customerId);
            var share = string.Empty;
            if (customer != null)
            {
                share = customer.GetCustomerAttributeValue<string>(CustomerAttributeNames.MyQR_Code);

            }
            if (String.IsNullOrWhiteSpace(share))
            {
                share = "/images/qrcode.jpg";
            }

            ViewData["share"] = share;
            return View();
        }

        public ActionResult ApplyPromoter()
        {
            var enabled = _settingService.GetSettingByKey<bool>(PromotersSettings.Enabled);
            if (!enabled)
                return View("ClosePromoter");
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var promoter = customer.GetCustomerAttributeValue<bool>(CustomerAttributeNames.IsPromoter);
            if (promoter)
                return MyQRCode();

            var myOrders = _orderService.GetAllOrders(customerId: this.CustomerId);
            var model = new ApplyPromoterModel();
            model.CustomerId = this.CustomerId;
            model.CostTotal = myOrders.Items.Where(i => i.OrderStatusId == (int)OrderStatus.Complete).Sum(i => i.Subtotal);
            model.Mobile = customer.Mobile;
            model.OrderCount = myOrders.Items.Count(i => i.OrderStatusId == (int)OrderStatus.Complete);
            model.NickName = customer.NickName;

            var condition = _settingService.GetSettingByKey<int>(PromotersSettings.ApplyCondition);
            var value = _settingService.GetSettingByKey<int>(PromotersSettings.ApplyValue);
            switch ((PromoterApplyMode)condition)
            {
                case PromoterApplyMode.Unlimited:
                    model.MayBe = true;
                    break;
                case PromoterApplyMode.OrderTotal:
                    model.MayBe = model.CostTotal >= value;
                    break;
                case PromoterApplyMode.OrderCount:
                    model.MayBe = model.OrderCount >= value;
                    break;
                default:
                    model.MayBe = false;
                    break;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyPromoter(ApplyPromoterModel model)
        {
            var customer = _customerService.GetCustomerId(model.CustomerId);
            var promoter = customer.GetCustomerAttributeValue<bool>(CustomerAttributeNames.IsPromoter);
            if (promoter)
                return MyQRCode();
            _applyService.Insert(new Domain.Customers.ApplyPromoter
            {
                CustomerId = model.CustomerId,
                Audit = false,
                AuditReason = "",
                CreationTime = DateTime.Now,
                Mobile = model.Mobile,
                NickName = model.NickName,
            });

            return View("PromoterList");
        }
        #endregion

        #region 
        #endregion

        #region 我推广的用户
        public ActionResult MyCustomer()
        {
            var customers = _customerService.GetAllCustomers(isSub: true, promoter: this.CustomerId, pageSize: 5);

            var model = new MyCustomerListModel();
            model.TotalCount = customers.TotalCount;
            model.MonthTotal = customers.Items.Where(c => c.CreationTime > DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.Date.Day))).Count();//本月  
            model.WeekTotal = customers.Items.Where(c => c.CreationTime > DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.Date.DayOfWeek))).Count();//本周
            model.Customers = customers.Items.Select(c => c.MapTo<MyCustomerListModel.CustomerModel>()).ToList();
            return View(model);
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public ActionResult MyCustomer(DataSourceRequest command)
        {
            var myCustomers = _customerService.GetAllCustomers(promoter: this.CustomerId, pageIndex: command.Page, pageSize: command.PageSize);
            var jsonData = new DataSourceResult
            {
                ExtraData = myCustomers.Items.Select(c => new
                {
                    Id = c.Id,
                    Name = c.NickName,
                    CreationTime = c.CreationTime.ToString("yyyy/MM/dd")
                }).ToList(),
            };
            return AbpJson(jsonData);
        }

        #endregion

        #region 借款的表单
        [CustomerAntiForgery]
        public ActionResult Loan()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var model = new LoanModel();
            model.Mobile = customer.Mobile;
            foreach (var item in LoanRelevant.Cycle)
            {
                model.AvailableCyclies.Add(new SelectListItem
                {
                    Text = item.Value,
                    Selected = false,
                    Value = item.Key.ToString()
                });
            }
            foreach (var item in LoanRelevant.Quotas)
            {
                model.AvailableQuotas.Add(new SelectListItem
                {
                    Text = item.Value,
                    Selected = false,
                    Value = item.Key.ToString()
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Loan(LoanModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<Loan>();
                var id = _loanService.InsertLoan(entity);
                return RedirectToAction("LoanResult", new { id = id });
            }
            return View(model);
        }

        public ActionResult LoanResult(int id)
        {
            var loan = _loanService.GetLoanById(id);
            var model = loan.MapTo<LoanModel>();
            return View(model);
        }
        #endregion

        #region  收益

        public ActionResult Commission()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var commission = customer.GetCustomerAttributeValue<decimal>(CustomerAttributeNames.Commission);
            ViewData["Commission"] = commission;
            return PartialView();
        }
        [CustomerAntiForgery]
        public ActionResult Earnings()
        {
            var model = new EarningModel();
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var commission = customer.GetCustomerAttributeValue<decimal>(CustomerAttributeNames.Commission);
            model.CustomerId = customer.Id;
            model.Commission = commission;

            var dateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var dateEnd = DateTime.Now.AddMonths(1).AddDays(-1);
            var applies = _applyCashService.GetAllApplyCashs(createdFrom: dateStart, createdTo: dateEnd, customerId: this.CustomerId, audit: (int)AuditStatus.Audited);

            model.CurrentWithdrawalAmount = applies.Items.Sum(a => a.Amount);
            model.CurrentWithdrawals = applies.TotalCount;

            var comms = _commissionServicer.GetAllCommissions(customerId: this.CustomerId, createdFrom: dateStart, createdTo: dateEnd);
            model.CurrentCommissionCount = comms.TotalCount;
            model.CurrentCommissionAmount = comms.Items.Sum(c => c.ReturnAmount);
            return View(model);
        }

        //[CustomerAntiForgery]
        public ActionResult ApplyCash()
        {
            var customer = _customerService.GetCustomerId(this.CustomerId);
            var commission = customer.GetCustomerAttributeValue<decimal>(CustomerAttributeNames.Commission);
            if (commission <= 0)
                return RedirectToAction("Center");

            var model = new ApplyCashModel
            {
                Allowance = commission,
                CustomerId = this.CustomerId,
                Audit = (int)AuditStatus.None,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyCash(ApplyCashModel model)
        {
            var customer = _customerService.GetCustomerId(model.CustomerId);
            var commissions = customer.GetCustomerAttributeValue<decimal>(CustomerAttributeNames.Commission);
            var entity = model.MapTo<ApplyCash>();
            entity.CreationTime = DateTime.Now;
            entity.CustomerId = this.CustomerId;
            entity.Allowance = commissions - model.Amount;

            _applyCashService.InsertApplyCash(entity);

            return RedirectToAction("ResultNotity", "Common", new { contextValue = "您的提现申请已经申请完毕,请等待", url = "/Customer/Center" });
        }
        #endregion

        #region 自动领取优惠券
        #endregion

        #region 愿望单
        public ActionResult Wish()
        {
            var model = new WishOrderModel();
            model.CustomerId = this.CustomerId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Wish(WishOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = model.MapTo<WishOrder>();
                _wishService.InsertWishOrder(entity);
                return RedirectToAction("Center");
            }
            return View(model);
        }
        #endregion
    }
}