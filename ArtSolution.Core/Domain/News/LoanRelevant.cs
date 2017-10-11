using System.Collections;
using System.Collections.Generic;

namespace ArtSolution.Domain.News
{
    public class LoanRelevant
    {
        /// <summary>
        /// 贷款额度
        /// </summary>
        public static Dictionary<int ,string> Quotas = GetQuotas();
        /// <summary>
        /// 贷款周期
        /// </summary>
        public static Dictionary<int, string> Cycle = GetCyclis();

        /// <summary>
        /// 获取额度
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, string> GetQuotas()
        {
            var table = new Dictionary<int, string>();
            table.Add(1, "3000元");
            table.Add(2, "5000元");
            table.Add(3, "8000元");
            return table;
        }
        private static Dictionary<int, string> GetCyclis()
        {
            var table = new Dictionary<int, string>();
            table.Add(1, "3个月");
            table.Add(2, "6个月");
            table.Add(3, "9个月");
            return table;
        }
    }
}
