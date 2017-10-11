using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;

namespace ArtSolution.News
{
    /// <summary>
    /// 愿望单服务接口
    /// </summary>
    public interface IWishOrderService: IApplicationService
    {
        /// <summary>
        /// 新增愿望单
        /// </summary>
        /// <param name="order"></param>
        void InsertWishOrder(WishOrder order);

        /// <summary>
        /// 更新愿望单
        /// </summary>
        /// <param name="order"></param>
        void UpdateWishOrder(WishOrder order);
        /// <summary>
        /// 删除愿望单
        /// </summary>
        /// <param name="orderId"></param>
        void DeleteWishOrder(int orderId);

        /// <summary>
        /// 根据主键获取愿望单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        WishOrder GetWishOrderById(int orderId);

        /// <summary>
        /// 查询所有的愿望单
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<WishOrder> GetAllOrders(string keywords = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
