using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;

namespace ArtSolution.News
{
    /// <summary>
    /// 主题服务接口
    /// </summary>
    public interface ITopicService : IApplicationService
    {
        /// <summary>
        /// 新增主题
        /// </summary>
        /// <param name="topic"></param>
        void InsertTopic(Topic topic);

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topic"></param>
        void UpdateTopic(Topic topic);

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="topicId"></param>
        void DeleteTopic(int topicId);

        /// <summary>
        /// 根据主键获取主题
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        Topic GetTopicById(int topicId);

        /// <summary>
        /// 根据systemName获取主题
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        Topic GetTopicBySystemName(string systemName);
        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Topic> GetAllTopics(string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
