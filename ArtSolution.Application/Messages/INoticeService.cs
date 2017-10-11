using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.Messages;
using System.Threading.Tasks;

namespace ArtSolution.Messages
{
    public interface INoticeService : IApplicationService
    {

        Task NewCustomer(Customer customer);

        //订阅通知
        Task Subscribe_SentFrendshipRequest(long userId);

        /// <summary>
        /// 新增通知
        /// </summary>
        /// <param name="notice"></param>
        void InsertNotice(Notice notice);
        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="notice"></param>
        void UpdateNotice(Notice notice);

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="noticeId"></param>
        void DeleteNotice(int noticeId);

        /// <summary>
        /// 获取通知
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        Notice GetNoticeById(int noticeId);

        /// <summary>
        /// 获取所有通知
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="toId"></param>
        /// <param name="isRead"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Notice> GetAllNotices(int? fromId = null, int? toId = null, bool? isRead = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
