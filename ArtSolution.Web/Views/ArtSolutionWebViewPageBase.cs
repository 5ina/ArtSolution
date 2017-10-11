using Abp.Web.Mvc.Controllers.Results;
using Abp.Web.Mvc.Views;
using System.Text;
using System.Web.Mvc;

namespace ArtSolution.Web.Views
{
    public abstract class ArtSolutionWebViewPageBase : ArtSolutionWebViewPageBase<dynamic>
    {

    }

    public abstract class ArtSolutionWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected ArtSolutionWebViewPageBase()
        {
            LocalizationSourceName = ArtSolutionConsts.LocalizationSourceName;
        }
    }
}