using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    public class CustomerListModel
    {
        public CustomerListModel()
        {
            this.AvailableCustomerRoles = new List<SelectListItem>();
        }

        public string Keywords { get; set; }
        
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }
        
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
        
        public bool? Sub { get; set; }

        public int? RoleId { get; set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    }
}