using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtSolution.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();//注册属性路由

            //ASP.NET Web API Route Config
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ArtSolution.Web.Controllers" }
            );

            routes.Add("Official", new DomainRoute("www.jie-pang.com", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }));

            //授权
            routes.MapRoute(
                name: "OAuth",
                url: "oauth",
                defaults: new { controller = "WeChat", action = "OAuthUserInfo", id = UrlParameter.Optional },
                namespaces: new[] { "ArtSolution.Web.Controllers" }
            );


            routes.Add(
                "DomainRoute", new DomainRoute(
                "admin.bb-girl.cn",
                "{controller}/{action}/{id}",
                new
                {
                    area = "Admin",
                    controller = "Dashboard",
                    action = "Index",
                    id = "",
                    Namespaces = new string[] { "ArtSolution.Web.Areas.Admin" }
                }
                ));
        }
    }
}
