using Abp.Domain.Repositories;
using ArtSolution.Domain.Customers;
using System;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Threading.Tasks;
using ArtSolution.Authentication.Dto;
using ArtSolution.Security;
using ArtSolution.Messages;
using Abp.Notifications;
using Abp.Runtime.Caching;

namespace ArtSolution.Customers
{
    public class CustomerService : ArtSolutionAppServiceBase, ICustomerService
    {
        #region Ctor && Field

        /// <summary>
        /// 用户缓存key，{0}为用户ID
        /// </summary>
        private const string CUSTOMER_BY_ID = "art.customer.by-id.{0}";
        private const string CUSTOMER_BY_OPENID = "art.customer.by-openid.{0}";


        private readonly IRepository<Customer> _customerRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEncryptionService _encryptionService;
        private readonly INoticeService _noticeService;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IRepository<CustomerAttribute> _attributeRepository;
        public CustomerService(
            IRepository<Customer> customerRepository,
            ICacheManager cacheManager,
            IEncryptionService encryptionService,
            INoticeService noticeService,
            IRepository<CustomerAttribute> attributeRepository,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            this._customerRepository = customerRepository;
            this._cacheManager = cacheManager;
            this._encryptionService = encryptionService;
            this._noticeService = noticeService;
            this._attributeRepository = attributeRepository;
            this._notificationSubscriptionManager = notificationSubscriptionManager;
        }

        #endregion

        #region Method        
        public int CreateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            return _customerRepository.InsertAndGetId(customer);

        }

        public void DeleteCustomer(int customerId)
        {
            _customerRepository.Delete(customerId);
        }

