using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 退单服务接口
    /// </summary>
    public interface IReturnOrderService: IApplicationService
    {
        /// <summary>
        /// 新增退单申请
        /// </summary>
        /// <param name="model"></param>
        void InsertReturnOrder(ReturnOrder model);

        /// <summary>
        /// 更新退单申请
        /// </summary>
        /// <param name="model"></param>
        void UpdateReturnOrder(ReturnOrder model);

        /// <summary>
        /// 删除退单申请
        /// </summary>
        /// <param name="returnId"></param>
        void DeleteReturnOrder(int returnId);

        /// <summary>
        /// 根据主键获取退单信息
        /// </summary>
        /// <param name="returnId"></param>
        /// <returns></returns>
        ReturnOrder GetReturnOrder(int returnId);

        /// <summary>
        /// 获取所有退单申请
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ReturnOrder> GetAllReturnOrders(int? customerId = null, 
            int? orderId = null, string keywords = "",
            int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
