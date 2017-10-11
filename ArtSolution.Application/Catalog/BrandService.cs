using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;

namespace ArtSolution.Catalog
{
    public class BrandService : ArtSolutionAppServiceBase, IBrandService
    {
        #region Ctor && Field

        private readonly IRepository<Brand> _brandRepository;

        public BrandService(IRepository<Brand> brandRepository)
        {
            this._brandRepository = brandRepository;
        }

        #endregion

        #region Brand

        public void DeleteBrand(int brandId)
        {
            if (brandId > 0)
                _brandRepository.Delete(brandId);
        }
        public IPagedResult<Brand> GetAllBrands( string keywords = "", 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _brandRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(b => b.Name.Contains(keywords));

            query = query.OrderBy(b => b.DisplayOrder);
            return new PagedResult<Brand>(query, pageIndex, pageSize);
        }
        public Brand GetBrandById(int brandId)
        {
            if (brandId <= 0)
                return null;
            return _brandRepository.Get(brandId);
        }
        public int InsertBrand(Brand brand)
        {
            if (brand == null)
                throw new Exception("brand");
            return _brandRepository.InsertAndGetId(brand);
        }
        public void UpdateBrand(Brand brand)
        {
            if (brand == null)
                throw new Exception("brand");
            _brandRepository.Update(brand);
        }
        #endregion        
    }
}
