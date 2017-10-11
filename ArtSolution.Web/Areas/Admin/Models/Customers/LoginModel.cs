using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    public class LoginModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
        
        [DisplayName("保存密码")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}