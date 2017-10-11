using Abp.Runtime.Session;
using Abp.UI;
using System;
using System.Web.Mvc;

namespace ArtSolution.Web.Framework.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class CustomerAntiForgeryAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var abpSession = Abp.Dependency.IocManager.Instance.Resolve<IAbpSession>();
                if (!abpSession.UserId.HasValue)
                {
                    throw new UserFriendlyException("操作错误","请在微信中操作");                    
                }
            }
            base.OnActionExecuting(filterContext);
        }
        
        
    }
}