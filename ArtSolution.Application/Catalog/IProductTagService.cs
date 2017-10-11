using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 商品标签服务接口
    /// </summary>
    public interface IProductTagService : IApplicationService
    {
        #region Product-Tag

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="tag"></param>
        void InsertTag(ProductTag tag);
        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="tag"></param>
        void UpdateTag(ProductTag tag);
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tagId"></param>
        void DeleteTag(int tagId);
        /// <summary>
        /// 查询标签
        /// </summary>
        /// <param name="productTagId"></param>
        /// <returns></returns>
        ProductTag GetTag(int productTagId);

        /// <summary>
        /// 所有标签
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ProductTag> GetAllTags(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion


        #region Product-ProductTag-Mapping
        void InsertMapping(ProductTagMapping mapping);

        void DeleteMappingById(int productMappingId);

        void DeleteMappingByProductId(int productId);

        void DeleteMappingByTagId(int tagId);
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        IList<ProductTagMapping> GetAllProductTagMappings(int productId = 0, int tagId = 0);
        #endregion
    }
}
