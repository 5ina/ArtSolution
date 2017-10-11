using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;
using Abp.Domain.Repositories;

namespace ArtSolution.Customers
{
    public class ApplyCashService : ArtSolutionAppServiceBase, IApplyCashService
    {
        #region Ctor && Field
        
        private readonly IRepository<ApplyCash> _cashRepository;
        public ApplyCashService(IRepository<ApplyCash> cashRepository)
        {
            this._cashRepository = cashRepository;
        }

        #endregion
        #region Method
        public void DeleteApplyCash(int entityId)
        {
            _cashRepository.Delete(entityId);
        }

        public IPagedResult<ApplyCash> GetAllApplyCashs(int customerId = 0,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            int? audit = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _cashRepository.GetAll();

            if (createdFrom.HasValue)
                query = query.Where(a => createdFrom.Value <= a.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(a => createdTo.Value >= a.CreationTime);

            if (customerId > 0)
                query = query.Where(a => a.CustomerId == customerId);

            if (audit.HasValue)
                query = query.Where(a => a.Audit == audit.Value);
            query = query.OrderByDescending(a => a.CreationTime);
            return new PagedResult<ApplyCash>(query, pageIndex, pageSize);
        }

        public ApplyCash GetApplyCashById(int entityId)
        {
            return _cashRepository.Get(entityId);
        }

        public void InsertApplyCash(ApplyCash entity)
        {
            _cashRepository.Insert(entity);
        }

        public void UpdateApplyCash(ApplyCash entity)
        {
            _cashRepository.Update(entity);
        }
        #endregion
    }
}
