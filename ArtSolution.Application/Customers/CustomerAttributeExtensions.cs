using ArtSolution.Domain.Customers;
using System;
using System.Linq;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 用户属性扩展类
    /// </summary>
    public static class CustomerAttributeExtensions
    {
        /// <summary>
        /// 获取用户的属性值
        /// </summary>
        /// <typeparam name="TPropType"></typeparam>
        /// <param name="customer"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TPropType GetCustomerAttributeValue<TPropType>(this Customer customer, string key)
        {
            var _customerAttributeService = Abp.Dependency.IocManager.Instance.Resolve<ICustomerAttributeService>();

            return GetCustomerAttributeValue<TPropType>(customer, key, _customerAttributeService);
        }

        /// <summary>
        /// 获取用户的属性值
        /// </summary>
        /// <typeparam name="TPropType"></typeparam>
        /// <param name="customer"></param>
        /// <param name="key"></param>
        /// <param name="_customerAttributeService"></param>
        /// <returns></returns>
        public static TPropType GetCustomerAttributeValue<TPropType>(this Customer customer, string key,
            ICustomerAttributeService _customerAttributeService)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var props = _customerAttributeService.GetAttributeByCustomerId(customer.Id);

            if (props == null)
                return default(TPropType);
            if (props.Count == 0)
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(prop.Value);
        }

        public static void SaveCustomerAttribute<TPropType>(this Customer customer, string key, TPropType value)
        {
            var _customerAttributeService = Abp.Dependency.IocManager.Instance.Resolve<ICustomerAttributeService>();

            SaveCustomerAttribute<TPropType>(customer, key, value, _customerAttributeService);
        }

        public static void SaveCustomerAttribute<TPropType>(this Customer customer, string key, TPropType value,
            ICustomerAttributeService _customerAttributeService)
        {
            if (customer == null|| customer.Id ==0)
                throw new ArgumentNullException("customer");

            var props = _customerAttributeService.GetAttributeByCustomerId(customer.Id);
            var attribute = new CustomerAttribute();
            if (props != null
                && props.Count > 0
                && props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) != null)
            {
                attribute = props.FirstOrDefault(ga =>
                 ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                attribute.Value = value.ToString();
            }
            else {
                attribute.CustomerId = customer.Id;
                attribute.Key = key;
                attribute.Value = value.ToString();            
            }
            _customerAttributeService.SaveAttribute(attribute);
        }
    }
}
