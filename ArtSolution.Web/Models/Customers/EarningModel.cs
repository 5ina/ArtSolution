namespace ArtSolution.Web.Models.Customers
{
    public class EarningModel
    {
        public int CustomerId { get; set; }
        /// <summary>
        /// 总资产
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// 当月提款次数
        /// </summary>
        public int CurrentWithdrawals { get; set; }

        /// <summary>
        /// 当月提款金额
        /// </summary>
        public decimal CurrentWithdrawalAmount { get; set; }

        /// <summary>
        ///  当月返佣单数
        /// </summary>
        public int CurrentCommissionCount { get; set; }
        /// <summary>
        ///  当月返佣金额
        /// </summary>
        public decimal CurrentCommissionAmount { get; set; }
    }
}