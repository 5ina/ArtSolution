using System.Collections.Generic;

namespace ArtSolution.Web.Models.Orders
{
    public class FavoriteListModel
    {

        public FavoriteListModel()
        {
            this.Favorites = new List<FavoriteModel>();
        }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public IList<FavoriteModel> Favorites { get; set; }
    }
}