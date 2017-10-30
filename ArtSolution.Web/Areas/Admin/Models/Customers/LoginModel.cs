using Abp.Runtime.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    public class LoginModel: ICustomValidate
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
        
        [DisplayName("保存密码")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (String.IsNullOrWhiteSpace(LoginName) )
            {
                context.Results.Add(new ValidationResult("请输入登录账户"));
            }
        }
    }
}