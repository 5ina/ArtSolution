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
    public class PaymentRecordService: ArtSolutionAppServiceBase,IPaymentRecordService
    {
        #region Ctor && Field

        private readonly IRepository<PaymentRecord> _recordRepository;

        public PaymentRecordService(IRepository<PaymentRecord> orderRepository)
        {
            this._recordRepository = orderRepository;
        }
        #endregion

        #region Method
        public void DeletePaymentRecord(int recordId)
        {
            _recordRepository.Delete(recordId);
        }

        public IPagedResult<PaymentRecord> GetAllPaymentRecords(int customerId = 0, int orderId = 0,
            int? audit = null, string keywords = "", 
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _recordRepository.GetAll();

            if (customerId > 0)
                query = query.Where(r => r.CustomerId == customerId);

            if (orderId > 0)
                query = query.Where(r => r.OrderId == customerId);

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(r => r.OpenId.Contains(keywords) || r.Transaction.Contains(keywords));

            if (audit.HasValue)
                query = query.Where(r => r.Audit == audit.Value);

            query = query.OrderByDescending(r => r.Id);

            return new PagedResult<PaymentRecord>(query, pageIndex, pageSize);
        }

        public PaymentRecord GetPaymentRecord(int recordId)
        {
            if (recordId > 0)
                return _recordRepository.Get(recordId);
            return null;
        }

        public void InsertPaymentRecord(PaymentRecord record)
        {
            if (record != null)
                _recordRepository.Insert(record);
        }

        public void UpdatePaymentRecord(PaymentRecord record)
        {
            if (record != null)
                _recordRepository.Update(record);
        }


        #endregion
        
    }
}
