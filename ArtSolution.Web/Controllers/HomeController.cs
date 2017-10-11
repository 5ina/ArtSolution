using ArtSolution.Common;
using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    public class HomeController : ArtSolutionControllerBase
    {
        private readonly ISettingService _settingService;
        public HomeController(ISettingService settingService)
        {
            this._settingService = settingService;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
    }
}