using System.Collections.Generic;

namespace ArtSolution.Web.Models.Orders
{
    public class SendMessageToAdminModel
    {
        public SendMessageToAdminModel()
        {
            this.text = new Text();
        }

        /// <summary>
        /// 接收人OpenId
        /// </summary>
        public List<string> touser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Text text { get; set; }
        public class Text
        {
            /// <summary>
            /// 文本
            /// </summary>
            public string content { get; set; }
        }
    }
}