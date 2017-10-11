using Abp.Application.Services;
using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    public interface IProductImagesService: IApplicationService
    {
        /// <summary>
        /// 新增图片
        /// </summary>
        /// <param name="image"></param>
        void InsertImage(ProductImage image);
        
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imageId"></param>
        void DeleteImage(int imageId);

        /// <summary>
        /// 清空图片
        /// </summary>
        /// <param name="productId"></param>
        void ClearImages(int productId);

        /// <summary>
        /// 根据主键获取图片
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        ProductImage GetProductImageById(int imageId);

        /// <summary>
        /// 获取商品的图片集
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<ProductImage> GetProductImagesByProductId(int productId);
    }
}
