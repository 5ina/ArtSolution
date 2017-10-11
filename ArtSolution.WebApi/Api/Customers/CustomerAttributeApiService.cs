using Abp.WebApi.Controllers;
using ArtSolution.Customers;
using System.Linq;
using Abp.AutoMapper;
using System.Collections.Generic;
using ArtSolution.Api.Models.Customers;

namespace ArtSolution.Api.Customers
{
    /// <summary>
    /// 用户属性服务接口
    /// </summary>
    public class CustomerAttributeApiService : AbpApiController, ICustomerAttributeApiService
    {

        #region Ctor && Field

        private readonly ICustomerAttributeService _attributeService;

        public CustomerAttributeApiService(ICustomerAttributeService attributeService)
        {
            this._attributeService = attributeService;
        }

        #endregion

        #region Method
        public ResultMessage<CustomerAttributeModel> GetCustomerAttribute(int customerId, string key)
        {
            var attributes = _attributeService.GetAttributeByCustomerId(customerId);
            var attribute = attributes.FirstOrDefault(a => a.Key == key);
            return new ResultMessage<CustomerAttributeModel>(attribute.MapTo< CustomerAttributeModel>());
        }

        public ResultMessage<CustomerAttributeModel> GetCustomerAttributes(int customerId)
        {
            var attributes = _attributeService.GetAttributeByCustomerId(customerId);
            var models = attributes.MapTo<List<CustomerAttributeModel>>();
            return new ResultMessage<CustomerAttributeModel>(models);
        }

        #endregion
    }
}
