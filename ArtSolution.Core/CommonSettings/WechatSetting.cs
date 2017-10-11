using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolution.CommonSettings
{
    /// <summary>
    /// 微信配置,用于服务层使用
    /// </summary>
    public class WechatSetting
    {        

        /// <summary>
        /// 微信AppID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 微信支付商户Id
        /// </summary>
        public string MchId { get; set; }
        /// <summary>
        /// 微信支付秘钥Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 回调Url
        /// </summary>
        public string Notify_Url { get; set; }

    }
}
