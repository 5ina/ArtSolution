using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Messages;
using Abp.Domain.Repositories;
using ArtSolution.Domain.Customers;
using System.Threading.Tasks;
using Abp.Notifications;
using Abp;

namespace ArtSolution.Messages
{
    public class NoticeService : ArtSolutionAppServiceBase, INoticeService
    {
        #region Ctor && Field

        private readonly IRepository<Notice> _noticeRepository;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        public NoticeService(IRepository<Notice> noticeRepository, 
            INotificationPublisher notificationPublisher,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            this._noticeRepository = noticeRepository;
            this._notificationPublisher = notificationPublisher;
            this._notificationSubscriptionManager = notificationSubscriptionManager;
        }

        #endregion

        #region Method
        public void DeleteNotice(int noticeId)
        {
            try {
                _noticeRepository.Delete(noticeId);
            }
            catch
            {
                throw new ArgumentNullException("notice");
            }
        }

        public IPagedResult<Notice> GetAllNotices(int? fromId = null, int? toId = null, 
            bool? isRead = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _noticeRepository.GetAll();
            if (fromId.HasValue)
                query = query.Where(n => n.FromId == fromId.Value);

            if (toId.HasValue)
                query = query.Where(n => n.ToId == toId.Value);

            if (isRead.HasValue)
                query = query.Where(n => n.IsRead == isRead.Value);
            query = query.OrderByDescending(n => n.CreationTime);
            return new PagedResult<Notice>(query, pageIndex, pageSize);
        }

        public Notice GetNoticeById(int noticeId)
        {
            try
            {
               return _noticeRepository.Get(noticeId);
            }
            catch
            {
                throw new ArgumentNullException("notice");
            }
        }

        public void InsertNotice(Notice notice)
        {
            if (notice == null)
                throw new ArgumentNullException("notice");

            _noticeRepository.Insert(notice);
        }

        public async Task NewCustomer(Customer customer)
        {

            await _notificationPublisher.PublishAsync("NewsCustomer",
                new MessageNotificationData("新用户关注"),
                severity: NotificationSeverity.Info,
                userIds: new Abp.UserIdentifier[] { });
                //userIds: new[] { customer.OpenId });
        }

        public async Task Subscribe_SentFrendshipRequest(long userId)
        {
            await _notificationSubscriptionManager.SubscribeAsync(new UserIdentifier(null, userId), "SentFrendshipRequest");
        }
        public async Task Publish_SentFrendshipRequest(string senderUserName, string friendshipMessage, UserIdentifier targetUserId)
        {
            await _notificationPublisher.PublishAsync("SentFrendshipRequest", new SentFrendshipRequestNotificationData(senderUserName, friendshipMessage), userIds: new[] { targetUserId });
        }
        public void UpdateNotice(Notice notice)
        {
            if (notice == null)
                throw new ArgumentNullException("notice");

            _noticeRepository.Update(notice);
        }
        #endregion
    }
}
