using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;

namespace ArtSolution.Catalog
{
    /// <summary>
    /// 商品评论
    /// </summary>
    public interface IProductReviewService: IApplicationService
    {
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="review"></param>
        void InsertReview(ProductReview review);

        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="review"></param>
        void UpdateReview(ProductReview review);

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="reviewId"></param>
        void DeleteReviewById(int reviewId);
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="productId"></param>
        void DeletereviewByProductId(int productId);

        /// <summary>
        /// 根据主键获取评论
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        ProductReview GetProductReview(int reviewId);

        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ProductReview> GetAllProductReviews(int productId = 0, int customerId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
