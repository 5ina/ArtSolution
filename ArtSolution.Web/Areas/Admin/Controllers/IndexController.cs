using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class IndexController : ArtSolutionControllerAdminBase
    {
        // GET: Admin/Index
        public ActionResult Index()
        {
            return View();
        }
    }
}