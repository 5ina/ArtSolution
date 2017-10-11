using Abp.Runtime.Caching;
using ArtSolution.Common;
using ArtSolution.Names;
using System;
using System.Web.Mvc;

namespace ArtSolution.Web.Framework.WeChat
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class WeChatSessionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cacheManager = Abp.Dependency.IocManager.Instance.Resolve<ICacheManager>();


        }


        public ActionResult OAuthUserInfo()
        {
            var _settingService = Abp.Dependency.IocManager.Instance.Resolve<ISettingService>();

            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            string code = Request.QueryString["code"];
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    OAuthToken oauthToken = HttpUtility.Get<OAuthToken>(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appSecret, code));

                    if (oauthToken != null && !string.IsNullOrEmpty(oauthToken.openid) && !string.IsNullOrEmpty(oauthToken.access_token))
                    {

                        OAuthUserInfo userInfo = GetValue<OAuthUserInfo>(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", oauthToken.access_token, oauthToken.openid));
                        if (userInfo != null)
                        {

                            Logger.Debug("获取到用户信息nickName:" + userInfo.nickname);
                            ViewData["headImage"] = userInfo.headimgurl;
                            ViewData["openid"] = userInfo.openid;
                            ViewData["nickName"] = userInfo.nickname;
                            if (userInfo.sex == 0)
                            {
                                ViewData["sex"] = "未知";
                            }
                            else if (userInfo.sex == 1)
                            {
                                ViewData["sex"] = "男";
                            }
                            else
                            {
                                ViewData["sex"] = "女";
                            }
                            ViewData["province"] = userInfo.province;
                            ViewData["city"] = userInfo.city;
                        }
                        else
                        {
                            Logger.Debug("未获取到用户信息");
                        }
                    }
                    else
                    {
                        Logger.Debug("access_token:" + oauthToken.access_token + ",openid:" + oauthToken.openid);
                    }

                }
                else
                {
                    return Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=123456#wechat_redirect", appId, Server.UrlEncode("http://" + Request.Url.Host + Url.Action("OAuthUserInfo"))));
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("OAuthUserInfo:" + ex.Message);
                ViewData["errmsg"] = ex.Message;
            }

            return View();
        }
    }
}