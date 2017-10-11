using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;
using Abp.Domain.Repositories;

namespace ArtSolution.Customers
{
    public class ApplyPromoterService : ArtSolutionAppServiceBase, IApplyPromoterService
    {
        #region Ctor && Field

        private readonly IRepository<ApplyPromoter> _applyRepository;

        public ApplyPromoterService(IRepository<ApplyPromoter> applyRepository)
        {
            this._applyRepository = applyRepository;
        }

        #endregion
        #region Method
        public IPagedResult<ApplyPromoter> GetAllList(string keywords = "",
            DateTime? createdFrom = null, DateTime? createdTo = null,
            bool? audit =null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _applyRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(a => a.Mobile.Contains(keywords) ||
                                        a.NickName.Contains(keywords) ||
                                        a.AuditReason.Contains(keywords));
            
            if (createdFrom.HasValue)
                query = query.Where(a => createdFrom.Value <= a.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(a => createdTo.Value >= a.CreationTime);

            if (audit.HasValue)
                query = query.Where(a => a.Audit == audit.Value);

            query = query.OrderByDescending(a => a.CreationTime);
            return new PagedResult<ApplyPromoter>(query, pageIndex, pageSize);
        }

        public ApplyPromoter GetPromoterById(int id)
        {
            return _applyRepository.Get(id);
        }

        public void Insert(ApplyPromoter model)
        {
            if (model == null || model.Id > 0)
                return;
            _applyRepository.Insert(model);
        }

        public void Update(ApplyPromoter model)
        {
            if (model == null || model.Id <= 0)
                return;
            _applyRepository.Update(model);
        }
        #endregion
    }
}
