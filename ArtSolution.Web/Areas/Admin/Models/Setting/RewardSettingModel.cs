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


        [DisplayName("积分支付")]
        [Description("是否开启积分支付")]
        public bool RewardPaymentEnabled { get; set; }

        [DisplayName("金额倍率")]
        [Description("支付金额需要多少倍的积分")]
        public int PaymentRate { get; set; }



        [DisplayName("签到奖励")]
        [Description("是否开启签到奖励")]
        public bool SignRewardEnabled { get; set; }

        [DisplayName("首次奖励")]
        [Description("首次签到奖励")]
        public int FirstRewardPoint { get; set; }


        [DisplayName("最高奖励")]
        [Description("最高奖励积分")]
        public int MaxRewardPoint { get; set; }

        [DisplayName("连续登录奖励")]
        [Description("连续登录后追加上次附加的奖励")]
        public int AdditionalReward{ get; set; }
        public bool Result { get; set; }
    }
}