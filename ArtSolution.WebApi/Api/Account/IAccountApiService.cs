using ArtSolution.Api.Models.Account;
using ArtSolution.Authentication.Dto;
using System.Web.Http;

namespace ArtSolution.Api.Account
{
    public interface IAccountApiService :IApiService
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        ResultMessage<CustomerDto> Login([FromBody] LoginModel model);

        [HttpGet]
        string Connect();
    }
}
