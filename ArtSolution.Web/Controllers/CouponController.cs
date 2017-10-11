using System.Web.Mvc;

namespace ArtSolution.Web.Controllers
{
    /// <summary>
    /// 优惠券控制器
    /// </summary>
    public class CouponController : ArtSolutionControllerBase
    {
        // GET: Coupon
        public ActionResult Index()
        {
            return View();
        }
    }
}