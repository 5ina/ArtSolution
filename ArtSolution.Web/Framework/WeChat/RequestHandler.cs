using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace ArtSolution.Web.Framework.WeChat
{
    /**
   '签名工具类
    ============================================================================/// <summary>
   'api说明：
   'Init();
   '初始化函数，默认给一些参数赋值。
   'SetKey(key_)'设置商户密钥
   'CreateMd5Sign(signParams);字典生成Md5签名
   'GenPackage(packageParams);获取package包
   'CreateSHA1Sign(signParams);创建签名SHA1
   'ParseXML();输出xml
   'GetDebugInfo(),获取debug信息
    * 
    * ============================================================================
    */
    public class RequestHandler
    {
        public RequestHandler(HttpContext httpContext)
        {
            Parameters = new Hashtable();

            this.HttpContext = httpContext ?? HttpContext.Current;

        }
        /// <summary>
        /// 密钥
        /// </summary>
        private string Key;

        protected HttpContext HttpContext;

        /// <summary>
        /// 请求的参数
        /// </summary>
        protected Hashtable Parameters;

        /// <summary>
        /// debug信息
        /// </summary>
        private string DebugInfo;

        /// <summary>
        /// 初始化函数
        /// </summary>
        public virtual void Init()
        {
        }
        /// <summary>
        /// 获取debug信息
        /// </summary>
        /// <returns></returns>
        public String GetDebugInfo()
        {
            return DebugInfo;
        }
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            return Key;
        }
        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (Parameters.Contains(parameter))
                {
                    Parameters.Remove(parameter);
                }

                Parameters.Add(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        public string GetParameter(string key)
        {
            if (key != null && key != "")
            {
                if (Parameters.Contains(key))
                {
                    return Parameters[key].ToString();
                }
            }
            return "";
        }


        /// <summary>
        /// 获取package带参数的签名包
        /// </summary>
        /// <returns></returns>
        public string GetRequestURL()
        {
            this.CreateSign();
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();
            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            //去掉最后一个&
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }


            return sb.ToString();

        }

        /// <summary>
        /// 创建md5摘要,规则是:按参数名称a-z排序,遇到空值的参数不参加签名
        /// </summary>
        protected virtual void CreateSign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.GetKey());
            string sign = CommonHelper.GetMD5(sb.ToString()).ToUpper();

            this.SetParameter("sign", sign);

            //debug信息
            this.SetDebugInfo(sb.ToString() + " => sign:" + sign);
        }


        /// <summary>
        /// 创建package签名
        /// </summary>
        /// <returns></returns>
        public virtual string CreateMd5Sign(string key, bool addSign = true)
        {
            StringBuilder sb = new StringBuilder();
            ArrayList akeys = new ArrayList(Parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)Parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "".CompareTo(v) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }
            sb.Append("key=" + key);
            string sign = CommonHelper.GetMD5(sb.ToString()).ToUpper();
            if (addSign)
                this.Parameters.Add("sign", sign);
            return sign;
        }



        /// <summary>
        /// 输出XML
        /// </summary>
        /// <returns></returns>
        public string ParseXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");

            ArrayList list = new ArrayList(Parameters.Keys);
            list.Sort();

            foreach (string k in list)
            {
                string v = (string)Parameters[k];
                //if (Regex.IsMatch(v, @"^[0-9.]$"))
                //{

                //    sb.Append("<" + k + ">" + v + "</" + k + ">");
                //}
                //else
                //{
                //    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                //}
                sb.Append("<" + k + ">" + v + "</" + k + ">");

            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <returns></returns>
        public string ToPrintStr()
        {
            string str = "";
            foreach (string k in Parameters.Keys)
            {
                string v = (string)Parameters[k];
                str += string.Format("{0}={1}<br>", k, v);
            }
            return str;
        }


        /// <summary>
        /// 设置debug信息
        /// </summary>
        /// <param name="debugInfo"></param>
        public void SetDebugInfo(String debugInfo)
        {
            this.DebugInfo = debugInfo;
        }

        public Hashtable GetAllParameters()
        {
            return this.Parameters;
        }


        /**
        * @将xml转为WxPayData对象并返回对象内部的数据
        * @param string 待转换的xml串
        * @return 经转换得到的Dictionary
        * @throws WxPayException
        */
        public SortedDictionary<string, object> FormatSorted(string xml, string sign)
        {
            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

            if (string.IsNullOrEmpty(xml))
            {
                m_values["return_code"] = "ERROR";
                return m_values;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;

            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }

            return m_values;

        }

        public Hashtable FromXml(string xml,string key, ILogger logger)
        {
            if (string.IsNullOrEmpty(xml))
            {
                logger.Error("将空的xml串转换为WxPayData不合法!");
                throw new Exception("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                this.SetParameter(xe.Name, xe.InnerText);//获取xml的键值对到WxPayData内部的数据中
            }

            try
            {
                //2015-06-29 错误是没有签名
                if (this.GetParameter("return_code") != "SUCCESS")
                {
                    return Parameters;
                }
                //CheckSign(key, logger);//验证签名,不通过会抛异常
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Parameters;
        }

        /// <summary>
        /// 转换为json
        /// </summary>
        /// <returns></returns>
        public string FromJson()
        {
            return JsonConvert.SerializeObject(this.Parameters);
        }



        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            return this.Parameters.ContainsKey(key);
        }

        /**
       * 
       * 检测签名是否正确
       * 正确返回true，错误抛异常
       */
        public bool CheckSign(string key,ILogger logger)
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                return false;
            }
            //如果设置了签名但是签名为空，则抛异常
            else if ( GetParameter("sign") == null || GetParameter("sign").ToString() == "")
            {
                return false;
            }

            //获取接收到的签名
            string return_sign = GetParameter("sign").ToString();

            //在本地计算新的签名
            string cal_sign = CreateMd5Sign(key);

            logger.Debug("本地签名为：" + cal_sign);

            if (cal_sign == return_sign)
            {
                return true;
            }
            return false;
        }
    }
}