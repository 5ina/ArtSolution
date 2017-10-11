using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.official;
using System;

namespace ArtSolution.Official
{
    /// <summary>
    /// 留言板服务接口
    /// </summary>
    public interface IMessageBoardService: IApplicationService
    {
        /// <summary>
        /// 新增留言
        /// </summary>
        /// <param name="message"></param>
        void Insert(MessageBoard message);
        /// <summary>
        /// 更新留言
        /// </summary>
        /// <param name="message"></param>
        void Update(MessageBoard message);
        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="messageId"></param>
        void Delete(int messageId);
        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        MessageBoard Get(int messageId);
        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="isRead"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<MessageBoard> GetAlls(DateTime? createdFrom = null,
            DateTime? createdTo = null, bool? isRead = null,
            int pageIndex = 0, int pageSize = int.MaxValue);


    }
}
