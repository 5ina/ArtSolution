namespace ArtSolution.Web.Models.WeChat
{
    public class WxMessage
    {
        /// <summary>
        /// 接收方
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送者
        /// </summary>
        public string FromUserName { get; set; }
        public long CreateTime { get; set; }

        public string Content { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string EventKey { get; set; }
    }
}