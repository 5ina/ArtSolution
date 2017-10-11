using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;
using System;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 推广者申请列表
    /// </summary>
    public interface IApplyPromoterService: IApplicationService
    {
        /// <summary>
        /// 新增审核
        /// </summary>
        /// <param name="model"></param>
        void Insert(ApplyPromoter model);

        /// <summary>
        /// 更新审核
        /// </summary>
        /// <param name="model"></param>
        void Update(ApplyPromoter model);
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApplyPromoter GetPromoterById(int id);
        /// <summary>
        /// 获取所有的审核信息
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="audit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ApplyPromoter> GetAllList(string keywords = "",
            DateTime? createdFrom = null,DateTime? createdTo = null,
            bool? audit = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
