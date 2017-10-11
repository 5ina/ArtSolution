using System.Reflection;
using Abp.Modules;

namespace ArtSolution
{
    public class ArtSolutionCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
