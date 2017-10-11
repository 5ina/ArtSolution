using ArtSolution.Api.Models.Customers;
using System.Web.Http;

namespace ArtSolution.Api.Customers
{
    /// <summary>
    /// 用户属性服务接口
    /// </summary>
    public interface ICustomerAttributeApiService :IApiService
    {
        /// <summary>
        /// 获取用户的所有属性
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<CustomerAttributeModel> GetCustomerAttributes(int customerId);

        /// <summary>
        /// 获取用户属性
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        ResultMessage<CustomerAttributeModel> GetCustomerAttribute(int customerId, string key);
    }
}
