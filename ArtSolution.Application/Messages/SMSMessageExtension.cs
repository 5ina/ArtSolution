namespace ArtSolution.Messages
{
    public static class SMSMessageExtension
    {
        /// <summary>
        /// 手机验证码
        /// </summary>
        /// <param name="_messageService"></param>
        /// <param name="content"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool SendMobileCode(this ISMSMessageService _messageService, string content, string mobile)
        {
            return _messageService.SendMessage(mobile, "贴身保膘", "SMS_75885084", "{\"code\":\"" + content + "\"}");
        }
    }
}
