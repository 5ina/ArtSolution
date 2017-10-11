using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;
using System;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 返佣记录表服务接口
    /// </summary>
    public interface ICommissionService : IApplicationService
    {
        /// <summary>
        /// 新增返佣记录
        /// </summary>
        /// <param name="entity"></param>
        void InsertCommission(Commission entity);

        /// <summary>
        /// 更新返佣记录
        /// </summary>
        /// <param name="entity"></param>
        void UpdateCommission(Commission entity);

        /// <summary>
        /// 根据主键获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Commission GetCommissionById(int id);

        /// <summary>
        /// 获取所有的返佣记录
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <param name="createdFrom"></param>
        /// <param name="createdTo"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Commission> GetAllCommissions(int customerId = 0,int orderId =0,
            DateTime? createdFrom = null, DateTime? createdTo = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
