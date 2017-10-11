using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSolution.Web.Framework.DataGrids
{
    public class DataSourceRequest
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public DataSourceRequest()
        {
            this.Page = 1;
            this.PageSize = 20;
        }
    }
}