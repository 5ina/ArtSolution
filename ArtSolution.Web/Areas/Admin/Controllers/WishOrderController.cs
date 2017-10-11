using ArtSolution.News;
using ArtSolution.Web.Framework.DataGrids;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class WishOrderController : ArtSolutionControllerAdminBase
    {
        #region ctor && Fields
        private readonly IWishOrderService _wishService;


        public WishOrderController(IWishOrderService wishService)
        {
            this._wishService = wishService;
        }
        #endregion

        #region Method
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command,string keywords)
        {
            var wishs = _wishService.GetAllOrders(keywords: keywords,
                                        pageIndex: command.Page - 1,
                                        pageSize: command.PageSize);

            var jsonData = new DataSourceResult
            {
                Data = wishs.Items.Select(b => new
                {
                    Id = b.Id,
                    Brand = b.BrandName,
                    Product = b.ProductName,
                    Image = b.ProductImages,
                    CreationTime = b.CreationTime,
                }),
                Total = wishs.TotalCount
            };
            return AbpJson(jsonData);
        }
        #endregion
    }
}