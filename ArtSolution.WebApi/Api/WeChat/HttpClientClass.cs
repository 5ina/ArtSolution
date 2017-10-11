using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ArtSolution.Api.WeChat
{
    public class HttpWebResponseUtility
    {
        private const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        ///创建GET方式的HTTP请求   
        public HttpWebResponse CreateGetHttpResponse(string url, int? timeout = null, CookieCollection cookies = null, string userAgent = DefaultUserAgent, Dictionary<string,string> paramas = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (paramas != null && paramas.Count > 0)
            {
                var result = paramas.Select(x =>
                {
                    return string.Format("{0}={1}", x.Key, x.Value);
                }).ToArray();
                
                string tmpStr = string.Join("&", result);
                if (result.Count() > 0)
                    url = string.Format("{0}?{1}", url, tmpStr);
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;            
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }



            return request.GetResponse() as HttpWebResponse;
        }
    }
}