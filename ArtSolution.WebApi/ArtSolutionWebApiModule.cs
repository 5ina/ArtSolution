using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using ArtSolution.Api;
using ArtSolution.Api.Account;
using ArtSolution.Api.Catalog;
using ArtSolution.Api.Customers;
using ArtSolution.Api.Messages;
using ArtSolution.Api.WeChat;
using Swashbuckle.Application;
using System.Linq;
using System.Reflection;


namespace ArtSolution
{
    [DependsOn(typeof(AbpWebApiModule), typeof(ArtSolutionApplicationModule))]
    public class ArtSolutionWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
            //    .ForAll<IApiService>(typeof(ArtSolutionApplicationModule).Assembly, "api")
            //    .Build();

            //Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationAttribute("Bearer"));

            //var cors = new EnableCorsAttribute("*", "*", "*");
            //GlobalConfiguration.Configuration.EnableCors(cors);

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<ITokenApiService>("app/wechat").Build();

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<ICustomerApiService>("app/customer").Build();
           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<ICustomerAttributeApiService>("app/customerattributes").Build();

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<IAccountApiService>("app/account").Build();

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<ICategoryApiService>("app/category").Build();

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<IProductApiService>("app/product").Build();

           // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
           //     .For<INoticeApiService>("app/notice").Build();

           // Configuration.Modules.AbpWebApi().HttpConfiguration
           //.EnableSwagger(c =>
           //{
           //    c.SingleApiVersion("v1", "SwaggerIntegration.WebApi");
           //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
           //})
           //.EnableSwaggerUi();
        }
    }
}
