using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{
    public class CatalogDetailModel : CategoryModel
    {
        public IList<CategoryModel> SubCatagoryies { get; set; }

        public IList<SimpleProductModel> SubProducts { get; set; }
        /// <summary>
        /// 商品总数量
        /// </summary>
        public int TotalProductCount { get; set; }
    }
}