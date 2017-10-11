using Abp.Runtime.Caching;
using Abp.WebApi.Controllers;
using System.IO;
using System.Text;

namespace ArtSolution.Api.WeChat
{
    public class WeChatApiController : AbpApiController
    {
        protected const string Token = "weixin";
        protected const string AppId = "wx481e6678625ee05a";
        protected const string AppSecret = "62b99c6728d80821d5b91c7cbb527766";
        protected const string WeChatAccess_Token_Url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";


        private readonly ICacheManager _cacheManager;
        public WeChatApiController()
        {
            this._cacheManager = Abp.Dependency.IocManager.Instance.Resolve<ICacheManager>();
        }

        public string AccessToken {
            get {
                var value = _cacheManager.GetCache(ArtSolutionConsts.CACHE_TOKEN).Get("", () => GetToken());
                if (value == "")
                    value = GetToken();
                _cacheManager.GetCache(ArtSolutionConsts.CACHE_TOKEN).Set("", value);
                return value;
            }
        }
        
        public string GetToken()
        {
            string url = string.Format(WeChatAccess_Token_Url, AppId, AppSecret);
            var response = new HttpWebResponseUtility().CreateGetHttpResponse(url);

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;

        }
    }
}
