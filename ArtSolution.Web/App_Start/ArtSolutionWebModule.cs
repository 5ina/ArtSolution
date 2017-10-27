using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using ArtSolution.Web.Areas.Admin.Models.Customers;

namespace ArtSolution.Web
{
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(ArtSolutionDataModule), 
        typeof(ArtSolutionApplicationModule), 
        typeof(ArtSolutionWebApiModule),
        typeof(AbpAutoMapperModule))]
    public class ArtSolutionWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                mapper.CreateMap<Customer, CustomerModel>();
            });
            //Add/remove languages for your application
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england"));
            

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ArtSolutionConsts.LocalizationSourceName,
                    new XmlFileLocalizationDictionaryProvider(
                        HttpContext.Current.Server.MapPath("~/Localization/ArtSolution")
                        )
                    )
                );

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<ArtSolutionNavigationProvider>();

            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });

            //为特定的缓存配置有效期
            Configuration.Caching.Configure(ArtSolutionConsts.CACHE_TOKEN, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromSeconds(3600);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