        public IPagedResult<Customer> GetAllCustomers(DateTime? createdFrom = null,
            DateTime? createdTo = null,
            bool? isSub = null,
            CustomerRole? roleId = null,
            string keywords = null,
            bool showHidden = false, int promoter = 0,
            bool? isPromoter = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.GetAll();
                        
            if (createdFrom.HasValue)
                query = query.Where(c => createdFrom.Value <= c.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(c => createdTo.Value >= c.CreationTime);

            if (promoter > 0)
                query = query.Where(c => c.Promoter == promoter);

            if (isSub.HasValue)
                query = query.Where(c => c.IsSubscribe == isSub.Value);

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(c => c.NickName.Contains(keywords) || c.Mobile.Contains(keywords));

            if (roleId.HasValue)
                query = query.Where(c => c.CustomerRoleId == (int)roleId.Value);

            if (isPromoter.HasValue)
                query = query.Join(_attributeRepository.GetAll(), x => x.Id, y => y.CustomerId, (x, y) => new { Customer = x, CustomerAttribute = y })
                   .Where((z => z.CustomerAttribute.Key == CustomerAttributeNames.IsPromoter &&
                   z.CustomerAttribute.Value == Boolean.TrueString)).Select(z => z.Customer);

            query = query.OrderByDescending(c => c.CreationTime);

            return new PagedResult<Customer>(query, pageIndex, pageSize);
        }

        public Customer GetCustomerByMobile(string mobile)
        {
            return _customerRepository.GetAllList(x => x.Mobile == mobile).FirstOrDefault();
        }

        public async Task<Customer> GetCustomerByMobileAsync(string mobile)
        {
            return await _customerRepository.FirstOrDefaultAsync(c => c.Mobile == mobile);
        }
        public Customer GetCustomerId(int customerId)
        {
            if (customerId > 0)
            {
                var key = string.Format(CUSTOMER_BY_ID, customerId);
                return _cacheManager.GetCache(key).Get(customerId.ToString(), () =>
                {
                    return _customerRepository.Get(customerId);
                });
            }
            else
                return null;
        }

        public async Task<Customer> GetCustomerIdAsync(int customerId)
        {
            var key = string.Format(CUSTOMER_BY_ID, customerId);
            return await _cacheManager.GetCache(key).GetAsync(customerId.ToString(), () =>
            {
                return _customerRepository.GetAsync(customerId);
            });
        }


        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);
            if (customer.Id > 0)
                _cacheManager.GetCache(string.Format(CUSTOMER_BY_ID, customer.Id)).Remove(customer.Id.ToString());
            if (!String.IsNullOrWhiteSpace(customer.OpenId))
                _cacheManager.GetCache(string.Format(CUSTOMER_BY_OPENID, customer.OpenId)).Remove(customer.OpenId);
        }

        public async Task UpdateAsyncCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            await _customerRepository.UpdateAsync(customer);
            await _cacheManager.GetCache(string.Format(CUSTOMER_BY_ID, customer.Id)).RemoveAsync(customer.Id.ToString());
            await _cacheManager.GetCache(string.Format(CUSTOMER_BY_OPENID, customer.OpenId)).RemoveAsync(customer.OpenId);
        }

        public CustomerLoginResults ValidateCustomer(string loginName, string password, CustomerRole? role = null)
        {
            var result = new CustomerLoginResults();
            var customer = GetCustomerByMobile(loginName);
            if (customer == null)
                return new CustomerLoginResults(LoginResults.NotRegistered);
            var psd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt);
            bool isValid = psd == customer.Password;
            if (!isValid)
                result.Result = LoginResults.WrongPassword;
            if (role.HasValue)
            {
                if (role.Value != (CustomerRole)customer.CustomerRoleId)
                    result.Result = LoginResults.Unauthorized;
            }

            customer.LastModificationTime = DateTime.Now;
            //_customerService.UpdateCustomer(customer);
            result.Result = LoginResults.Successful;
            result.Customer = customer;
            return result;
        }

        public async Task<CustomerLoginResults> ValidateAsyncCustomer(string loginName, string password, CustomerRole? role = null)
        {
            var result = new CustomerLoginResults();
            var customer = await GetCustomerByMobileAsync(loginName);
            if (customer == null)
                return new CustomerLoginResults(LoginResults.NotRegistered);
            var psd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt);
            bool isValid = psd == customer.Password;
            if (!isValid)
                result.Result = LoginResults.WrongPassword;
            if (role.HasValue)
            {
                if (role.Value != (CustomerRole)customer.CustomerRoleId)
                    result.Result = LoginResults.Unauthorized;
            }

            customer.LastModificationTime = DateTime.Now;
            //_customerService.UpdateCustomer(customer);
            result.Result = LoginResults.Successful;
            result.Customer = customer;
            return result;
        }

        public Customer GetCustomerByOpenId(string openId)
        {
            return _customerRepository.FirstOrDefault(c => c.OpenId == openId);
            //var key = string.Format(CUSTOMER_BY_OPENID, openId);
            //return _cacheManager.GetCache(key).Get(openId, () =>
            //{
            //var customer = _customerRepository.GetAllList(x => x.OpenId == openId).FirstOrDefault();
            //if (customer == null)
            //{
            //    customer = new Customer();
            //    var code = CommonHelper.GenerateCode(6);
            //    customer.PasswordSalt = code;
            //    customer = new Customer
            //    {
            //        CustomerRole = CustomerRole.Buyer,
            //        OpenId = openId,
            //        PasswordSalt = code,
            //        Password = _encryptionService.CreatePasswordHash("123456", code)
            //    };

            //    customer.Id = _customerRepository.InsertAndGetId(customer);
            //}

            //return customer;
            //});
        }

        public bool HasCustomer(string openId ,out Customer customer)
        {
            customer = _customerRepository.FirstOrDefault(e => e.OpenId == openId);
            if (customer == null)
                return false;
            if (customer.Id == 0)
                return false;
            return true;
        }

        public int GetRegisteredCustomersReport(int days, bool isSubscrbe = true)
        {
            DateTime date = DateTime.Now.AddDays(-days);

            var query = from c in _customerRepository.GetAll()
                        where
                        c.Id == (int)CustomerRole.Buyer &&
                        c.CreationTime >= date &&
                        (isSubscrbe && c.IsSubscribe)
                        //&& c.CreatedOnUtc <= DateTime.UtcNow
                        select c;
            int count = query.Count();
            return count;
        }

        #endregion
    }
}
