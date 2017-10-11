using Abp.AutoMapper;
using ArtSolution.Authentication.Dto;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace ArtSolution.Authentication
{

    public class CustomerStore : IUserStore<CustomerDto, int>
    {
        public CustomerStore(ICustomerService customerService)
        {
            this._customerService = customerService;
        }
        private readonly ICustomerService _customerService;

        public Task CreateAsync(CustomerDto user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CustomerDto user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerDto> FindByIdAsync(int customerId)
        {
            var customer = await _customerService.GetCustomerIdAsync(customerId);
            return new CustomerDto
            {
                Id = customer.Id,
                CustomerRoleId = customer.CustomerRoleId,
                Mobile = customer.Mobile,
                Password = customer.Password,
                PasswordSalt = customer.PasswordSalt,
                UserName = customer.Mobile,
                CreationTime = customer.CreationTime,
                LastModificationTime = customer.LastModificationTime,
                VerificationCode = customer.VerificationCode,
                OpenId = customer.OpenId,
                Promoter = customer.Promoter,
                NickName = customer.NickName,
                IsSubscribe =customer.IsSubscribe,
                CustomerRole = (CustomerRole)customer.CustomerRoleId,
            };
        }

        public async Task<CustomerDto> FindByNameAsync(string loginName)
        {
            var customer = await _customerService.GetCustomerByMobileAsync(loginName);
            return customer.MapTo<CustomerDto>();
        }

        public async Task UpdateAsync(CustomerDto user)
        {
            var customer = user.MapTo<Customer>();

            await _customerService.UpdateAsyncCustomer(customer);
        }
    }
}
