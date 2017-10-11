using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Security;

namespace ArtSolution.Api.WeChat
{
    public class TokenApiService : WeChatApiController,  ITokenApiService
    {
        public HttpResponseMessage OAuth(string code)
        {
            var openUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

            string url = string.Format(openUrl, AppId, AppSecret,code);
            var response = new HttpWebResponseUtility().CreateGetHttpResponse(url);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            
            return new HttpResponseMessage
            {
                Content = new StringContent(retString, Encoding.GetEncoding("UTF-8"), "text/html")
            };
        }

        public HttpResponseMessage ValidToken(string echoStr, string signature, string timestamp, string nonce)
        {
            
            string[] ArrTmp = { Token, timestamp, nonce };

            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");


            if (tmpStr.ToLower() == signature.ToLower())
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(echoStr, Encoding.GetEncoding("UTF-8"), "text/html")
                };

            }
            return new HttpResponseMessage
            {
                Content = new StringContent("返回失败", Encoding.GetEncoding("UTF-8"), "text/html")
            };
        }
        
    }
}
