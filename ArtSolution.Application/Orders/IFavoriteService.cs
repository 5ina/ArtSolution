using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 收藏夹服务
    /// </summary>
    public interface IFavoriteService : IApplicationService
    {
        /// <summary>
        /// 新增收藏夹
        /// </summary>
        /// <param name="model"></param>
        void InsertFavorite(Favorite model);
        /// <summary>
        /// 更新收藏夹
        /// </summary>
        /// <param name="model"></param>
        void UpdateFavorite(Favorite model);

        /// <summary>
        /// 删除收藏夹
        /// </summary>
        /// <param name="favoriteId"></param>
        void DeleteFavorite(int favoriteId);

        /// <summary>
        /// 清空收藏夹
        /// </summary>
        /// <param name="customerId"></param>
        void DeleteAll(int customerId);

        /// <summary>
        /// 获取收藏夹
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>
        Favorite GetFavoriteById(int favoriteId);

        /// <summary>
        /// 获取用户所有收藏夹
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Favorite> GetAllFavorites(int customerId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
