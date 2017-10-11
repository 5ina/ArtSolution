using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class RewardSettingModel
    {
        public RewardSettingModel()
        {
            this.Result = false;
        }

        /// <summary>
        /// 是否启用积分
        /// </summary>
        [DisplayName("是否启用")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 积分消费的比率（1分需要多少消费）
        /// </summary>
        [DisplayName("积分比率")]
        [Description("1积分需要多少消费额")]
        [UIHint("Rate")]
        public decimal ExchangeRate { get; set; }


        /// <summary>
        /// 首次关注赠送的积分
        /// </summary>
        [DisplayName("首次关注赠送积分")]
        public int PointsForRegistration { get; set; }


        /// <summary>
        /// 首次下单赠送的积分（额外）
        /// </summary>
        [DisplayName("首单赠送")]
        [Description("额外赠送的积分")]
        public int PointsForFirstOrder { get; set; }


        public bool Result { get; set; }
    }
}