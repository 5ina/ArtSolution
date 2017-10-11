using Castle.Core.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ArtSolution.Web.Framework.WeChat
{
    public class WechatRequest
    {
        private readonly string _xml;
        private XmlDocument xmlDoc;
        protected Hashtable Parameters;
        public WechatRequest(string xml)
        {
            this._xml = xml;
            this.xmlDoc = new XmlDocument();
            this.Parameters = new Hashtable();
        }

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <returns></returns>
        public string LoadEvent(ILogger logger)
        {
            xmlDoc.LoadXml(this._xml);
            var msgType = xmlDoc.SelectSingleNode("/xml/MsgType").InnerText;
            logger.Debug("msgType:" + msgType);
            if (!String.IsNullOrWhiteSpace(msgType) && msgType == "event")
            {
                logger.Debug("Event:" + xmlDoc.SelectSingleNode("/xml/Event").InnerText);
                return xmlDoc.SelectSingleNode("/xml/Event").InnerText;
            }
               
            return "";
        }

        public Hashtable LoadXml()
        {
            xmlDoc.LoadXml(this._xml);
            this.Parameters.Add("ToUserName", xmlDoc.SelectSingleNode("/xml/ToUserName").InnerText);
            this.Parameters.Add("FromUserName", xmlDoc.SelectSingleNode("/xml/FromUserName").InnerText);
            this.Parameters.Add("CreateTime", xmlDoc.SelectSingleNode("/xml/CreateTime").InnerText);
            this.Parameters.Add("MsgType", xmlDoc.SelectSingleNode("/xml/MsgType").InnerText);
            this.Parameters.Add("Event", xmlDoc.SelectSingleNode("/xml/Event").InnerText);
            this.Parameters.Add("EventKey", xmlDoc.SelectSingleNode("/xml/EventKey").InnerText);
            return Parameters;
        }

    }
}