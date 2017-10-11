using Abp.Application.Services;

namespace ArtSolution
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ArtSolutionAppServiceBase : ApplicationService
    {
        protected ArtSolutionAppServiceBase()
        {
            LocalizationSourceName = ArtSolutionConsts.LocalizationSourceName;
        }
    }
}