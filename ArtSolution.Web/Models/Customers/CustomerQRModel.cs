using System;

namespace ArtSolution.Web.Models.Customers
{
    /// <summary>
    /// 我的二维码
    /// </summary>
    public class CustomerQRModel
    {
        public int CustomerID { get; set; }

        public DateTime CreateTime { get; set; }

        public string QR_Url { get; set; }
        /// <summary>
        /// 有效天数
        /// </summary>
        public int Expire { get; set; }
    }
}