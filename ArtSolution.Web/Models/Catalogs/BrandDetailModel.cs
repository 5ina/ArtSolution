using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{
    public class BrandDetailModel : BrandModel
    {
        public IList<SimpleProductModel> SubProducts { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        public int TotalProductCount { get; set; }
    }
}