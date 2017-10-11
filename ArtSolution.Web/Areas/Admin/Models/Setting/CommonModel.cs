using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class CommonModel
    {
        public CommonModel()
        {
            this.Result = false;
        }


        [DisplayName("商城名称")]
        public string StoreName { get; set; }

        [DisplayName("商城地址")]
        public string StoreURL { get; set; }
        [DisplayName("MetaTitle")]
        public string MetaTitle { get; set; }
        [DisplayName("MetaKeywords")]
        public string MetaKeywords { get; set; }
        [DisplayName("MetaDescription")]
        public string MetaDescription { get; set; }
        [DisplayName("是否启用图标")]
        public bool EnabledIcon { get; set; }
        [DisplayName("开启评论")]
        public bool EnabledReview { get; set; }

        public bool Result { get; set; }

    }
}