using Abp.Runtime.Caching;
using ArtSolution.Common;
using ArtSolution.Models.WeChats;
using ArtSolution.Names;
using ArtSolution.Web.Areas.Admin.Models.WeChat;
using ArtSolution.Web.Controllers;
using ArtSolution.Web.Framework.Controllers;
using ArtSolution.Web.Framework.DataGrids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 微信管理控制器
    /// </summary>
    public class WeChatManagerController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        /// <summary>
        /// 客服账号
        ///  {0}：access_token
        /// </summary>
        private readonly string kf_Url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}";

        /// <summary>
        /// TokenUrl 
        ///  {0}：AppId
        ///  {1}：secret
        /// </summary>
        private readonly string token_Url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 创建客服url
        /// {0}：token
        /// </summary>
        private readonly string kf_create_url = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token={0}";

        private readonly string CACHE_ACCESSTOKEN = "wechat.token";

        private readonly ISettingService _settingService;

        private readonly ICacheManager _cacheManager;
        public WeChatManagerController(ISettingService settingService, ICacheManager cacheManager)
        {
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            
        }
        #endregion


        #region Utilities
        [NonAction]
        public AccessToken GetAccessToken()
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);
            AccessToken token = HttpUtility.Get<AccessToken>(string.Format(token_Url, appId, appSecret));
            return token;
        }
        #endregion

        #region Kf
        public ActionResult KfList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KfListData()
        {
            var token = this._cacheManager.GetCache(CACHE_ACCESSTOKEN).Get(CACHE_ACCESSTOKEN, () => GetAccessToken());
            string url = string.Format(kf_Url, token.access_token);
            var list = HttpUtility.Get<KfAccountListModel>(url);
            var jsonData = new DataSourceResult
            {
                ExtraData = list.kf_list
            };
            return AbpJson(jsonData);
        }

        public ActionResult KfCreate()
        {
            var model = new KfModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult KfCreate(KfModel model, bool continueEditing)
        {
            var token = this._cacheManager.GetCache(CACHE_ACCESSTOKEN).Get(CACHE_ACCESSTOKEN, () => GetAccessToken());
            var kValue = new Dictionary<string, string>();
            kValue.Add("kf_account", model.kf_account + "@wochegang1568");
            kValue.Add("nickname", model.nickname);
            kValue.Add("password", model.password);
            string url = string.Format(kf_create_url, token.access_token);
            var result = HttpUtility.Post(url, kValue);

            if (result.errcode == "0")
            {
                return KfList();
            }
            else
            {
                ViewData["error"] = result.errmsg;
                return View(model);
            }



        }
        #endregion

        #region WechatNavicate
        public ActionResult MenuList()
        {
            var menuUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
            var token = this._cacheManager.GetCache(CACHE_ACCESSTOKEN).Get(CACHE_ACCESSTOKEN, () => GetAccessToken());
            string url = string.Format(menuUrl, token.access_token);
            var list = HttpUtility.Get<KfAccountListModel>(url);
            var jsonData = new DataSourceResult
            {
                ExtraData = list.kf_list
            };
            return AbpJson(jsonData);
        }
        
        
        #endregion
    }
}