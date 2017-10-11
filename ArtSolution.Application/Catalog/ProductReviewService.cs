using Abp.Domain.Repositories;
using ArtSolution.Domain.Catalog;
using System;
using System.Linq;
using Abp.Application.Services.Dto;

namespace ArtSolution.Catalog
{
    public class ProductReviewService:ArtSolutionAppServiceBase, IProductReviewService
    {

        #region Ctor && Field



        private readonly IRepository<ProductReview> _reviewRepository;

        public ProductReviewService(IRepository<ProductReview> reviewRepository)
        {
            this._reviewRepository = reviewRepository;
        }

        #endregion

        #region Method

        public void DeleteReviewById(int reviewId)
        {
            if (reviewId > 0)
                _reviewRepository.Delete(reviewId);
        }

        public void DeletereviewByProductId(int productId)
        {
            if (productId > 0)
                _reviewRepository.Delete(r => r.ProductId == productId);
        }

        public IPagedResult<ProductReview> GetAllProductReviews(int productId = 0, int customerId = 0, 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _reviewRepository.GetAll();
            
            if (productId > 0)
                query = query.Where(r => r.ProductId == productId);
            if (customerId > 0)
                query = query.Where(r => r.CreatorUserId == customerId);

            query = query.OrderByDescending(r => r.CreationTime);

            return new PagedResult<ProductReview>(query, pageIndex, pageSize);
        }

        public ProductReview GetProductReview(int reviewId)
        {
            if (reviewId <= 0)
                return null;
            return _reviewRepository.Get(reviewId);
        }

        public void InsertReview(ProductReview review)
        {
            if (review == null)
                throw new Exception("review");
            _reviewRepository.Insert(review);
        }

        public void UpdateReview(ProductReview review)
        {
            if (review == null)
                throw new Exception("review");

            _reviewRepository.Update(review);
        }


        #endregion
    }
}
