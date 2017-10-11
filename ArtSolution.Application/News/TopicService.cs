using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;
using Abp.Domain.Repositories;

namespace ArtSolution.News
{
    public class TopicService : ArtSolutionAppServiceBase, ITopicService
    {

        #region Ctor && Field

        private readonly IRepository<Topic> _topicRepository;
        public TopicService(IRepository<Topic> topicRepository)
        {
            this._topicRepository = topicRepository;
        }

        #endregion
        #region Method
        public void DeleteTopic(int topicId)
        {
            _topicRepository.Delete(topicId);
        }

        public IPagedResult<Topic> GetAllTopics(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _topicRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(t => t.SystemName.Contains(keywords) || t.Content.Contains(keywords) || t.Title.Contains(keywords));

            query = query.OrderByDescending(t => t.CreationTime);
            return new PagedResult<Topic>(query, pageIndex, pageSize);
        }

        public Topic GetTopicById(int topicId)
        {
            return _topicRepository.Get(topicId);
        }

        public Topic GetTopicBySystemName(string systemName)
        {
            return _topicRepository.FirstOrDefault(i => i.SystemName == systemName);
        }

        public void InsertTopic(Topic topic)
        {
            _topicRepository.Insert(topic);
        }

        public void UpdateTopic(Topic topic)
        {
            _topicRepository.Update(topic);
        }
        #endregion
    }
}
