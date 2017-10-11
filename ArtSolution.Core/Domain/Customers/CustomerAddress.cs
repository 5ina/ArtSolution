using Abp.Domain.Entities;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 用户地址
    /// </summary>
    public class CustomerAddress: Entity
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailInfo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string UserName { get; set; }

    }
}
