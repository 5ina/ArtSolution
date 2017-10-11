using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;
using System;

namespace ArtSolution.News
{
    /// <summary>
    /// 促销专题服务接口
    /// </summary>
    public interface IPromotionalService: IApplicationService
    {
        /// <summary>
        /// 新增专题
        /// </summary>
        /// <param name="model"></param>
        void InsertPromotional(Promotional model);

        /// <summary>
        /// 更新专题
        /// </summary>
        /// <param name="model"></param>
        void UpdatePromotional(Promotional model);


        /// <summary>
        /// 根据主键获取专题
        /// </summary>
        /// <param name="promotionalId"></param>
        /// <returns></returns>
        Promotional GetPromotionalById(int promotionalId);

        /// <summary>
        /// 删除专题
        /// </summary>
        /// <param name="promotionalId"></param>
        void DeletePromotional(int promotionalId);

        /// <summary>
        /// 获取所有专题
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="showHidden"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Promotional> GetAllPromotionals(string keywords = "", bool showHidden = false,
            DateTime? createdFrom = null,
            DateTime? createdTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
