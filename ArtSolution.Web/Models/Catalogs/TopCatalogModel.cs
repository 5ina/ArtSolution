using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{
    public class TopCatalogModel
    {
        public TopCatalogModel()
        {
            this.SubCategories = new List<CategoryModel>();
        }

        public IList<CategoryModel> SubCategories { get; set; }

        //当前选中的ID
        public int ActiveId { get; set; }
        /// <summary>
        /// 当前是否为顶级分类
        /// </summary>

        public bool IsTopCategory { get; set; }
    }
}