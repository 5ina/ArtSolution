using Abp.Application.Services;

namespace ArtSolution.Layout
{
    public interface IPageHeadBuilder: IApplicationService
    {
        string GenerateTitle();
        
        /// <summary>
        /// 生成说明
        /// </summary>
        /// <returns></returns>
        string GenerateMetaDescription();
        
        /// <summary>
        /// 生成关键字
        /// </summary>
        /// <returns></returns>
        string GenerateMetaKeywords();
    }
}
