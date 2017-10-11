using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Discounts;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace ArtSolution.Discount
{
    public class CouponService : ArtSolutionAppServiceBase, ICouponService
    {
        #region Ctor && Field

        private readonly IRepository<Coupon> _couponRepository;

        private readonly IRepository<CouponTemplate> _tempRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManage;
        public CouponService(IRepository<Coupon> couponRepository,
            IRepository<CouponTemplate> tempRepository, IUnitOfWorkManager unitOfWorkManage)
        {
            this._couponRepository = couponRepository;
            this._tempRepository = tempRepository;
            this._unitOfWorkManage = unitOfWorkManage;
        }

        #endregion

        #region Method
        

        #region  Coupon


        public void DeleteCoupon(int couponId)
        {
            var coupon =  _couponRepository.Get(couponId);
            if (!coupon.Used)
                _couponRepository.Delete(coupon);

            throw new Exception("优惠券已经被使用，不能删除");
        }
        public IPagedResult<Coupon> GetAllCoupons(string keywords = "", int customerId = 0,
            int orderId = 0, bool? used = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _couponRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(c => c.Name.Contains(keywords));

            if (customerId > 0)
                query = query.Where(c => c.CustomerId == customerId);

            if (orderId > 0)
                query = query.Where(c => c.OrderId == orderId);

            if (used.HasValue)
                query = query.Where(c => c.Used == used.Value);

            query = query.OrderByDescending(c => c.Effective).OrderByDescending(c => c.CreationTime);

            return new PagedResult<Coupon>(query, pageIndex, pageSize);
        }

        public Coupon GetCouponById(int couponId)
        {
            if (couponId <= 0)
                throw new Exception("coupon");

            return _couponRepository.Get(couponId);
        }

        public int InsertCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new Exception("coupon");
            return _couponRepository.InsertAndGetId(coupon);
        }

        public void UpdateCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new Exception("coupon");

            _couponRepository.Update(coupon);
        }


        public Coupon GetCouponByCode(string code)
        {
            return _couponRepository.FirstOrDefault(c => c.DiscountCode == code);
        }

        #endregion

        public Coupon GetCouponByOrderId(int orderId)
        {
            return _couponRepository.FirstOrDefault(c => c.OrderId == orderId && c.CustomerId > 0 && !c.Used);
        }



        #region Template


        public void InsertTemplate(CouponTemplate temp)
        {
            if (temp == null)
                throw new Exception("temp");
            _tempRepository.Insert(temp);
        }

        public void UpdateTemplate(CouponTemplate temp)
        {
            if (temp == null)
                throw new Exception("temp");
            _tempRepository.Update(temp);
        }

        public void DeleteTemplate(int tempId)
        {
            _tempRepository.Delete(tempId);
        }

        public CouponTemplate GetCouponTemplateById(int tempId)
        {
            return _tempRepository.Get(tempId);
        }

        public IPagedResult<CouponTemplate> GetAllTemplate(string keywords = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _tempRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(t => t.CouponTemplateName.Contains(keywords));

            query = query.OrderByDescending(t => t.Id);

            return new PagedResult<CouponTemplate>(query, pageIndex, pageSize);
        }

        #endregion

        #endregion
    }
}
