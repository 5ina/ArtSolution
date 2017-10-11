using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ArtSolution.Web.Framework
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

        /// <summary>
        /// 创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string CreatePostHttpResponse(string url, int? timeout = null, string contentType = "application/x-www-form-urlencoded", string data = null)
        {
            HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
            hwr.Method = "POST";
            hwr.ContentType = contentType;
            byte[] bytes;
            bytes = System.Text.Encoding.UTF8.GetBytes(data);//通过UTF-8编码  
            hwr.ContentLength = bytes.Length;
            Stream writer = hwr.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            var result = hwr.GetResponse() as HttpWebResponse; //此句是获得上面URl返回的数据  
            string strMsg = WebResponseGet(result);
            return strMsg;
        }

        public string WebResponseGet(HttpWebResponse webResponse)
        {
            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
    }
}