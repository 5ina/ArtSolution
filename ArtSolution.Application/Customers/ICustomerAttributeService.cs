using Abp.Application.Services;
using ArtSolution.Domain.Customers;
using System.Collections.Generic;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 用户属性服务接口
    /// </summary>
    public interface ICustomerAttributeService : IApplicationService
    {
        /// <summary>
        /// 保存属性
        /// </summary>
        /// <param name="attribute"></param>
        void SaveAttribute(CustomerAttribute attribute);

        /// <summary>
        /// 获取用户的所有属性
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IList<CustomerAttribute> GetAttributeByCustomerId(int customerId);
        
        /// <summary>
        /// 删除熟悉
        /// </summary>
        /// <param name="attributeId"></param>
        void DeleteAttribute(int attributeId);

        /// <summary>
        /// 清空用户所有属性
        /// </summary>
        /// <param name="customerId"></param>
        void ClearAttribute(int customerId);
    }
}