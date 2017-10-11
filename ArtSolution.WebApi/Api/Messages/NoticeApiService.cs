using Abp.Localization;
using Abp.Notifications;
using Abp.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolution.Api.Messages
{
    public class NoticeApiService : AbpApiController, INoticeApiService
    {

        #region Ctor && Field

        private readonly INotificationPublisher _notiticationPublisher;

        public NoticeApiService(INotificationPublisher notiticationPublisher)
        {
            this._notiticationPublisher = notiticationPublisher;
        }


        #endregion

        #region Method

        public void PublisNotice()
        {
            var data = new LocalizableMessageNotificationData(new LocalizableString("通知", ArtSolutionConsts.LocalizationSourceName));
            
            _notiticationPublisher.Publish("通知", data, severity: NotificationSeverity.Warn);
        }
        #endregion
    }
}
