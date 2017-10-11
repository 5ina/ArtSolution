namespace ArtSolution.Names
{
    public static class CustomerCacheNames
    {
        /// <summary>
        /// 统计模块的用户统计缓存名称
        /// </summary>
        public static string CACHE_CUSTOMER_StatisticalOverview { get { return "cache.statistical.customer.overview"; } }

        /// <summary>
        /// 微信信息
        /// </summary>
        public static string CACHE_CUSTOMER_TO_WECHAT { get { return "store.customers.wechat.by-customerid-{0}"; } }
    }

    public static class OrderCacheNames
    {
        /// <summary>
        /// 统计模块的用户统计缓存名称
        /// </summary>
        public static string CACHE_ORDER_StatisticalOverview { get { return "cache.statistical.order.overview"; } }
    }

    public static class ProductCacheNames
    {
        /// <summary>
        /// 统计模块的用户统计缓存名称
        /// </summary>
        public static string CACHE_PRODUCT_StatisticalOverview { get { return "cache.statistical.product.overview"; } }

    }
}
