using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Customers;
using System;

namespace ArtSolution.Customers
{
    public interface IApplyCashService: IApplicationService
    {
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity"></param>
        void InsertApplyCash(ApplyCash entity);
        
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        void UpdateApplyCash(ApplyCash entity);

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        ApplyCash GetApplyCashById(int entityId);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entityId"></param>
        void DeleteApplyCash(int entityId);

        /// <summary>
        /// 获取所有的记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="createdFrom">创建时间</param>
        /// <param name="createdTo">创建时间</param>
        /// <param name="audit">审核状态</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<ApplyCash> GetAllApplyCashs(int customerId = 0,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            int? audit = null,
            int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
