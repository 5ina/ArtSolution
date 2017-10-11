using Abp.WebApi.Controllers;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using ArtSolution.Models.Customers;
using ArtSolution.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolution.Api.Customers
{
    public class CustomerApiController : AbpApiController, IApiService
    {
        #region Ctor && Field

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;

        public CustomerApiController(ICustomerService customerService, IEncryptionService encryptionService)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
        }

        #endregion

        public int CreateCustomer(CreateCustomerModel model)
        {
            var code = CommonHelper.GenerateCode(6);
            var customer = new Customer
            {
                PasswordSalt = code,
                IsDeleted = true,
                Password = _encryptionService.CreatePasswordHash("123456", code),
                CustomerRoleId = model.CustomerRole,
                Mobile = model.Mobile,
            };
            _customerService.CreateCustomer(customer);
            return customer.Id;
        }

        public IList<Customer> GetAllCustomers()
        {
            return _customerService.GetAllCustomers().Items.ToList();
        }
    }
}
