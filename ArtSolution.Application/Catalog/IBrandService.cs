using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 品牌服务接口
    /// </summary>
    public interface IBrandService : IApplicationService
    {
        #region Brand

        /// <summary>
        /// 新增品牌
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        int InsertBrand(Brand brand);

        /// <summary>
        /// 更新品牌
        /// </summary>
        /// <param name="brand"></param>
        void UpdateBrand(Brand brand);

        /// <summary>
        /// 删除品牌
        /// </summary>
        /// <param name="brandId"></param>
        void DeleteBrand(int brandId);

        /// <summary>
        /// 根据主键获取品牌
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        Brand GetBrandById(int brandId);

        /// <summary>
        /// 获取所有品牌
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Brand> GetAllBrands(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion        
    }
}
