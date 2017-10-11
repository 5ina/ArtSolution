using Abp.WebApi.Controllers;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using ArtSolution.Security;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolution.Api.Customers
{
    public class CustomerApiService :AbpApiController, ICustomerApiService
    {

        #region Ctor && Field

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;

        public CustomerApiService(ICustomerService customerService, IEncryptionService encryptionService)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
        }

        #endregion

        #region Method

        
        public IList<Customer> GetAllCustomers()
        {
            return _customerService.GetAllCustomers().Items.ToList();
        }
        #endregion
    }
}
