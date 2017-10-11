using System;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Orders;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ArtSolution.Orders
{
    public class DeliveryService : ArtSolutionAppServiceBase, IDeliveryService
    {
        #region Ctor && Field

        private readonly IRepository<Delivery> _deliveryRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public DeliveryService(IRepository<Delivery> deliveryRepository,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._deliveryRepository = deliveryRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }


        #endregion

        #region Method

        public IPagedResult<Delivery> GetAllDeliveries(string keywords = "", bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _deliveryRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(d => d.Name.Contains(keywords));

            if (!showHidden)
                query = query.Where(d => d.Active);


            query = query.OrderBy(d => d.DisplayOrder);
            return new PagedResult<Delivery>(query, pageIndex, pageSize);
        }

        public Delivery GetDelivery(int deliveryId)
        {
            if (deliveryId > 0)
                return _deliveryRepository.Get(deliveryId);
            return null;
        }

        public void InsertDelivery(Delivery delivery)
        {
            if (delivery == null)
                throw new Exception("delivery");
            _deliveryRepository.Insert(delivery);
        }

        public void UpdateDelivery(Delivery delivery)
        {
            if (delivery == null)
                throw new Exception("delivery");
            _deliveryRepository.Update(delivery);
        }

        public void DeleteDelivery(int deliveryId)
        {
            _deliveryRepository.Delete(deliveryId);
        }

        #endregion
    }
}
