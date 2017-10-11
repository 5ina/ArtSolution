using System.Collections.Generic;

namespace ArtSolution.Web.Areas.Admin.Models.WeChat
{
    public class KfAccountModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { get; set; }
        /// <summary>
        /// 客服id
        /// </summary>
        public string kf_id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string kf_headimgurl { get; set; }
    }

    public class KfAccountListModel
    {
        public KfAccountListModel()
        {
            this.kf_list = new List<KfAccountModel>();
        }
        public List<KfAccountModel> kf_list { get; set; }
    }
}