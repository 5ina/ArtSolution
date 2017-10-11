using Abp.Application.Services;

namespace ArtSolution.Messages
{
    public interface ISMSMessageService : IApplicationService
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        bool SendMessage(string mobile, string signName, string tempCode, string param);
    }
}
