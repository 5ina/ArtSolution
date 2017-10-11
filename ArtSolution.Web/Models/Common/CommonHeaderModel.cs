namespace ArtSolution.Web.Models.Common
{
    public class CommonHeaderModel
    {
        /// <summary>
        /// 商城名称
        /// </summary>
        public string StoreName { get; set; }

        public string StoreUrl { get; set; }

        public bool EnabledIcon { get; set; }

        /// <summary>
        /// 用户是否登录
        /// </summary>
        public bool HasLoginCustomer { get; set; }
        
    }
}
