using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 商品组合套餐服务接口
    /// </summary>
    public interface IComBoProductService: IApplicationService
    {

        #region ComBoProduct

        /// <summary>
        /// 新增组合套餐
        /// </summary>
        /// <param name="combo"></param>
        void InsertComBoProduct(ComBoProduct combo);

        /// <summary>
        /// 更新组合套餐
        /// </summary>
        /// <param name="combo"></param>
        void UpdateComBoProduct(ComBoProduct combo);

        /// <summary>
        /// 删除组合套餐
        /// </summary>
        /// <param name="comboId"></param>
        void DeleteComBoProduct(int comboId);

        /// <summary>
        /// 获取组合套餐
        /// </summary>
        /// <param name="comboId"></param>
        /// <returns></returns>
        ComBoProduct GetComBoProductById(int comboId);
        /// <summary>
        /// 获取所有套餐
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ComBoProduct> GetComBoProducts(string keywords = "",int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion

        #region ComBoProductMapping

        /// <summary>
        /// 把商品加入到组合套餐中
        /// </summary>
        /// <param name="comBoId"></param>
        /// <param name="productId"></param>
        void InsertMapping(int comBoId, int productId);

        /// <summary>
        /// 把商品集合加入套餐内
        /// </summary>
        /// <param name="comBoId"></param>
        /// <param name="productIds"></param>
        void InsertMapping(int comBoId, List<int> productIds);

        /// <summary>
        /// 把商品移除套餐
        /// </summary>
        /// <param name="comBoId"></param>
        /// <param name="productId"></param>
        void DeleteMapping(int comBoId, int productId);

        /// <summary>
        /// 删除套餐内所有商品
        /// </summary>
        /// <param name="comBoId"></param>
        void DeleteMapping(int comBoId);

        /// <summary>
        /// 获取套餐内的商品集合Id
        /// </summary>
        /// <param name="comboId"></param>
        /// <returns></returns>
        List<int> GetComBoProductMappings(int comboId);


        #endregion
    }
}
