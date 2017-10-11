using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 用户地址服务
    /// </summary>
    public interface ICustomerAddressService: IApplicationService
    {
        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns>收货地址主键</returns>
        int InsertAddress(CustomerAddress address);

        /// <summary>
        /// 查询所有地址
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="customerId"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<CustomerAddress> GetAllAddress(string keywords = "",
            int customerId = 0,
            string province = "", string city = "", string country = "", 
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        CustomerAddress GetAddressById(int addressId);
    }
}
