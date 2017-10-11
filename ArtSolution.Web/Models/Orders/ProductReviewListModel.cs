using System.Collections.Generic;

namespace ArtSolution.Web.Models.Orders
{
    public class ProductReviewListModel
    {

        public ProductReviewListModel()
        {
            this.Reviews = new List<ProductReviewModel>();
        }
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int ProductId { get; set; }

        public IList<ProductReviewModel> Reviews { get; set; }
    }
}