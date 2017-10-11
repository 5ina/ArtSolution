using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Orders
{
    public interface IDeliveryService : IApplicationService
    {
        /// <summary>
        /// 新增配货方式
        /// </summary>
        /// <param name="delivery"></param>
        void InsertDelivery(Delivery delivery);

        /// <summary>
        /// 更新配货方式
        /// </summary>
        /// <param name="delivery"></param>
        void UpdateDelivery(Delivery delivery);

        /// <summary>
        /// 根据主键获取配送方式
        /// </summary>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        Delivery GetDelivery(int deliveryId);

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="deliveryId"></param>
        void DeleteDelivery(int deliveryId);
        /// <summary>
        /// 获取所有配送方式
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="showHidden"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Delivery> GetAllDeliveries(string keywords = "", bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
