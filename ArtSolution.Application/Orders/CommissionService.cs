using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ArtSolution.Orders
{
    public class CommissionService : ArtSolutionAppServiceBase, ICommissionService
    {
        #region Ctor && Field

        private readonly IRepository<Commission> _commissionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public CommissionService(IRepository<Commission> commissionRepository,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._commissionRepository = commissionRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }


        #endregion

        #region Method
        public IPagedResult<Commission> GetAllCommissions(int customerId = 0, int orderId = 0,
            DateTime? createdFrom =null, DateTime? createdTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _commissionRepository.GetAll();

            if (customerId > 0)
                query = query.Where(c => c.CustomerId == customerId);

            if (orderId > 0)
                query = query.Where(c => c.OrderId == orderId);
            
            if (createdFrom.HasValue)
                query = query.Where(c => createdFrom.Value <= c.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(c => createdTo.Value >= c.CreationTime);

            query = query.OrderByDescending(c => c.CreationTime);

            return new PagedResult<Commission>(query, pageIndex, pageSize);
        }

        public Commission GetCommissionById(int id)
        {
            if (id > 0)
                return _commissionRepository.Get(id);
            return null;
        }

        public void InsertCommission(Commission entity)
        {
            if (entity != null)
                _commissionRepository.Insert(entity);
        }

        public void UpdateCommission(Commission entity)
        {
            if (entity != null)
                _commissionRepository.Update(entity);
        }

        #endregion
    }
}
