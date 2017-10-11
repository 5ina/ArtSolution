using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class MediaSettingModel
    {
        public MediaSettingModel()
        {
            this.Result = false;
        }

        [DisplayName("AccessKey")]
        public string AccessKeyId { get; set; }
        [DisplayName("KeySecret")]
        public string AccessKeySecret { get; set; }
        [DisplayName("Endpoint")]
        public string Endpoint { get; set; }

        [DisplayName("Bucket")]
        public string Bucket { get; set; }
        
        [DisplayName("是否本地存储")]
        public bool IsLocalStorage { get; set; }

        public bool Result { get; set; }
    }
}