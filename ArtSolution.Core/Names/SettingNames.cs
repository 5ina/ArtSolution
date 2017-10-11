namespace ArtSolution.Names
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemSettingNames
    {
        public static string SiteName { get { return "Setting.SiteName"; } }
        public static string Title { get { return "Setting.Title"; } }
        public static string Keywords { get { return "Setting.Keywords"; } }
        public static string Description { get { return "Setting.Description"; } }
        public static string Tel { get { return "Setting.Tel"; } }
    }

    /// <summary>
    /// 媒体配置
    /// </summary>
    public class MediaSettingNames
    {
        /// <summary>
        /// 是否本地存储
        /// </summary>
        public static string IsLocalStorage { get { return "Setting.IsLocalStorage"; } }

        /// <summary>
        /// 使用OSS存储图片
        /// </summary>
        public static string OssStorage { get { return "Setting.OssStorage"; } }
        public static string AccessKeyId { get { return "Setting.AccessKeyId"; } }
        public static string AccessKeySecret { get { return "Setting.AccessKeySecret"; } }
        public static string Endpoint { get { return "Setting.Endpoint"; } }

        public static string Bucket { get { return "Setting.Bucket"; } }
    }

    /// <summary>
    /// 订单配置
    /// </summary>
    public class OrderSettingNames
    {
        /// <summary>
        /// 订单失效时间
        /// </summary>
        public static string OrderFailureTime{ get { return "Setting.Order.OrderFailureTime"; } }

        /// <summary>
        /// 免费配送最小金额
        /// </summary>
        public static string OrderFreeShip{ get { return "Setting.Order.OrderFreeShip"; } }

        /// <summary>
        /// 配送费
        /// </summary>
        public static string ShipFee { get { return "Setting.Order.ShipFee"; } }

    }

    /// <summary>
    /// 微信配置
    /// </summary>
    public class WeChatSettingNames
    {
        /// <summary>
        /// AppId
        /// </summary>
        public static string AppId { get { return "Setting.Wechat.AppId"; } }

        /// <summary>
        /// mchid 微信支付Id
        /// </summary>
        public static string MchId { get { return "Setting.Wechat.MchId"; } }
        /// <summary>
        /// 商户支付Key
        /// </summary>
        public static string Key { get { return "Setting.Wechat.Key"; } }
        /// <summary>
        /// 支付回调地址
        /// </summary>
        public static string NotifyUrl { get { return "Setting.Wechat.NotifyUrl"; } }
        
        /// <summary>
        /// AppSecret
        /// </summary>
        public static string AppSecret { get { return "Setting.Wechat.Secret"; } }

        public static string Token { get { return "Setting.Wechat.Token"; } }

        /// <summary>
        /// 推广时间(天数)
        /// </summary>
        public static string Expire { get { return "Setting.Wechat.Expire"; } }

    }

    /// <summary>
    /// 公共配置
    /// </summary>
    public class CommonSettingNames
    {
        /// <summary>
        /// 商店名称
        /// </summary>
        public static string StoreName { get { return "Setting.Common.StoreName"; } }

        /// <summary>
        /// 商店URL
        /// </summary>
        public static string StoreURL { get { return "Setting.Common.StoreURL"; } }

        /// <summary>
        /// MetaTitle
        /// </summary>
        public static string MetaTitle { get { return "Setting.Common.MetaTitle"; } }
        /// <summary>
        /// MetaKeywords
        /// </summary>
        public static string MetaKeywords { get { return "Setting.Common.MetaKeywords"; } }
        /// <summary>
        /// MetaDescription
        /// </summary>
        public static string MetaDescription { get { return "Setting.Common.MetaDescription"; } }

        /// <summary>
        /// 是否启用图标
        /// </summary>
        public static string EnabledIcon { get { return "Setting.Common.EnabledIcon"; } }
        /// <summary>
        /// 是否开启评论
        /// </summary>
        public static string EnabledReview { get { return "Setting.Common.EnabledReview"; } }
    }

    /// <summary>
    /// 阿里云配置
    /// </summary>
    public class AliyunSettingNames
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        public static string AccessKeyId { get { return "Setting.Aliyun.AccessKeyId"; } }

        /// <summary>
        /// SecretAccessKey
        /// </summary>
        public static string SecretAccessKey { get { return "Setting.Aliyun.SecretAccessKey"; } }
        /// <summary>
        /// Endpoint
        /// </summary>
        public static string Endpoint { get { return "Setting.Aliyun.Endpoint"; } }
    }

    /// <summary>
    /// 积分配置
    /// </summary>
    public class RewardSettingNames {

        /// <summary>
        /// 是否启用积分
        /// </summary>
        public static string Enabled { get { return "Setting.Reward.Enabled"; } }

        /// <summary>
        /// 积分消费的比率（1分需要多少消费）
        /// </summary>
        public static string ExchangeRate { get { return "Setting.Reward.ExchangeRate"; } }
        
        /// <summary>
        /// 首次关注赠送的积分
        /// </summary>
        public static string PointsForRegistration { get { return "Setting.Reward.PointsForRegistration"; } }

        /// <summary>
        /// 首次下单赠送的积分（额外）
        /// </summary>
        public static string PointsForFirstOrder { get { return "Setting.Reward.PointsForFirstOrder"; } }
    }


    /// <summary>
    /// 推广者配置
    /// </summary>
    public class PromotersSettings
    {
        /// <summary>
        /// 是否启用推广者
        /// </summary>
        public static string Enabled { get { return "Setting.Promoters.Enabled"; } }

        /// <summary>
        /// 奖励模式（时间/单数）
        /// </summary>
        public static string RewardMode { get { return "Setting.Promoters.RewardMode"; } }

        /// <summary>
        /// 奖励值（多少天内奖励/多少单内奖励）
        /// </summary>
        public static string ModeValue { get { return "Setting.Promoters.ModeValue"; } }

        /// <summary>
        /// 奖励比率（订单额的百分比，不含运费）
        /// </summary>
        public static string RewardRate{ get { return "Setting.Promoters.RewardRate"; } }

        /// <summary>
        /// 申请推广人的条件
        /// </summary>
        public static string ApplyCondition { get { return "Setting.Promoters.ApplyCondition"; } }
        /// <summary>
        /// 条件源自于（订单数量/金额）
        /// </summary>
        public static string ApplyForSource { get { return "Setting.Promoters.ApplyForSource"; } }
        /// <summary>
        /// 应允值
        /// </summary>
        public static string ApplyValue { get { return "Setting.Promoters.ApplyForValue"; } }
        public static string IsAudit { get { return "Setting.Promoters.IsAudit"; } }


    }
}
