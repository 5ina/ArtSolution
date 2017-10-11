using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class AliyunSettingModel
    {
        public AliyunSettingModel()
        {
            this.Result = false;
        }
        [DisplayName("AccessKey")]
        public string AccessKeyId { get; set; }

        [DisplayName("SecretAccessKey")]
        public string SecretAccessKey { get; set; }
        [DisplayName("Endpoint")]
        public string Endpoint { get; set; }

        public bool Result { get; set; }
    }
}