using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.News;
using Abp.Domain.Repositories;

namespace ArtSolution.News
{
    public class LoanService : ArtSolutionAppServiceBase, ILoanService
    {
        #region Ctor && Field

        private readonly IRepository<Loan> _loanRepository;
        public LoanService(IRepository<Loan> loanRepository)
        {
            this._loanRepository = loanRepository;
        }

        #endregion

        #region Method
        public void DeleteLoan(int loanId)
        {
            if (loanId > 0)
                _loanRepository.Delete(loanId);
        }

        public IPagedResult<Loan> GetAllLoans(int? audit = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _loanRepository.GetAll();

            if (audit.HasValue)
                query = query.Where(l => l.Audit == audit.Value);

            query = query.OrderByDescending(l => l.CreationTime);
            return new PagedResult<Loan>(query, pageIndex, pageSize);
        }

        public Loan GetLoanById(int loanId)
        {
            return _loanRepository.Get(loanId);
        }

        public int InsertLoan(Loan loan)
        {
            if (loan != null)
               return _loanRepository.InsertAndGetId(loan);
            return 0;
        }

        public void UpdateLoan(Loan loan)
        {
            if (loan != null)
                _loanRepository.Update(loan);
        }
        #endregion
    }
}
