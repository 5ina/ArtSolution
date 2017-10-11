using System.Collections.Generic;

namespace ArtSolution.Web.Models.Catalogs
{
    public class CatalogListModel
    {
        public CatalogListModel()
        {
            this.SubListCatalog = new List<CatalogModel>();
        }

        public IList<CatalogModel> SubListCatalog { get; set; }
    }
}