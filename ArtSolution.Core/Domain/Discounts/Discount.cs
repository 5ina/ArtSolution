using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSolution.Domain.Discounts
{
    class Discount:Entity
    {
        /// <summary>
        /// 折扣类型
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 折扣类型标识符
        /// </summary>
        public int DiscountTypeId { get; set; }

        /// <summary>
        /// 是否使用百分比
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 最大折扣金额（使用UsePercentage的时）
        /// </summary>
        public decimal? MaximumDiscountAmount { get; set; }

        /// <summary>
        /// 折扣开始日期和时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 折扣结束日期和时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 指示折扣是否需要优惠券代码
        /// </summary>
        public bool RequiresCouponCode { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// 折扣是否可以与其他折扣同时使用（是否可以多个使用）
        /// </summary>
        public bool IsCumulative { get; set; }

        /// <summary>
        /// 折扣限制标识符
        /// </summary>
        public int DiscountLimitationId { get; set; }

        /// <summary>
        /// 折扣的限制次数
        /// </summary>
        public int LimitationTimes { get; set; }
        
        /// <summary>
        /// Gets or sets value indicating whether it should be applied to all subcategories or the selected one
        /// Used with "Assigned to categories" type only.
        /// </summary>
        public bool AppliedToSubCategories { get; set; }

        /// <summary>
        /// Gets or sets the discount type
        /// </summary>
        public DiscountType DiscountType
        {
            get
            {
                return (DiscountType)this.DiscountTypeId;
            }
            set
            {
                this.DiscountTypeId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the discount limitation
        /// </summary>
        public DiscountLimitationType DiscountLimitation
        {
            get
            {
                return (DiscountLimitationType)this.DiscountLimitationId;
            }
            set
            {
                this.DiscountLimitationId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the discount requirement
        /// </summary>
        public virtual ICollection<DiscountRequirement> DiscountRequirements
        {
            get { return _discountRequirements ?? (_discountRequirements = new List<DiscountRequirement>()); }
            protected set { _discountRequirements = value; }
        }
        /// <summary>
        /// Gets or sets the categories
        /// </summary>
        public virtual ICollection<Category> AppliedToCategories
        {
            get { return _appliedToCategories ?? (_appliedToCategories = new List<Category>()); }
            protected set { _appliedToCategories = value; }
        }
        /// <summary>
        /// Gets or sets the categories
        /// </summary>
        public virtual ICollection<Manufacturer> AppliedToManufacturers
        {
            get { return _appliedToManufacturers ?? (_appliedToManufacturers = new List<Manufacturer>()); }
            protected set { _appliedToManufacturers = value; }
        }
        /// <summary>
        /// Gets or sets the products 
        /// </summary>
        public virtual ICollection<Product> AppliedToProducts
        {
            get { return _appliedToProducts ?? (_appliedToProducts = new List<Product>()); }
            protected set { _appliedToProducts = value; }
        }
    }
}
