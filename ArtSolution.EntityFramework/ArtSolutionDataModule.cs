using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ArtSolution.EntityFramework;

namespace ArtSolution
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(ArtSolutionCoreModule))]
    public class ArtSolutionDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<ArtSolutionDbContext>(null);
        }
    }
}
