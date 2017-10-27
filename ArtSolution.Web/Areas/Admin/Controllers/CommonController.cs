using Abp.Notifications;
using Abp.Runtime.Caching;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Controllers
{
    public class CommonController : ArtSolutionControllerAdminBase
    {

        #region Ctor && Field
        private readonly IUserNotificationManager _notificationManager;
        private readonly ICacheManager _cacheManager;

        public CommonController(IUserNotificationManager notificationManager,
            ICacheManager cacheManager)
        {
            this._notificationManager = notificationManager;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Method

        public ActionResult Notice()
        {
            var notices = _notificationManager.GetUserNotifications(new Abp.UserIdentifier(null, AbpSession.UserId.Value), UserNotificationState.Unread);
            return PartialView(notices);
        }

        
        #endregion
    }
}