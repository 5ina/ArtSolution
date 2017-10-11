using Abp.Application.Services;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 签到服务接口
    /// </summary>
    public interface ISignLogService: IApplicationService
    {
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="customerId"></param>
        void SignIn(int customerId);
        
        /// <summary>
        /// 今天是否签到(true 签到）
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool IsSign(int customerId);

        /// <summary>
        /// 清空签到信息
        /// </summary>
        /// <param name="customerId"></param>
        void Clear(int customerId = 0);
    }
}
