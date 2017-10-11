using Abp.Web.Mvc.Controllers;
using Abp.Web.Mvc.Controllers.Results;
using System;
using System.Text;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{

    [RouteArea("Admin")]
    public abstract class ArtSolutionControllerAdminBase : AbpController
    {
        protected ArtSolutionControllerAdminBase()
        {
            LocalizationSourceName = ArtSolutionConsts.LocalizationSourceName;

        }



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //子节点抛出
            if (filterContext.IsChildAction)
                return;
            if (AbpSession.UserId == 0)
            {

            }
            var accountControllerName = string.Concat(this.GetType().Namespace, ".", "AccountController");

            string controllerName = filterContext.Controller.ToString();
            //string actionName = filterContext.ActionDescriptor.ActionName;

            if (!controllerName.Equals(accountControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath;
                filterContext.Result = new RedirectResult("/Admin/Account/Login?returnUrl=" + returnUrl);
                //filterContext.Result = new RedirectResult(string.Concat("/login?returnUrl=", filterContext.RequestContext.HttpContext.Request.Url.ToString()));
            }

            base.OnActionExecuting(filterContext);
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
    }
}