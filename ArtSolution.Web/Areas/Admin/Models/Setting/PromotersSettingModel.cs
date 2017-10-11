using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    /// <summary>
    /// 推广者配置
    /// </summary>
    public class PromotersSettingModel
    {
        public PromotersSettingModel()
        {
            this.Result = false;
        }


        [DisplayName("是否启用")]
        public bool Enabled { get; set; }

        [DisplayName("奖励模式")]        
        public int RewardMode { get; set; }
        [DisplayName("奖励值")]
        public int ModeValue { get; set; }
        [DisplayName("奖励比率")]
        [UIHint("Rate")]
        public decimal RewardRate { get; set; }


        [DisplayName("申请条件")]
        public int ApplyCondition { get; set; }
        [DisplayName("要求额")]
        public int ApplyValue { get; set; }
        [DisplayName("是否审核")]
        public bool IsAudit { get; set; }

        public bool Result { get; set; }

    }
}