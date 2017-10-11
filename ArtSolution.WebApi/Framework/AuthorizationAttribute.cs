using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace ArtSolution.Framework
{
    /// <summary>
    /// Request Headers 扩展类
    /// </summary>
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null || actionContext.Request == null || actionContext.Request.Headers == null)
                return;

            if (actionContext.Request.Headers.VerifyToken())
                base.OnActionExecuting(actionContext);
            actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
        }
    }
}
