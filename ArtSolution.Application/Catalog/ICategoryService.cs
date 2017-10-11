using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 商品类别服务接口
    /// </summary>
    public interface ICategoryService: IApplicationService
    {
        /// <summary>
        /// 新增类别
        /// </summary>
        /// <param name="category"></param>
        int InsertCategory(Category category);

        /// <summary>
        /// 更新类别
        /// </summary>
        /// <param name="cateogry"></param>
        void UpdateCategory(Category category);

        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="categoryId"></param>
        void DeleteCatgory(int categoryId);
        
        /// <summary>
        /// 根据主键获取类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Category GetCategoryById(int categoryId);

        /// <summary>
        /// 根据父Id获取子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IList<Category> GetCategoriesByParentId(int parentId, bool showHidden = false);

        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="parentId"></param>
        /// <param name="showHidden"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Category> GetAllCategories(string categoryName = "", int? parentId = null
            , bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
