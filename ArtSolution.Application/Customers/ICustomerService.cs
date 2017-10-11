using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Authentication.Dto;
using ArtSolution.Domain.Customers;
using System;
using System.Threading.Tasks;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface ICustomerService:IApplicationService
    {

        #region Customer
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="customer"></param>
        int CreateCustomer(Customer customer);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="customer"></param>
        void UpdateCustomer(Customer customer);

        Task UpdateAsyncCustomer(Customer customer);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customerId"></param>
        void DeleteCustomer(int customerId);

        /// <summary>
        /// 根据主键查询用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomerId(int customerId);

        Customer GetCustomerByOpenId(string openId);

        /// <summary>
        /// 该用户是否存在
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        bool HasCustomer(string openId, out Customer customer);

        /// <summary>
        /// 根据主键查询用户(异步)
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<Customer> GetCustomerIdAsync(int customerId);

        /// <summary>
        /// 根据手机号查询用户
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Customer GetCustomerByMobile(string mobile);
        Task<Customer> GetCustomerByMobileAsync(string mobile);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="keywords"></param>
        /// <param name="promoter">推广人Id</param>
        /// <param name="showHidden"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Customer> GetAllCustomers(
            DateTime? createdFrom = null, 
            DateTime? createdTo = null,
            bool? isSub = null,
            CustomerRole? roleId = null,
            string keywords = null,
            bool showHidden = false, int promoter = 0,
            bool? isPromoter =null,
            int pageIndex = 0, int pageSize = int.MaxValue);


        #endregion

        #region Validate


        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码（未加密的）</param>
        /// <param name="role">角色</param>
        /// <returns></returns>
        CustomerLoginResults ValidateCustomer(string loginName, string password, CustomerRole? role = null);
        Task<CustomerLoginResults> ValidateAsyncCustomer(string loginName, string password, CustomerRole? role = null);


        /// <summary>
        /// 打印注册用户保膘
        /// </summary>
        /// <param name="days">天数</param>
        /// <param name="isSubscrbe">是否关注</param>
        /// <returns></returns>
        int GetRegisteredCustomersReport(int days, bool isSubscrbe = true);
        #endregion
    }
}
