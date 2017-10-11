using ArtSolution.Api.Models.Catalog;
using ArtSolution.Domain.Catalog;
using System.Web.Http;

namespace ArtSolution.Api.Catalog
{
    /// <summary>
    /// 商品服务接口
    /// </summary>
    public interface IProductApiService : IApiService
    {
        /// <summary>
        /// 根据商品ID获取商品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<ProductModel> GetProductById(int productId);

        /// <summary>
        /// 根据商品类别获取商品集合
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<ProductModel> GetProductListByCategoryId(int categoryId, int pageIndex, int pageSize);

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<ProductModel> SearchProductList(int categoryId, string keywords, int pageIndex, int pageSize);
    }
}