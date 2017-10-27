using Abp.Domain.Repositories;
using ArtSolution.Domain.Catalog;
using System;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using System.Collections.Generic;

namespace ArtSolution.Catalog
{
    public class ComBoProductService : ArtSolutionAppServiceBase, IComBoProductService
    {
        #region Ctor && Field

        private readonly IRepository<ComBoProduct> _comboRepository;
        private readonly IRepository<ComBoProductMapping> _mappingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public ComBoProductService(IRepository<ComBoProduct> comboRepository, 
                        IRepository<ComBoProductMapping> mappingRepository,
                        IUnitOfWorkManager unitOfWorkManage)
        {
            this._comboRepository = comboRepository;
            this._mappingRepository = mappingRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }

        #endregion

        #region ComBoProduct


        public void DeleteComBoProduct(int comboId)
        {
            using (var unitOfWork = _unitOfWorkManage.Begin())
            {

                _comboRepository.Delete(comboId);
                DeleteMapping(comboId);

                unitOfWork.Complete();
            }
        }


        public ComBoProduct GetComBoProductById(int comboId)
        {
            return _comboRepository.Get(comboId);
        }

        public IPagedResult<ComBoProduct> GetComBoProducts(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _comboRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(c => c.Name.Contains(keywords));

            query = query.OrderByDescending(c => c.Id);

            return new PagedResult<ComBoProduct>(query, pageIndex, pageSize);
        }

        public void InsertComBoProduct(ComBoProduct combo)
        {
            if (combo != null)
                _comboRepository.Insert(combo);
        }        

        public void UpdateComBoProduct(ComBoProduct combo)
        {
            if (combo != null && combo.Id > 0)
                _comboRepository.Update(combo);
        }


        #endregion

        #region Combo Product Mapping
        public void InsertMapping(int comBoId, int productId)
        {
            if (comBoId > 0 && productId > 0)
                _mappingRepository.Insert(new ComBoProductMapping
                {
                    ComBoId = comBoId,
                    ProductId = productId
                });
        }

        public void InsertMapping(int comBoId, List<int> productIds)
        {
            if (comBoId > 0)
            {
                productIds.ForEach(p => _mappingRepository.Insert(new ComBoProductMapping
                {
                    ComBoId = comBoId,
                    ProductId = p
                }));
            }
        }


        public void DeleteMapping(int comBoId)
        {
            if (comBoId > 0)
                _mappingRepository.Delete(m => m.ComBoId == comBoId);
        }

        public void DeleteMapping(int comBoId, int productId)
        {
            if (comBoId > 0 && productId > 0)
                _mappingRepository.Delete(e => e.ComBoId == comBoId && e.ProductId == productId);

        }

        public List<int> GetComBoProductMappings(int comboId)
        {
             var query = _mappingRepository.GetAll();

            query = query.Where(m => m.ComBoId == comboId);

            return query.Select(m => m.ProductId).ToList();
        }
        #endregion
    }
}
