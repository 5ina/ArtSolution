using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.UI;
using ArtSolution.Common;
using ArtSolution.Domain.Configuration;
using ArtSolution.Names;
using ArtSolution.Web.Areas.Admin.Models.Setting;
using ArtSolution.Web.Framework;
using ArtSolution.Web.Framework.DataGrids;
using ArtSolution.Web.Framework.Mvc;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{

    public class SettingController : ArtSolutionControllerAdminBase
    {
        #region Fields && Ctor

        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public SettingController(ISettingService settingService, ICacheManager cacheManager)
        {
            this._settingService = settingService;
            this._cacheManager = cacheManager;
        }

        #endregion

        public ActionResult System()
        {
            var model = new SystemSettingModel
            {
                SiteName = _settingService.GetSettingByKey<string>(SystemSettingNames.SiteName),
                Description = _settingService.GetSettingByKey<string>(SystemSettingNames.Description),
                Keywords = _settingService.GetSettingByKey<string>(SystemSettingNames.Keywords),
                Tel = _settingService.GetSettingByKey<string>(SystemSettingNames.Tel),
                Title = _settingService.GetSettingByKey<string>(SystemSettingNames.Title),
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult System(SystemSettingModel model)
        {
            _settingService.SaveSetting(SystemSettingNames.SiteName, model.SiteName);
            _settingService.SaveSetting(SystemSettingNames.Description, model.Description);
            _settingService.SaveSetting(SystemSettingNames.Keywords, model.Keywords);
            _settingService.SaveSetting(SystemSettingNames.Tel, model.Tel);
            _settingService.SaveSetting(SystemSettingNames.Title, model.Title);
            model.Result = true;
            return View(model);
        }

        public ActionResult Media()
        {
            var model = new MediaSettingModel
            {
                AccessKeyId = _settingService.GetSettingByKey<string>(MediaSettingNames.AccessKeyId),
                AccessKeySecret = _settingService.GetSettingByKey<string>(MediaSettingNames.AccessKeySecret),
                Bucket = _settingService.GetSettingByKey<string>(MediaSettingNames.Bucket),
                Endpoint = _settingService.GetSettingByKey<string>(MediaSettingNames.Endpoint),                
                IsLocalStorage = _settingService.GetSettingByKey<bool>(MediaSettingNames.IsLocalStorage),
            };

            return View(model);
        }


        [HttpPost]
        [UnitOfWork]
        public ActionResult Media(MediaSettingModel model)
        {
            _settingService.SaveSetting(MediaSettingNames.Endpoint, model.Endpoint);
            _settingService.SaveSetting(MediaSettingNames.Bucket, model.Bucket);
            _settingService.SaveSetting(MediaSettingNames.AccessKeySecret, model.AccessKeySecret);
            _settingService.SaveSetting(MediaSettingNames.AccessKeyId, model.AccessKeyId);
            _settingService.SaveSetting(MediaSettingNames.IsLocalStorage, model.IsLocalStorage);
            model.Result = true;
            return View(model);
        }

        public ActionResult Order()
        {
            var model = new OrderSettingModel
            {
                OrderFailureTime = _settingService.GetSettingByKey<int>(OrderSettingNames.OrderFailureTime),
                OrderFreeShip = _settingService.GetSettingByKey<decimal>(OrderSettingNames.OrderFreeShip),
                ShipFee = _settingService.GetSettingByKey<decimal>(OrderSettingNames.ShipFee),
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Order(OrderSettingModel model)
        {
            _settingService.SaveSetting(OrderSettingNames.OrderFailureTime, model.OrderFailureTime);
            _settingService.SaveSetting(OrderSettingNames.OrderFreeShip, model.OrderFreeShip);
            _settingService.SaveSetting(OrderSettingNames.ShipFee, model.ShipFee);
            model.Result = true;
            return View(model);
        }


        public ActionResult WeChat()
        {
            var model = new WeChatSettingModel
            {
                AppId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId),
                AppSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret),
                Token = _settingService.GetSettingByKey<string>(WeChatSettingNames.Token),
                MchId = _settingService.GetSettingByKey<string>(WeChatSettingNames.MchId),
                Notify_Url = _settingService.GetSettingByKey<string>(WeChatSettingNames.NotifyUrl),
                Key = _settingService.GetSettingByKey<string>(WeChatSettingNames.Key),
                Expire = _settingService.GetSettingByKey<int>(WeChatSettingNames.Expire),
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult WeChat(WeChatSettingModel model)
        {
            _settingService.SaveSetting(WeChatSettingNames.AppId, model.AppId);
            _settingService.SaveSetting(WeChatSettingNames.AppSecret, model.AppSecret);
            _settingService.SaveSetting(WeChatSettingNames.Token, model.Token);
            _settingService.SaveSetting(WeChatSettingNames.MchId, model.MchId);
            _settingService.SaveSetting(WeChatSettingNames.NotifyUrl, model.Notify_Url);
            _settingService.SaveSetting(WeChatSettingNames.Key, model.Key);
            _settingService.SaveSetting(WeChatSettingNames.Expire, model.Expire);

            model.Result = true;
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command, string Keyword = "")
        {
            var list = _settingService.GetAllSettings();
            var jsonData = new DataSourceResult
            {
                Data = list,
                Total = list.Count            
            };
            return AbpJson(jsonData);
        }



        public ActionResult Aliyun()
        {
            var model = new AliyunSettingModel
            {
                AccessKeyId = _settingService.GetSettingByKey<string>(AliyunSettingNames.AccessKeyId),
                SecretAccessKey = _settingService.GetSettingByKey<string>(AliyunSettingNames.SecretAccessKey),
                Endpoint = _settingService.GetSettingByKey<string>(AliyunSettingNames.Endpoint)
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Aliyun(AliyunSettingModel model)
        {
            _settingService.SaveSetting(AliyunSettingNames.AccessKeyId, model.AccessKeyId);
            _settingService.SaveSetting(AliyunSettingNames.SecretAccessKey, model.SecretAccessKey);
            _settingService.SaveSetting(AliyunSettingNames.Endpoint, model.Endpoint);

            model.Result = true;
            _cacheManager.GetCache(CacheNames.Settings.SMS_MESSAGE_SETTINGS).Remove(CacheNames.Settings.SMS_MESSAGE_SETTINGS);
            return View(model);
        }

        /// <summary>
        /// 积分配置
        /// </summary>
        /// <returns></returns>
        public ActionResult Reward()
        {
            var model = _cacheManager.GetCache(CacheNames.Settings.REWARD_SETTINGS).Get(CacheNames.Settings.REWARD_SETTINGS, () =>
            {
                return new RewardSettingModel
                {
                    Enabled = _settingService.GetSettingByKey<bool>(RewardSettingNames.Enabled),
                    ExchangeRate = _settingService.GetSettingByKey<decimal>(RewardSettingNames.ExchangeRate),
                    PointsForRegistration = _settingService.GetSettingByKey<int>(RewardSettingNames.PointsForRegistration),
                    PointsForFirstOrder = _settingService.GetSettingByKey<int>(RewardSettingNames.PointsForFirstOrder),
                    PaymentRate = _settingService.GetSettingByKey<int>(RewardSettingNames.PaymentRate),
                    RewardPaymentEnabled = _settingService.GetSettingByKey<bool>(RewardSettingNames.RewardPaymentEnabled),

                    SignRewardEnabled = _settingService.GetSettingByKey<bool>(RewardSettingNames.SignRewardEnabled),
                    AdditionalReward = _settingService.GetSettingByKey<int>(RewardSettingNames.AdditionalReward),
                    FirstRewardPoint = _settingService.GetSettingByKey<int>(RewardSettingNames.FirstRewardPoint),
                    MaxRewardPoint = _settingService.GetSettingByKey<int>(RewardSettingNames.MaxRewardPoint),

                };
            });
            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Reward(RewardSettingModel model)
        {
            _settingService.SaveSetting(RewardSettingNames.Enabled, model.Enabled);
            _settingService.SaveSetting(RewardSettingNames.ExchangeRate, model.ExchangeRate);
            _settingService.SaveSetting(RewardSettingNames.PointsForRegistration, model.PointsForRegistration);
            _settingService.SaveSetting(RewardSettingNames.PointsForFirstOrder, model.PointsForFirstOrder);

            _settingService.SaveSetting(RewardSettingNames.RewardPaymentEnabled, model.RewardPaymentEnabled);
            _settingService.SaveSetting(RewardSettingNames.PaymentRate, model.PaymentRate);
            
            _settingService.SaveSetting(RewardSettingNames.SignRewardEnabled, model.SignRewardEnabled);
            _settingService.SaveSetting(RewardSettingNames.MaxRewardPoint, model.MaxRewardPoint);
            _settingService.SaveSetting(RewardSettingNames.FirstRewardPoint, model.FirstRewardPoint);
            _settingService.SaveSetting(RewardSettingNames.AdditionalReward, model.AdditionalReward);

            model.Result = true;
            _cacheManager.GetCache(CacheNames.Settings.REWARD_SETTINGS).Remove(CacheNames.Settings.REWARD_SETTINGS);
            return View(model);
        }


        public ActionResult Promoter()
        {
            var model = _cacheManager.GetCache(CacheNames.Settings.PROMOTERS_SETTINGS).Get(CacheNames.Settings.PROMOTERS_SETTINGS, () =>
            {
                return new PromotersSettingModel
                {
                    Enabled = _settingService.GetSettingByKey<bool>(PromotersSettings.Enabled),
                    ModeValue = _settingService.GetSettingByKey<int>(PromotersSettings.ModeValue),
                    RewardMode = _settingService.GetSettingByKey<int>(PromotersSettings.RewardMode),
                    RewardRate = _settingService.GetSettingByKey<decimal>(PromotersSettings.RewardRate),
                    ApplyCondition = _settingService.GetSettingByKey<int>(PromotersSettings.ApplyCondition),
                    ApplyValue = _settingService.GetSettingByKey<int>(PromotersSettings.ApplyValue),
                    IsAudit = _settingService.GetSettingByKey<bool>(PromotersSettings.IsAudit)
                };
            });
            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Promoter(PromotersSettingModel model)
        {
            _settingService.SaveSetting(PromotersSettings.Enabled, model.Enabled);
            _settingService.SaveSetting(PromotersSettings.ModeValue, model.ModeValue);
            _settingService.SaveSetting(PromotersSettings.RewardMode, model.RewardMode);
            _settingService.SaveSetting(PromotersSettings.RewardRate, model.RewardRate);
            _settingService.SaveSetting(PromotersSettings.ApplyCondition, model.ApplyCondition);
            _settingService.SaveSetting(PromotersSettings.ApplyValue, model.ApplyValue);
            _settingService.SaveSetting(PromotersSettings.IsAudit, model.IsAudit);

            model.Result = true;
            _cacheManager.GetCache(CacheNames.Settings.PROMOTERS_SETTINGS).Remove(CacheNames.Settings.PROMOTERS_SETTINGS);
            return View(model);
        }

        public ActionResult Common()
        {
            var model = new CommonModel
            {
                EnabledIcon = _settingService.GetSettingByKey<bool>(CommonSettingNames.EnabledIcon),
                MetaDescription = _settingService.GetSettingByKey<string>(CommonSettingNames.MetaDescription),
                MetaKeywords = _settingService.GetSettingByKey<string>(CommonSettingNames.MetaKeywords),
                MetaTitle = _settingService.GetSettingByKey<string>(CommonSettingNames.MetaTitle),
                StoreName = _settingService.GetSettingByKey<string>(CommonSettingNames.StoreName),
                StoreURL = _settingService.GetSettingByKey<string>(CommonSettingNames.StoreURL),
                EnabledReview = _settingService.GetSettingByKey<bool>(CommonSettingNames.EnabledReview),
            };

            return View(model);
        }

        [HttpPost]
        [UnitOfWork]
        public ActionResult Common(CommonModel model)
        {
            _settingService.SaveSetting(CommonSettingNames.EnabledIcon, model.EnabledIcon);
            _settingService.SaveSetting(CommonSettingNames.MetaDescription, model.MetaDescription);
            _settingService.SaveSetting(CommonSettingNames.MetaKeywords, model.MetaKeywords);
            _settingService.SaveSetting(CommonSettingNames.MetaTitle, model.MetaTitle);
            _settingService.SaveSetting(CommonSettingNames.StoreName, model.StoreName);
            _settingService.SaveSetting(CommonSettingNames.StoreURL, model.StoreURL);
            _settingService.SaveSetting(CommonSettingNames.EnabledReview, model.EnabledReview);
            _cacheManager.GetCache("store.commonheader.model").Remove("store.commonheader.model");
            model.Result = true;
            return View(model);
        }

        #region Update / Delete 

        [HttpPost]
        public ActionResult SettingUpdate(SettingModel model)
        {
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            var setting = _settingService.GetSettingById(model.Id);
            if (setting == null)
                return Content("找不到配置ID");

            setting = model.MapTo<SettingModel, Setting>(setting);
            _settingService.UpdateSetting(setting);
            
            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult SettingDelete(int id)
        {
            var setting = _settingService.GetSettingById(id);
            if (setting == null)
                throw new UserFriendlyException("找不到ID");
            setting.Value = null;
            _settingService.UpdateSetting(setting);
            
            return new NullJsonResult();
        }
        #endregion


    }
}