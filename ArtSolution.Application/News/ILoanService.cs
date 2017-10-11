using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;

namespace ArtSolution.News
{
    /// <summary>
    /// 借款服务接口
    /// </summary>
    public interface ILoanService: IApplicationService
    {
        /// <summary>
        /// 新增借款
        /// </summary>
        /// <param name="loan"></param>
        int InsertLoan(Loan loan);
        /// <summary>
        /// 更新借款
        /// </summary>
        /// <param name="loan"></param>
        void UpdateLoan(Loan loan);

        /// <summary>
        /// 删除借款
        /// </summary>
        /// <param name="loanId"></param>
        void DeleteLoan(int loanId);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="loanId"></param>
        /// <returns></returns>
        Loan GetLoanById(int loanId);

        /// <summary>
        /// 获取所有借款
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Loan> GetAllLoans(int? audit = null, int pageIndex = 0 ,int pageSize = int.MaxValue);
    }
}
