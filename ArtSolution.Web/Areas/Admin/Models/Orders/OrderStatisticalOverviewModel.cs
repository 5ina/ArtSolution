namespace ArtSolution.Web.Areas.Admin.Models.Orders
{
    public class OrderStatisticalOverviewModel
    {
        public int OrderTotalCount { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public int OrderWeekTotalCount { get; set; }

        public decimal OrderWeekTotalPrice { get; set; }

        public int OrderMonthTotalCount { get; set; }

        public decimal OrderMonthTotalPrice { get; set; }
    }
}