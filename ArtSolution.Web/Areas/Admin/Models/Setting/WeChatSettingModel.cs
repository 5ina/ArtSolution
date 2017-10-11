using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class WeChatSettingModel
    {
        public WeChatSettingModel()
        {
            this.Result = false;
        }


        [DisplayName("微信AppID")]
        public string AppId { get; set; }

        [DisplayName("微信AppSecret")]
        public string AppSecret { get; set; }
        [DisplayName("Token")]
        public string Token { get; set; }
        [DisplayName("微信支付商户Id")]
        public string MchId { get; set; }
        [DisplayName("微信商户支付秘钥")]
        public string Key { get; set; }
        [DisplayName("支付回调")]
        public string Notify_Url { get; set; }
        
        [DisplayName("二维码推广时间")]
        [UIHint("DisplayOrder")]
        public int Expire { get; set; }

        public bool Result { get; set; }

    }
}