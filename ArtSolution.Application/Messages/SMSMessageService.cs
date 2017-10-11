using ArtSolution.Common;
using ArtSolution.Names;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace ArtSolution.Messages
{
    public class SMSMessageService : ArtSolutionAppServiceBase, ISMSMessageService
    {

        #region Ctor && Field
        private const string SMSMESSAGENAME = "store.settings.sms";
        
        private readonly ISettingService _settingService;
        
        private string accessKeyId;
        private string accessKeySecret;


        private readonly IClientProfile _clientProfile;

        public SMSMessageService(ISettingService settingService)
        {
            this._settingService = settingService;
            this.accessKeyId = settingService.GetSettingByKey<string>(MediaSettingNames.AccessKeyId);
            this.accessKeySecret = settingService.GetSettingByKey<string>(MediaSettingNames.AccessKeySecret);            

            _clientProfile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", "Dysmsapi", "dysmsapi.aliyuncs.com");
        }
                
        #endregion

        #region Method
        public bool SendMessage(string mobile, string signName,string tempCode , string param)
        {
            IAcsClient acsClient = new DefaultAcsClient(_clientProfile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = mobile;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = signName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = tempCode;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = param;
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);

                System.Console.WriteLine(sendSmsResponse.Message);
                return true;

            }
            catch (ServerException e)
            {
                System.Console.WriteLine("Hello World!");
                return false;
            }
            catch (ClientException e)
            {
                System.Console.WriteLine("Hello World!");
                return false;
            }
        }
        #endregion
        
    }
}
