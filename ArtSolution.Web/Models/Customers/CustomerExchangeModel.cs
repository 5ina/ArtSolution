using System.ComponentModel;

namespace ArtSolution.Web.Models.Customers
{
    public class CustomerExchangeModel
    {
        public int CustomerId { get; set; }

        [DisplayName("输入代码")]
        public string Code { get; set; }

        [DisplayName("验证码")]
        public string VerifyCode { get; set; }
    }
}