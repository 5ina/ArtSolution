using System.Collections.Generic;

namespace ArtSolution.Web.Models.News
{
    public class PromotionalListModel
    {

        public PromotionalListModel()
        {
            this.Items = new List<PromotionalModel>();
        }

        public IList<PromotionalModel> Items { get; set; }

        public int Current { get; set; }

        public int Count { get; set; }

        public int Pages { get; set; }
    }
}