using Abp.Web.Models;
using System.Net.Http;
using System.Web.Http;

namespace ArtSolution.Api.WeChat
{
    public interface ITokenApiService:IApiService
    {
        [DontWrapResult]
        [HttpGet]
        HttpResponseMessage ValidToken(string echoStr, string signature, string timestamp, string nonce);

        [DontWrapResult]
        [HttpGet]
        HttpResponseMessage OAuth(string code);
    }
}
