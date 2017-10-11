using Abp.AutoMapper;
using ArtSolution.Domain.official;
using ArtSolution.Official;
using ArtSolution.Web.Areas.Official.Models;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Official.Controllers
{
    public class HomeController : ArtSolutionControllerOfficialBase
    {
        #region Ctor && Field

        private readonly IMessageBoardService _messageService;
        public HomeController(IMessageBoardService messageRepository)
        {
            this._messageService = messageRepository;
        }

        #endregion

        #region Method
        // GET: Official/Home
        public ActionResult Index()
        {
            var model = new MessageModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Message(MessageModel model)
        {
            var entity = model.MapTo<MessageBoard>();
            _messageService.Insert(entity);
            return AbpJson("ok");
        }
        #endregion
    }
}