using Abp.Runtime.Session;
using ArtSolution.Common;
using ArtSolution.Names;
using Castle.Core.Logging;
using System;
using System.Web.Mvc;

namespace ArtSolution.Web.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class WeChatAntiForgeryAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var abpSession = Abp.Dependency.IocManager.Instance.Resolve<IAbpSession>();            
            var User_Agent = filterContext.RequestContext.HttpContext.Request.UserAgent;
            //判定是否微信打开
            if (!User_Agent.ToLower().Contains("micromessenger"))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            var accountControllerName = string.Concat("ArtSolution.Web.Controllers", ".", "WechatController");
            
            //判定是否微信控制器
            string controllerName = filterContext.Controller.ToString();
            if (controllerName.Equals(accountControllerName, StringComparison.InvariantCultureIgnoreCase))
            {
                base.OnActionExecuting(filterContext);
                return;
            }


            //判定是否子方法
            if (filterContext.IsChildAction)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            //判定当前访问用户是否在微信中打开
            if (!abpSession.UserId.HasValue)
            {
                var Logger = Abp.Dependency.IocManager.Instance.Resolve<ILogger>();
                Logger.Debug("this. URL : /" + controllerName + "/" + filterContext.ActionDescriptor.ActionName);
                var _settingService = Abp.Dependency.IocManager.Instance.Resolve<ISettingService>();
                var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
                var resultPath = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                    appId,
                    System.Web.HttpUtility.UrlEncode("http://" + filterContext.RequestContext.HttpContext.Request.Url.Host + "/Wechat/OAuth", System.Text.Encoding.UTF8),
                    filterContext.RequestContext.HttpContext.Request.Url);
                filterContext.Result = new RedirectResult(resultPath);
            }
        }
    }
}