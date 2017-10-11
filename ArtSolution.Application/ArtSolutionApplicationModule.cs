using System.Reflection;
using Abp.Modules;
using Microsoft.Owin.Security;
using ArtSolution.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ArtSolution.Authentication.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;

namespace ArtSolution
{
    [DependsOn(typeof(ArtSolutionCoreModule), typeof(AbpAutoMapperModule))]
    public class ArtSolutionApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                mapper.CreateMap<Customer, CustomerDto>();
            });
            IocManager.Register<IAuthenticationManager, AuthenticationManager>();
            IocManager.Register<IUserStore<CustomerDto, int>, CustomerStore>();
            IocManager.Register<UserManager<CustomerDto, int>, CustomerManager>();
            IocManager.Register<SignInManager<CustomerDto, int>, LoginManager>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
