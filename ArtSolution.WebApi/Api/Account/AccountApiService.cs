using Abp.WebApi.Controllers;
using ArtSolution.Api.Models.Account;
using ArtSolution.Authentication.Dto;
using ArtSolution.Customers;
using ArtSolution.Security;
using Abp.AutoMapper;

namespace ArtSolution.Api.Account
{
    public class AccountApiService : AbpApiController, IAccountApiService
    {
        #region Ctor && Field

        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        
        public AccountApiService(ICustomerService customerService, IEncryptionService encryptionService)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
        }

        #endregion

        #region Method
        public ResultMessage<CustomerDto> Login(LoginModel model)
        {

            var loginResult = _customerService.ValidateCustomer(model.Mobile, "");
            switch (loginResult.Result)
            {
                case LoginResults.Successful:
                    {
                        return new ResultMessage<CustomerDto>(loginResult.Customer.MapTo<CustomerDto>());
                    }

                case LoginResults.Deleted:
                    return new ResultMessage<CustomerDto>("用户已删除");
                case LoginResults.NotRegistered:
                    return new ResultMessage<CustomerDto>("用户未注册");
                case LoginResults.WrongPassword:
                default:
                    return new ResultMessage<CustomerDto>("密码错误");
            }

        }
        public string Connect()
        {
            return "true";
        }

        #endregion
    }
}
