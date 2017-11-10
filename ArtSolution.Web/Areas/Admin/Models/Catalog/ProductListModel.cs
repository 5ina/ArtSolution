using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Catalog
{
    public class ProductListModel
    {

        public ProductListModel()
        {
            this.AvailableCategories = new List<SelectListItem>();
            this.CategoryIds = new List<int>();
            this.AvailableBrands = new List<SelectListItem>();            
        }

        [DisplayName("关键字")]
        public string Keywords { get; set; }

        [UIHint("KendoMultipleSelect")]
        public IList<int> CategoryIds { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableBrands { get; set; }

        [DisplayName("是否预售")]
        public bool? IsPreSell { get; set; }
        

        public int BrandId { get; set; }

    }
}