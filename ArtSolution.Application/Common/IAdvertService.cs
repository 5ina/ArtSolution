using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Common;
using System;

namespace ArtSolution.Common
{
    public interface IAdvertService : IApplicationService
    {
        /// <summary>
        /// 删除首页广告
        /// </summary>
        /// <param name="Id"></param>
        void DeleteAdvert(int Id);

        /// <summary>
        /// 更新首页广告
        /// </summary>
        /// <param name="model"></param>
        void UpdateAdvert(Advert model);

        /// <summary>
        /// 新增首页广告
        /// </summary>
        /// <param name="model"></param>
        void InsertAdvert(Advert model);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="advertId"></param>
        /// <returns></returns>
        Advert GetAdvertById(int advertId);

        /// <summary>
        /// 查询所有的首页广告
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="showFrom"></param>
        /// <param name="showTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Advert> GetAllAdverts(string keywords = null,
            DateTime? showFrom = null,
            DateTime? showTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
