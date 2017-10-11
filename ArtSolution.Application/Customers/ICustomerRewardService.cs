using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 用户积分历史记录服务
    /// </summary>
    public interface ICustomerRewardService: IApplicationService
    {
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="history"></param>
        void InsertRewardHistory(CustomerReward history);

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="history"></param>
        void UpdateRewardHistory(CustomerReward history);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="historyId"></param>
        void DeleteRewardHistoryById(int historyId);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="customerId"></param>
        void DeleteRewardHistoryByCustomerId(int customerId);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="historyId"></param>
        /// <returns></returns>
        CustomerReward GetCustomerRewardHistoryById(int historyId);
        
        /// <summary>
        /// 获取所有积分历史
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<CustomerReward> GetAllCustomerRewards(int customer, string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
