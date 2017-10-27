using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ArtSolution.Catalog
{
    public class ProductService: ArtSolutionAppServiceBase,IProductService
    {
        #region Ctor && Field

        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;
        public ProductService(IRepository<Product> productRepository,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._productRepository = productRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }
        #endregion

        #region Method


        public void DeleteProduct(int productId)
        {
            if (productId > 0)
                _productRepository.Delete(productId);
        }

        public IPagedResult<Product> GetAllProducts(string keywords = null,
            IList<int> categoryIds = null, int brand = 0,
            decimal? priceMin = null, decimal? priceMax = null,
            bool showHidden = false, bool? isPre = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            using (_unitOfWorkManage.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = _productRepository.GetAll();

                if (!String.IsNullOrWhiteSpace(keywords))
                    query = query.Where(p => p.Name.Contains(keywords));

                if (brand > 0)
                    query = query.Where(p => p.BrandId == brand);

                if (!showHidden)
                    query = query.Where(p => !p.IsDeleted);

                if (categoryIds != null && categoryIds.Count() > 0 && !(categoryIds.Count == 1 && categoryIds[0] == 0))
                    query = query.Where(p => categoryIds.Contains(p.CategoryId));

                if (priceMin.HasValue)
                    query = query.Where(p => p.Price > priceMin.Value);

                if (priceMax.HasValue)
                    query = query.Where(p => p.Price < priceMax.Value);

                if (isPre.HasValue)
                    query = query.Where(p => p.PreSell == isPre.Value);

                query = query.OrderByDescending(p => p.DisplayOrder);
                return new PagedResult<Product>(query, pageIndex, pageSize);
            }
        }

        public Product GetProductById(int productId)
        {
            try
            {
                using (_unitOfWorkManage.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    return _productRepository.Get(productId);
                }
            }
            catch
            {
                return null;
            }
        }

        public List<Product> GetProductByIds(List<int> productIds)
        {
            var query = _productRepository.GetAll();
            query = query.Where(p => productIds.Contains(p.Id));

            return query.ToList();                
            
        }

        public int InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            return _productRepository.InsertAndGetId(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            using (_unitOfWorkManage.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                _productRepository.Update(product);
            }
        }


        #endregion
    }
}
