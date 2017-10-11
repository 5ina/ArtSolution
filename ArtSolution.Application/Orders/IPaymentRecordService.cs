using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 支付记录
    /// </summary>
    public interface IPaymentRecordService: IApplicationService
    {
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="record"></param>
        void InsertPaymentRecord(PaymentRecord record);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="record"></param>
        void UpdatePaymentRecord(PaymentRecord record);

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        PaymentRecord GetPaymentRecord(int recordId);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="recordId"></param>
        void DeletePaymentRecord(int recordId);

        /// <summary>
        /// 获取支付记录列表
        /// </summary>
        /// <returns></returns>
        IPagedResult<PaymentRecord> GetAllPaymentRecords(int customerId = 0, int orderId = 0, 
            int? audit = null, string keywords = "", 
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
