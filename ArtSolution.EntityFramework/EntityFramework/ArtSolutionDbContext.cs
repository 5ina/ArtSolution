using Abp.EntityFramework;
using ArtSolution.Domain.Catalog;
using ArtSolution.Domain.Common;
using ArtSolution.Domain.Configuration;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.Discounts;
using ArtSolution.Domain.Messages;
using ArtSolution.Domain.News;
using ArtSolution.Domain.official;
using ArtSolution.Domain.Orders;
using System.Data.Entity;

namespace ArtSolution.EntityFramework
{
    public class ArtSolutionDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }
        
        public virtual IDbSet<ProductAttribute> ProductAttribute { get; set; }

        public virtual IDbSet<Product> Product { get; set; }
        public virtual IDbSet<ProductImage> ProductImage { get; set; }

        public virtual IDbSet<Category> Category { get; set; }
        //品牌
        public virtual IDbSet<Brand> Brand { get; set; }

        public virtual IDbSet<ProductReview> ProductReview { get; set; }
        public virtual IDbSet<ProductTag> ProductTag { get; set; }
        public virtual IDbSet<ProductTagMapping> ProductTagMapping { get; set; }
        


        /* 用户模块 */
        public virtual IDbSet<Customer> Customer { get; set; }
        public virtual IDbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual IDbSet<CustomerAttribute> CustomerAttribute { get; set; }
        public virtual IDbSet<CustomerReward> CustomerReward { get; set; }

        public virtual IDbSet<ApplyPromoter> ApplyPromoter { get; set; }
        public virtual IDbSet<ApplyCash> ApplyCash { get; set; }
        public virtual IDbSet<SignLog> SignLog { get; set; }


        public virtual IDbSet<Notice> Notice { get; set; }
        

        public virtual IDbSet<Setting> Setting { get; set; }
        //News
        public virtual IDbSet<NewItem> NewItem { get; set; }
        public virtual IDbSet<Topic> Topic { get; set; }
        public virtual IDbSet<Promotional> Promotional { get; set; }
        public virtual IDbSet<Loan> Loan { get; set; }
        public virtual IDbSet<WishOrder> WishOrder { get; set; }
        
        //Common
        public virtual IDbSet<Advert> Advert { get; set; }

        //orders

        public virtual IDbSet<Order> Order { get; set; }
        public virtual IDbSet<OrderItem> OrderItem { get; set; }
        public virtual IDbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        //public virtual IDbSet<Delivery> Delivery { get; set; }
        public virtual IDbSet<Favorite> Favorite { get; set; }
        public virtual IDbSet<ReturnOrder> ReturnOrder { get; set; }
        public virtual IDbSet<Commission> Commission { get; set; }

        public virtual IDbSet<PaymentRecord> PaymentRecord { get; set; }

        //Discount 优惠券
        public virtual IDbSet<Coupon> Coupon { get; set; }
        public virtual IDbSet<CouponTemplate> CouponTemplate { get; set; }
        

        //Official
        public virtual IDbSet<MessageBoard> MessageBoard { get; set; }

        public ArtSolutionDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in ArtSolutionDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of ArtSolutionDbContext since ABP automatically handles it.
         */
        public ArtSolutionDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
