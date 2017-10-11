using Abp.Domain.Repositories;
using ArtSolution.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ArtSolution.Orders
{
    public class ReturnOrderService : ArtSolutionAppServiceBase, IReturnOrderService
    {

        #region Ctor && Field

        private readonly IRepository<ReturnOrder> _returnRepository;

        public ReturnOrderService(IRepository<ReturnOrder> returnRepository)
        {
            this._returnRepository = returnRepository;
        }
        #endregion

        #region Method
        
        public void DeleteReturnOrder(int returnId)
        {
            if (returnId > 0)
                _returnRepository.Delete(returnId);
        }

        public IPagedResult<ReturnOrder> GetAllReturnOrders(int? customerId = null,
            int? orderId = null,string keywords = "",
            int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = _returnRepository.GetAll();
            if (customerId.HasValue)
                query = query.Where(r => r.CustomerId == customerId.Value);

            if (orderId.HasValue)
                query = query.Where(r => r.OrderId == orderId.Value);
            
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(r => r.ReasonForReturn.Contains(keywords));

            query = query.OrderByDescending(r => r.CreationTime);
            return new PagedResult<ReturnOrder>(query, pageIndex, pageSize);
        }

        public ReturnOrder GetReturnOrder(int returnId)
        {
            if (returnId == 0)
                return null;
            return _returnRepository.Get(returnId);
        }

        public void InsertReturnOrder(ReturnOrder model)
        {
            if (model == null)
                throw new Exception("returnOrder");
            _returnRepository.Insert(model);
        }

        public void UpdateReturnOrder(ReturnOrder model)
        {
            if (model == null)
                throw new Exception("returnOrder");
            _returnRepository.Update(model);
        }


        #endregion
    }
}
