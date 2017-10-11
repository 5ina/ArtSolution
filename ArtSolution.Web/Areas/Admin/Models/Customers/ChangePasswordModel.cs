using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    public class ChangePasswordModel
    {
        [AllowHtml]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }        
    }
}