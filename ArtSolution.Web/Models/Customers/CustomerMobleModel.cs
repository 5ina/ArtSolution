using System.ComponentModel;

namespace ArtSolution.Web.Models.Customers
{
    public class CustomerMobleModel
    {
        public int CustomerId { get; set; }

        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [DisplayName("验证码")]
        public string Captcha { get; set; }


        public bool Result { get; set; }

        public string Error { get; set; }
    }
}