using Abp.Application.Services;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    public interface IProductAttributeService: IApplicationService
    {
        /// <summary>
        /// 新增商品属性
        /// </summary>
        /// <param name="attribute"></param>
        int InsertProductAttribute(ProductAttribute attribute);

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="attribute"></param>
        void UpdateProductAttribute(ProductAttribute attribute);

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="id"></param>
        void DeleteProductAttributeById(int  id);

        /// <summary>
        /// 根据主键查询商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductAttribute GetProductAttributeById(int id);

        /// <summary>
        /// 查询商品属性
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<ProductAttribute> GetProductAttributes(int productId);

        /// <summary>
        /// 设置属性的价格
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        void SetProductAttributePrice(int productId, decimal price);
    }
}
