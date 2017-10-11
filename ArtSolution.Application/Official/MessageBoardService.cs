using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.official;
using Abp.Domain.Repositories;

namespace ArtSolution.Official
{
    public class MessageBoardService : ArtSolutionAppServiceBase, IMessageBoardService
    {

        #region Ctor && Field

        private readonly IRepository<MessageBoard> _messageRepository;
        public MessageBoardService(IRepository<MessageBoard> messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        #endregion
        #region Method
        public void Delete(int messageId)
        {
            _messageRepository.Delete(messageId);
        }

        public MessageBoard Get(int messageId)
        {
            return _messageRepository.Get(messageId);
        }

        public IPagedResult<MessageBoard> GetAlls(DateTime? createdFrom = null, DateTime? createdTo = null,
            bool? isRead = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _messageRepository.GetAll();

            if (isRead.HasValue)
                query = query.Where(m => m.IsRead == isRead.Value);
            if (createdFrom.HasValue)
                query = query.Where(m => createdFrom.Value <= m.CreationTime);
            if (createdTo.HasValue)
                query = query.Where(m => createdTo.Value >= m.CreationTime);

            query = query.OrderByDescending(m => m.CreationTime);
            return new PagedResult<MessageBoard>(query, pageIndex, pageSize);
        }

        public void Insert(MessageBoard message)
        {
            if (message != null)
                _messageRepository.Insert(message);
        }

        public void Update(MessageBoard message)
        {
            if (message != null && message.Id > 0)
                _messageRepository.Update(message);
        }

        #endregion
    }
}
