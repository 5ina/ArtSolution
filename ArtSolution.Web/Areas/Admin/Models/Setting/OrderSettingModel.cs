using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Setting
{
    public class OrderSettingModel
    {
        public OrderSettingModel()
        {
            this.Result = false;
        }
        [DisplayName("订单失效时间（分钟）")]
        public int OrderFailureTime { get; set; }
        [DisplayName("最低订单免费配送")]
        public decimal OrderFreeShip { get; set; }
        [DisplayName("配送费")]
        public decimal ShipFee { get; set; }

        public bool Result { get; set; }

    }
}