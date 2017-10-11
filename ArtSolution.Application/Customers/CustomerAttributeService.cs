using System.Collections.Generic;
using ArtSolution.Domain.Customers;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;

namespace ArtSolution.Customers
{
    public class CustomerAttributeService : ArtSolutionAppServiceBase,ICustomerAttributeService
    {

        #region Ctor && Field
        /// <summary>
        /// 用户属性{0} 用户主键
        /// </summary>
        private const string CUSTOMER_ATTRIBUTES_BY_ID = "store.customer.attributes.byId{0}";

        private readonly IRepository<CustomerAttribute> _attributeRepository;
        private readonly ICacheManager _cacheManager;

        public CustomerAttributeService(IRepository<CustomerAttribute> attributeRepository,
            ICacheManager cacheManager)
        {
            this._attributeRepository = attributeRepository;
            this._cacheManager = cacheManager;
        }


        #endregion

        #region Method
        public void ClearAttribute(int customerId)
        {
            _attributeRepository.Delete(x => x.CustomerId == customerId);
            var key = string.Format(CUSTOMER_ATTRIBUTES_BY_ID, customerId);
            _cacheManager.GetCache(key).Remove(customerId.ToString());
        }

        public void DeleteAttribute(int attributeId)
        {
            if (attributeId > 0)
                _attributeRepository.Delete(attributeId);
        }

        public IList<CustomerAttribute> GetAttributeByCustomerId(int customerId)
        {
            var key = string.Format(CUSTOMER_ATTRIBUTES_BY_ID, customerId);
            return _cacheManager.GetCache(key).Get(customerId.ToString(), () =>
            {
                return _attributeRepository.GetAllList(x => x.CustomerId == customerId);
            });            
        }

        public void SaveAttribute(CustomerAttribute attribute)
        {
            if (attribute == null || attribute.CustomerId <= 0)
                return;

            if (attribute.Id == 0)
                _attributeRepository.Insert(attribute);
            else
                _attributeRepository.Update(attribute);
            var key = string.Format(CUSTOMER_ATTRIBUTES_BY_ID, attribute.CustomerId);
            _cacheManager.GetCache(key).Remove(attribute.CustomerId.ToString());
            
        }
        #endregion
    }
}
