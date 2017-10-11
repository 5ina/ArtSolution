using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;

namespace ArtSolution.Authentication.Dto
{
    /// <summary>
    /// 用户登录反馈信息
    /// </summary>
    public class CustomerLoginResults : EntityDto
    {
        public CustomerLoginResults()
        {

        }
        public CustomerLoginResults(LoginResults result)
        {
            this.Result = result;
        }

        public LoginResults Result { get; set; }

        public Customer Customer { get; set; }
    }
}
