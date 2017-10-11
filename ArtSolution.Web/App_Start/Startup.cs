using System;
using ArtSolution.Web.App_Start;
using System.Configuration;
using Owin;

[assembly: Microsoft.Owin.OwinStartup(typeof(Startup))]
namespace ArtSolution.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseAbp();

            //app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);



            //ENABLE TO USE HANGFIRE dashboard (Requires enabling Hangfire in ArtSolutionWebModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter() } //You can remove this line to disable authorization
            //});

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/Login"),
                CookieHttpOnly = true,
                CookieName = "__ArtSolution_WEB"
            });

            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
                       

        }

        private static bool IsTrue(string appSettingName)
        {
            return string.Equals(
                ConfigurationManager.AppSettings[appSettingName],
                "true",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
