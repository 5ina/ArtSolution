using Abp.WebApi.Controllers;
using System.Collections.Generic;
using ArtSolution.Catalog;
using ArtSolution.Api.Models.Catalog;
using Abp.AutoMapper;

namespace ArtSolution.Api.Catalog
{
    public class ProductApiService : AbpApiController, IProductApiService
    {
        #region Ctor && Field

        private readonly IProductService _productService;

        public ProductApiService(IProductService productService)
        {
            this._productService = productService;
        }

        #endregion

        #region Method
        public ResultMessage<ProductModel> GetProductById(int productId)
        {
            var product = _productService.GetProductById(productId);
            return new ResultMessage<ProductModel>(product.MapTo<ProductModel>());
        }

        public ResultMessage<ProductModel> GetProductListByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var categories = new List<int>();
            categories.Add(categoryId);
            var products = _productService.GetAllProducts(categoryIds: categories, pageIndex: pageIndex, pageSize: pageSize);
            var models = products.Items.MapTo<List<ProductModel>>();
            return new ResultMessage<ProductModel>(models);
        }

        public ResultMessage<ProductModel> SearchProductList(int categoryId, string keywords, int pageIndex, int pageSize)
        {
            var categories = new List<int>();
            categories.Add(categoryId);
            var products = _productService.GetAllProducts(keywords: keywords, categoryIds: categories, pageIndex: pageIndex, pageSize: pageSize);
            var models = products.Items.MapTo<List<ProductModel>>();
            return new ResultMessage<ProductModel>(models);
        }
        #endregion
    }
}
