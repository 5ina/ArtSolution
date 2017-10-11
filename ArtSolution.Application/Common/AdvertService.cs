using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Common;
using Abp.Domain.Repositories;

namespace ArtSolution.Common
{
    /// <summary>
    /// 首页广告
    /// </summary>
    public class AdvertService : ArtSolutionAppServiceBase, IAdvertService
    {

        #region Ctor && Field

        private readonly IRepository<Advert> _advertRepository;
        public AdvertService(IRepository<Advert> advertRepository)
        {
            this._advertRepository = advertRepository;
        }

        #endregion
        #region Method
        public void DeleteAdvert(int Id)
        {
            if (Id > 0)
                _advertRepository.Delete(Id);
        }

        public Advert GetAdvertById(int advertId)
        {
            if (advertId <= 0)
                return null;
            return _advertRepository.Get(advertId);
        }

        public IPagedResult<Advert> GetAllAdverts(string keywords = null,
            DateTime? showFrom = null,
            DateTime? showTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _advertRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(a => a.Name.Contains(keywords));


            if (showFrom.HasValue)
                query = query.Where(c => showFrom.Value >= c.StartTime && showFrom.Value <= c.EndTime);
            if (showTo.HasValue)
                query = query.Where(c => showTo.Value >= c.EndTime && showTo.Value <= c.StartTime);

            query = query.OrderByDescending(a => a.DisplayOrder);
            return new PagedResult<Advert>(query, pageIndex, pageSize);
        }

        public void InsertAdvert(Advert model)
        {
            if (model != null)
                _advertRepository.Insert(model);
        }

        public void UpdateAdvert(Advert model)
        {
            if (model != null)
                _advertRepository.Update(model);
        }
        #endregion
    }
}
