namespace ArtSolution.Web.Framework
{
    public class CacheNames
    {
        /// <summary>
        /// 配置
        /// </summary>
        public class Settings
        {
            /// <summary>
            /// 短信配置
            /// </summary>
            public const string SMS_MESSAGE_SETTINGS = "store.settings.sms";
            /// <summary>
            /// 积分配置
            /// </summary>
            public const string REWARD_SETTINGS = "store.settings.reward";
            /// <summary>
            /// 推广者配置
            /// </summary>
            public const string PROMOTERS_SETTINGS = "store.settings.promoters";
        }

        public class Topics
        {
            /// <summary>
            /// Topic的系统名称缓存
            /// </summary>
            public const string TOPIC_SYSTEM = "store.topics.systemname-{0}";
        }

        /// <summary>
        /// 优惠券
        /// </summary>
        public class Coupons
        {
            /// <summary>
            /// Coupon-By-Id
            /// </summary>
            public const string COUPON_ID = "store.coupon.id-{0}";
            
        }

        public class Products
        {

            /// <summary>
            /// 商品品牌数量
            /// </summary>
            public static string CACHE_PRODUCT_TO_BRAND_COUNT { get { return "store.producttobrandcount.by-brandId-{0}"; } }
        }

        public class CACHE_ORDERS
        {
            public static string CACHE_ORDER_BY_CUSTOMER { get { return "store.orders.by-customer-{0}"; } }
        }
        
    }
    
}