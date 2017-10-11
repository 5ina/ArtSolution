using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Models.Orders
{
    public class OrderSettingsModel
    {
        /// <summary>
        /// 订单失效时间（分钟）
        /// </summary>
        public int OrderFailureTime { get; set; }
        /// <summary>
        /// 最低订单免费配送
        /// </summary>
        public decimal OrderFreeShip { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal ShipFee { get; set; }


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