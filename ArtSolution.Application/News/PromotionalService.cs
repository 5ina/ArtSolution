using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;
using Abp.Runtime.Caching;
using Abp.Domain.Repositories;

namespace ArtSolution.News
{
    public class PromotionalService : ArtSolutionAppServiceBase, IPromotionalService
    {
        #region Ctor && Field

        private const string PROMOTIONAL_CACHE_BY_ID = "store.promotional.by.id.{0}";

        private readonly IRepository<Promotional> _promotionalRepository;
        private readonly ICacheManager _cacheManager;
        public PromotionalService(IRepository<Promotional> promotionalRepository, ICacheManager cacheManager)
        {
            this._promotionalRepository = promotionalRepository;
            this._cacheManager = cacheManager;
        }

        #endregion


        #region Method
        public void DeletePromotional(int promotionalId)
        {
            var key = string.Format(PROMOTIONAL_CACHE_BY_ID, promotionalId);
            _cacheManager.GetCache(key).Remove(promotionalId.ToString());
            _promotionalRepository.Delete(promotionalId);
        }

        public IPagedResult<Promotional> GetAllPromotionals(string keywords = "", bool showHidden = false,
            DateTime? createdFrom = null, DateTime? createdTo = null, 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _promotionalRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(p => p.Name.Contains(keywords) 
                                        || p.Title.Contains(keywords) 
                                        || p.Description.Contains(keywords) 
                                        || p.Keywords.Contains(keywords));

            if (!showHidden)
                query = query.Where(c => c.Published && c.EndDate > DateTime.Now);

            if (createdFrom.HasValue)
                query = query.Where(c => createdFrom.Value <= c.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(c => createdTo.Value >= c.CreationTime);

            query = query.OrderByDescending(c => c.CreationTime);
            return new PagedResult<Promotional>(query, pageIndex, pageSize);
        }

        public Promotional GetPromotionalById(int promotionalId)
        {
            var key = string.Format(PROMOTIONAL_CACHE_BY_ID, promotionalId);
            return _cacheManager.GetCache(key).Get(promotionalId.ToString(), () => {
                return _promotionalRepository.Get(promotionalId);
            });
        }

        public void InsertPromotional(Promotional model)
        {
            if (model != null)
                _promotionalRepository.Insert(model);
        }

        public void UpdatePromotional(Promotional model)
        {
            var key = string.Format(PROMOTIONAL_CACHE_BY_ID, model.Id);
            _cacheManager.GetCache(key).Remove(model.Id.ToString());
            _promotionalRepository.Update(model);
        }
        #endregion
    }
}
