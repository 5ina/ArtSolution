using Abp.Runtime.Caching;
using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Names;
using ArtSolution.Web.Models.Common;
using System;
using System.Linq;
using System.Web.Mvc;
using ArtSolution.Web.Extensions;

namespace ArtSolution.Web.Controllers
{
    public class CommonController : ArtSolutionControllerBase
    {
        #region ctor && Fields

        private const string COMMONMODELNAME = "store.commonheader.model";


        private readonly ICustomerService _customerService;
        private readonly IAdvertService _advertService;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingService _settingService;

        public CommonController(ICacheManager cacheManager,
            ICustomerService customerService,
            IAdvertService advertService,
            ISettingService settingService)
        {
            this._cacheManager = cacheManager;
            this._advertService = advertService;
            this._customerService = customerService;
            this._settingService = settingService;
        }
        #endregion

        #region Utilities

        [NonAction]
        private CommonHeaderModel GetCommonModel()
        {

            var model = new CommonHeaderModel();
            model.StoreName = _settingService.GetSettingByKey<string>(CommonSettingNames.StoreName);
            model.StoreUrl = _settingService.GetSettingByKey<string>(CommonSettingNames.StoreURL);
            model.EnabledIcon = _settingService.GetSettingByKey<bool>(CommonSettingNames.EnabledIcon);
            return model;
        }
        #endregion

        #region Common Partial View

        [ChildActionOnly]
        public ActionResult HomeFocusMap()
        {
            var adverts = _advertService.GetAllAdverts(showFrom: DateTime.Now, pageIndex: 0, pageSize: 5);
            var models = adverts.Items.Select(a => new AdvertModel
            {
                AdvertUrl = a.AdvertUrl,
                ProductImage = a.AdvertImage,
                Name = a.Name
            }).ToList();

            return PartialView(models);
        }

        [ChildActionOnly]
        public ActionResult Header(string header = "")
        {
            var model = _cacheManager.GetCache(COMMONMODELNAME).Get(COMMONMODELNAME, GetCommonModel);            
            model.HasLoginCustomer = AbpSession.UserId.HasValue; //(this.CustomerId > 0);
            ViewData["Title"] = header;
            return PartialView(model);
        }
        #endregion

        #region Verify
        public ActionResult GetValidateCode()
        {
            int codeLength = 4;
            string code = codeLength.CreateValidateCode(); 
            Session["ValidateCode"] = code;
            byte[] bytes = code.CreateValidateGraphic();
            return File(bytes, @"image/jpeg");
        }

        #endregion

        #region Notity
        public ActionResult ResultNotity(string contextValue, string url = "/")
        {
            ViewData["contextValue"] = contextValue;
            ViewData["url"] = url;
            return View();
        }
        #endregion

    }
}