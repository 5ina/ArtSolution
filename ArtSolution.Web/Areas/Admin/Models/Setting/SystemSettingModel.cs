using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class SystemSettingModel
    {
        public SystemSettingModel()
        {
            this.Result = false;
        }

        [DisplayName("网站名称")]
        public string SiteName { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }
        [DisplayName("关键字")]
        public string Keywords { get; set; }
        [DisplayName("说明")]
        public string Description { get; set; }
        [DisplayName("联系电话")]
        public string Tel { get; set; }


        public bool Result { get; set; }
    }
}