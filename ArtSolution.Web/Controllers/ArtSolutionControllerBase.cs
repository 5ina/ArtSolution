using System.Web.Mvc;
using Abp.Web.Mvc.Controllers;
using System;
using Abp.Web.Mvc.Controllers.Results;
using System.Text;
using Abp.Runtime.Caching;
using ArtSolution.Web.Framework.UIEnums;
using ArtSolution.Web.Framework;
using ArtSolution.Web.Framework.Security;

namespace ArtSolution.Web.Controllers
{
    [WeChatAntiForgery]
    public abstract class ArtSolutionControllerBase : AbpController
    {
        #region 

        public string WeChat_Token
        {
            get
            {
                var cacheManager = Abp.Dependency.IocManager.Instance.Resolve<ICacheManager>();
                return cacheManager.GetCache(ArtSolutionConsts.CACHE_TOKEN).Get(ArtSolutionConsts.CACHE_TOKEN, () =>
                 {
                     return "";
                 });
            }
        }


        public int CustomerId
        {
            get
            {
                return 65;
                //return Convert.ToInt32(AbpSession.UserId);
            }
        }

        #endregion
        
        protected ArtSolutionControllerBase()
        {
            LocalizationSourceName = ArtSolutionConsts.LocalizationSourceName;            
        }
        

        protected ActionResult AjaxResult(AjaxResultStatus status, string content)
        {
            string result = "";
            switch(status) {
                case AjaxResultStatus.Error:
                    result = "<div class=\"alert alert-danger alert-dismissable vertical-center\" role=\"alert\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button>{0}</div>";
                    break;
                case AjaxResultStatus.Success:
                default:
                    result = "<div class=\"alert alert-info alert-dismissable vertical-center\" role=\"alert\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button>{0}</div>";
                    break;
            }
            return Content(string.Format(result, content));
            
        }

        protected override AbpJsonResult AbpJson(object data, string contentType = null,
            Encoding contentEncoding = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet,
            bool wrapResult = true, bool camelCase = false, bool indented = false)
        {            
            return new AbpJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue,
                CamelCase = camelCase,
                Indented = indented,
            };
        }

        protected virtual JavaScriptResult JavaScript(HtmlMessageTypeEnum type, string title, string content = "", int timer = 0)
        {
            var script = type.GetScripts(title, content, timer);
            return JavaScript(script);
        }


        #region 微信登录

        #endregion
    }
}