using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Threading;
using System.Threading.Tasks;
using ArtSolution.Authentication.Dto;
using ArtSolution.Authentication;
using Abp.Runtime.Caching;
using ArtSolution.Web.Areas.Admin.Models.Layout;
using System;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class LayoutController : ArtSolutionControllerAdminBase
    {

        #region Constructors && Fields

        private const string CURRENTCUSTOMER = "art.current.customer-{0}";

        private readonly ICacheManager _cacheManager;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly ILanguageManager _languageManager;
        private readonly CustomerManager _customerManager;

        public LayoutController(
            ICacheManager cacheManager,
            IUserNavigationManager userNavigationManager,
            ILocalizationManager localizationManager,
            ILanguageManager languageManager,
            CustomerManager customerManager)
        {
            this._userNavigationManager = userNavigationManager;
            this._languageManager = languageManager;
            this._customerManager = customerManager;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Utilities
        [NonAction]
        public async Task<CustomerDto> GetCurrentCustomerAsync()
        {
            string key = string.Format(CURRENTCUSTOMER, AbpSession.UserId);
            return await _cacheManager.GetCache(key).Get(key,
                () =>
                {
                    return _customerManager.FindByIdAsync(Convert.ToInt32(AbpSession.UserId));
                });
        }
        #endregion

        #region Method
        [ChildActionOnly]
        public PartialViewResult TopMenu(string activeMenu = "")
        {
            var model = new TopMenuViewModel
            {
                MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
                ActiveMenuItemName = activeMenu
            };

            return PartialView("_TopMenu", model);
        }

        [ChildActionOnly]
        public PartialViewResult LanguageSelection()
        {
            var model = new LanguageSelectionViewModel
            {
                CurrentLanguage = _languageManager.CurrentLanguage,
                Languages = _languageManager.GetLanguages()
            };

            return PartialView("_LanguageSelection", model);
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var customer = AsyncHelper.RunSync(() => GetCurrentCustomerAsync());

            var model = new HeaderViewModel
            {
                CustomerId = customer.Id,
                Mobile = customer.Mobile
            };

            return PartialView("_Header", model);
        }

        [ChildActionOnly]
        public PartialViewResult Navigation()
        {
            return PartialView("_Navigation");
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            return PartialView("_Footer");
        }

        #endregion
    }
}