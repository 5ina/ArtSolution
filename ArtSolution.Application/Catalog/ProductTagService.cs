using System;
using System.Linq;
using ArtSolution.Domain.Catalog;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    public class ProductTagService : ArtSolutionAppServiceBase, IProductTagService
    {

        #region Ctor && Field

        private readonly IRepository<ProductTag> _tagRepository;
        private readonly IRepository<ProductTagMapping> _mappedRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProductTagService(IRepository<ProductTag> tagRepository,
            IRepository<ProductTagMapping> mappedRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this._tagRepository = tagRepository;
            this._mappedRepository = mappedRepository;
            this._unitOfWorkManager = unitOfWorkManager;
        }


        #endregion

        #region ProductTag

        public void InsertTag(ProductTag tag)
        {
            if (tag == null)
                throw new Exception("tag");

            _tagRepository.Insert(tag);
        }

        public void UpdateTag(ProductTag tag)
        {
            if (tag == null)
                throw new Exception("tag");

            _tagRepository.Update(tag);
        }

        public void DeleteTag(int tagId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                _tagRepository.Delete(tagId);
                _mappedRepository.Delete(m => m.TagId == tagId);
                unitOfWork.Complete();
            }
        }

        public ProductTag GetTag(int productTagId)
        {
            if (productTagId <= 0)
                return null;
            return _tagRepository.Get(productTagId);
        }

        public IPagedResult<ProductTag> GetAllTags(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _tagRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(t => t.Name.Contains(keywords));

            query = query.OrderBy(t => t.DisplayOrder);
            return new PagedResult<ProductTag>(query, pageIndex, pageSize);
        }

        #endregion

        #region Product-ProductTag-Mapping

        public void InsertMapping(ProductTagMapping mapping)
        {
            _mappedRepository.Insert(mapping);
        }

        public void DeleteMappingById(int productMappingId)
        {
            _mappedRepository.Delete(productMappingId);
        }

        public void DeleteMappingByProductId(int productId)
        {
            _mappedRepository.Delete(m => m.ProductId == productId);
        }

        public void DeleteMappingByTagId(int tagId)
        {
            _mappedRepository.Delete(m => m.TagId == tagId);
        }

        public IList<ProductTagMapping> GetAllProductTagMappings(int productId = 0, int tagId = 0)
        {
            var query = _mappedRepository.GetAll();
            if (productId > 0)
                query = query.Where(m => m.ProductId == productId);
            if (tagId > 0)
                query = query.Where(m => m.TagId == tagId);

            query = query.OrderByDescending(m => m.Id);
            return query.ToList();
        }
        #endregion
    }
}
