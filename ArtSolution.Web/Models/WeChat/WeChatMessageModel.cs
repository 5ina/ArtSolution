using System.Collections.Generic;

namespace ArtSolution.Web.Models.WeChat
{
    public class WeChatMessageModel
    {
        public WeChatMessageModel()
        {
            this.news = new News();
        }
        /// <summary>
        /// OpenId
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 消息类型（news)
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 新闻类型
        /// </summary>
        public News news { get; set; }

        public class News
        {
            public News()
            {
                this.articles = new List<ArticlesItem>();
            }
            /// <summary>
            /// 
            /// </summary>
            public List<ArticlesItem> articles { get; set; }
        }

        public class ArticlesItem
        {

            /// <summary>
            /// 
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string picurl { get; set; }
        }
    }
}