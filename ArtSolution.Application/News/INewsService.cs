using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;

namespace ArtSolution.News
{
    public interface INewsService: IApplicationService
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="newId">主键</param>
        void DeleteNews(int newId);

        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="newsId">主键</param>
        /// <returns>News</returns>
        NewItem GetNewsById(int newsId);

        /// <summary>
        /// 获取所有新闻
        /// </summary>
        /// <param name="showHidden"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<NewItem> GetAllNews(bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 新增新闻
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int InsertNews(NewItem item);

        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <param name="item"></param>
        void UpdateNews(NewItem item);
    }
}
