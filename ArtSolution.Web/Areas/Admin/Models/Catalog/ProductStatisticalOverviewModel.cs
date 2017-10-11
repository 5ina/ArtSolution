namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    public class ProductStatisticalOverviewModel
    {
        /// <summary>
        /// 商品总数量
        /// </summary>
        public int ProductTotal { get; set; }

        /// <summary>
        /// 上架的商品
        /// </summary>
        public int PulishedTotal { get; set; }
    }
}