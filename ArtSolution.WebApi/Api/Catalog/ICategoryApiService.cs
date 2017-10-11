using ArtSolution.Api.Models.Catalog;
using System.Web.Http;

namespace ArtSolution.Api.Catalog
{
    /// <summary>
    /// 类别服务接口
    /// </summary>
    public interface ICategoryApiService: IApiService
    {
        /// <summary>
        /// 根据主键获取类别信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<CategoryModel> GetCategoryById(int categoryId);

        /// <summary>
        /// 根据父Id获取类别集合
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<CategoryModel> GetCategoriesByParentId(int parentId);
        
        [HttpGet]
        ResultMessage<CategoryModel> GetAllCategories(int pageIndex, int pageSize);
    }
}
