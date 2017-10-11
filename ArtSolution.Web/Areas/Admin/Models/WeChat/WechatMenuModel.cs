using System.Collections.Generic;

namespace ArtSolution.Web.Areas.Admin.Models.WeChat
{
    public class WechatMenuModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 关键值
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WechatLinkModel> sub_button { get; set; }
    }

    public class WechatLinkModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string url { get; set; }
    }
}