using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ArtSolution.Domain.Orders;
using System.Linq;
using Abp.Application.Services.Dto;

namespace ArtSolution.Orders
{
    public class FavoriteService: ArtSolutionAppServiceBase ,IFavoriteService
    {

        #region Ctor && Field

        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public FavoriteService(IRepository<Favorite> favoriteRepository,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._favoriteRepository = favoriteRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }


        #endregion

        #region Method

        public void DeleteAll(int customerId)
        {
            if (customerId > 0)
                _favoriteRepository.Delete(f => f.CustomerId == customerId);
        }

        public void DeleteFavorite(int favoriteId)
        {
            if (favoriteId > 0)
                    _favoriteRepository.Delete(favoriteId);
        }

        public IPagedResult<Favorite> GetAllFavorites(int customerId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (customerId <= 0)
                return null;

            var query = _favoriteRepository.GetAll();
            query = query.Where(f => f.CustomerId == customerId);

            query = query.OrderByDescending(f => f.CreationTime);
            return new PagedResult<Favorite>(query, pageIndex, pageSize);
        }

        public Favorite GetFavoriteById(int favoriteId)
        {
            if (favoriteId <= 0)
                return null;
            return _favoriteRepository.Get(favoriteId);
        }

        public void InsertFavorite(Favorite model)
        {
            if (model != null)
                _favoriteRepository.Insert(model);
        }

        public void UpdateFavorite(Favorite model)
        {
            if (model != null)
                _favoriteRepository.Update(model);
        }

        #endregion
    }
}
