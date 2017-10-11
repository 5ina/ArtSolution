using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Notifications;
using Abp.Runtime.Session;
using ArtSolution.Authentication;
using ArtSolution.Authentication.Dto;
using ArtSolution.Customers;
using ArtSolution.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class AccountController : ArtSolutionControllerAdminBase
    {
        #region Fields && Ctor

        private readonly LoginManager _loginManager;
        private readonly CustomerManager _customerManager;
        private readonly ICustomerService _customerService;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public AccountController(ICustomerService customerService,
                                LoginManager loginManager,
                                CustomerManager customerManager,
                                INotificationSubscriptionManager notificationSubscriptionManager)
        {
            this._customerService = customerService;
            this._loginManager = loginManager;
            this._customerManager = customerManager;
            this._notificationSubscriptionManager = notificationSubscriptionManager;
        }

        #endregion

        #region Utilities
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion

        #region Login / Logout
        public ActionResult Login(string returnUrl = "")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.ApplicationPath;
            }

            ViewBag.returnUrl = returnUrl;
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [DisableAuditing]
        public ActionResult Login(LoginModel model)
        {
            var loginResult = _customerService.ValidateCustomer(model.LoginName, model.Password);
            switch (loginResult.Result)
            {
                case LoginResults.Successful:
                    {
                        var customerDto = loginResult.Customer.MapTo<CustomerDto>();
                        //生成ClaimsIdentity
                        var identity = _loginManager.CreateUserIdentity(customerDto);

                        //用户登录
                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);
                        _notificationSubscriptionManager.Subscribe(new Abp.UserIdentifier(null, (long)customerDto.Id), "notice");
                        if (String.IsNullOrEmpty(model.ReturnUrl) || !Url.IsLocalUrl(model.ReturnUrl))
                            return RedirectToAction("Index", "Dashboard");

                        return Redirect(model.ReturnUrl);
                    }

                case LoginResults.Deleted:
                    ModelState.AddModelError("", L("Account.Login.WrongCredentials.Deleted"));
                    break;
                case LoginResults.NotRegistered:
                    ModelState.AddModelError("", L("Account.Login.WrongCredentials.NotRegistered"));
                    break;
                case LoginResults.Unauthorized:
                    ModelState.AddModelError("", L("Account.Login.WrongCredentials.Unauthorized"));
                    break;
                case LoginResults.WrongPassword:
                default:
                    ModelState.AddModelError("", L("Account.Login.WrongCredentials"));
                    break;
            }

            return View(model);
        }


        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            AbpSession = NullAbpSession.Instance;
            return Redirect("/admin");
        }

        #endregion

    }
}