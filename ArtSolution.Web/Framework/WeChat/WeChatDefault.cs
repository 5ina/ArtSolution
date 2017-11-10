using Abp.Runtime.Caching;
using ArtSolution.Common;
using ArtSolution.Names;
using ArtSolution.Web.Framework.WeChat.Models;
using ArtSolution.Web.Models.WeChat;
using Castle.Core.Logging;
using Newtonsoft.Json;


namespace ArtSolution.Web.Framework.WeChat
{
    public class WeChatDefault
    {        
        public string WeChatUrl = "https://api.weixin.qq.com/cgi-bin/";


        public AccessToken GetAccessToken(ICacheManager cacheManager, string appId, string appSecret)
        {
            //AccessToken token = cacheManager.GetCache(ArtSolution.ArtSolutionConsts.CACHE_ACCESS_TOKEN)
            //                        .Get(ArtSolutionConsts.CACHE_ACCESS_TOKEN, () =>
            //                        {
            //                            return HttpUtility.Get<AccessToken>(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));
            //                        });
            return HttpUtility.Get<AccessToken>(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));

            //return token;
        }

        public OAuthUserInfo GetWeChatUserInfo( ISettingService _settingService,string code)
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);
            
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    OAuthToken oauthToken = HttpUtility.Get<OAuthToken>(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appSecret, code));

                    string accesstoken = string.Empty;
                    AccessToken token = HttpUtility.Get<AccessToken>(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret));

                    if (token != null && !string.IsNullOrEmpty(token.access_token))
                    {
                        accesstoken = token.access_token;
                    }

                    if (oauthToken != null && !string.IsNullOrEmpty(oauthToken.openid))
                    {

                        OAuthUserInfo userInfo = HttpUtility.Get<OAuthUserInfo>(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accesstoken, oauthToken.openid));
                        return userInfo;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        public OAuthUserInfo GetWeChatUserInfo(ISettingService _settingService, ICacheManager cacheManager,string openId)
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            try
            {
                var Logger = Abp.Dependency.IocManager.Instance.Resolve<ILogger>();
                Logger.Debug("获取用户信息");
                
                var token = GetAccessToken(cacheManager, appId, appSecret);
                OAuthUserInfo userInfo = HttpUtility.Get<OAuthUserInfo>(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", token.access_token, openId), Logger);

                Logger.Debug(token.access_token + "|||" + userInfo.nickname + "|||" + userInfo.openid);
                return userInfo;
            }
            catch
            {
            }

            return null;
        }

        public WxConfigModel WxConfig(ISettingService _settingService,ICacheManager _cacheManager,string url)
        {
            var appId = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppId);
            var appSecret = _settingService.GetSettingByKey<string>(WeChatSettingNames.AppSecret);

            var token = GetAccessToken(_cacheManager,appId, appSecret);
            var tickent = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token.access_token);
        
            JSApiTicketModel apiTicker = HttpUtility.Get<JSApiTicketModel>(tickent);
            
            var nonceStr = CommonHelper.GenerateNonceStr();
            var timestamp = CommonHelper.GetTimeStamp();
            
            var urlPath = "jsapi_ticket=" + apiTicker.ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=http://" + url;
            var signature = CommonHelper.EncryptToSHA1(urlPath);
            
            var jsonData = new WxConfigModel
            {
                appId = appId,
                timestamp = timestamp,
                noncestr = nonceStr,
                signature = signature,
                ticket = apiTicker.ticket,
                value = signature,
                urlPath = urlPath
            };
            return jsonData;

        }

        public string OAuth(string appId, string redirect_uri)
        {
            var oatuth = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect", appId, System.Web.HttpUtility.UrlEncode(redirect_uri, System.Text.Encoding.UTF8), "wechat");
            return HttpUtility.Get(oatuth);
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="expire">有效时间</param>
        /// <param name="permanent">是否永久性二维码</param>
        /// <returns></returns>
        public string QRCode(ICacheManager cacheManager, string appId, string appSecret,int expire ,bool permanent,int scene)
        {
            var access_token = GetAccessToken(cacheManager, appId, appSecret);
            var QRCode_Url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            var url = string.Format(QRCode_Url, access_token.access_token);
            string data = string.Empty;
            if (permanent)
            {
                data = "{ \"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": { \"scene\": { \"scene_str\": \"" + scene + "\"} } }";
            }
            else
            {
                data = "{ \"expire_seconds\": " + expire + ", \"action_name\": \"QR_SCENE\", \"action_info\": { \"scene\": { \"scene_id\": " + scene + "} } }";
            }
            var result = HttpUtility.Post(url, data);
            var Logger = Abp.Dependency.IocManager.Instance.Resolve<ILogger>();
            Logger.Debug("返回的数据:" + result);
            var model = JsonConvert.DeserializeObject<QcScreenModel>(result);
            var ticket_url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
            return string.Format(ticket_url, model.ticket);
        }


        public bool SendMessage()
        {
            return true;
        }

    }
}