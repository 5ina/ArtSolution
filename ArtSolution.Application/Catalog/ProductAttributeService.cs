using System;
using System.Collections.Generic;
using System.Linq;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;

namespace ArtSolution.Catalog
{
    public class ProductAttributeService : ArtSolutionAppServiceBase, IProductAttributeService
    {

        #region Ctor && Field

        private readonly IRepository<ProductAttribute> _productAttributeRepository;

        public ProductAttributeService(IRepository<ProductAttribute> productAttributeRepository)
        {
            this._productAttributeRepository = productAttributeRepository;
        }


        #endregion

        #region Method

        public void DeleteProductAttributeById(int id)
        {
            try
            {
                _productAttributeRepository.Delete(id);
            }
            catch { }
        }

        public ProductAttribute GetProductAttributeById(int id)
        {
            try
            {
               return _productAttributeRepository.Get(id);
            }
            catch
            {
                return null;
            }
        }

        public IList<ProductAttribute> GetProductAttributes(int productId)
        {
            var query = _productAttributeRepository.GetAllList(a => a.ProductId == productId);
            return query.ToList();
        }
        
        public int InsertProductAttribute(ProductAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("ProductAttribute");

            return _productAttributeRepository.InsertAndGetId(attribute);
        }

        public void SetProductAttributePrice(int productId, decimal price)
        {
            var attributes = _productAttributeRepository.GetAllList(a => a.ProductId == productId);

            foreach (var item in attributes)
            {
                item.Price = price;
                _productAttributeRepository.Update(item);
            }
        }

        public void UpdateProductAttribute(ProductAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("ProductAttribute");

            _productAttributeRepository.Update(attribute);
        }
        #endregion
    }
}
