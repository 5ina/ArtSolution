using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ArtSolution.Domain.Discounts;

namespace ArtSolution.Discount
{
    /// <summary>
    /// 优惠券服务接口
    /// </summary>
    public interface ICouponService : IApplicationService
    {
        #region  Coupon Template

        void InsertTemplate(CouponTemplate temp);

        void UpdateTemplate(CouponTemplate temp);

        void DeleteTemplate(int tempId);

        CouponTemplate GetCouponTemplateById(int tempId);

        IPagedResult<CouponTemplate> GetAllTemplate(string keywords = null, int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion

        #region Coupon
        /// <summary>
        /// 创建新的优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        int InsertCoupon(Coupon coupon);

        /// <summary>
        /// 更新优惠券
        /// </summary>
        /// <param name="coupon"></param>
        void UpdateCoupon(Coupon coupon);

        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="couponId"></param>
        void DeleteCoupon(int couponId);

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        Coupon GetCouponById(int couponId);

        Coupon GetCouponByCode(string code);
        Coupon GetCouponByOrderId(int orderId);        

        /// <summary>
        /// 获取所有的优惠券
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <param name="used"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedResult<Coupon> GetAllCoupons(string keywords = "", int customerId = 0,
            int orderId=0,bool? used = null,
            int pageIndex = 0, int pageSize = int.MaxValue);


        #endregion
        
    }
}
