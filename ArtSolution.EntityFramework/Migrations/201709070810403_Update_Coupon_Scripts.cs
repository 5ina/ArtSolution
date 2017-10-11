namespace ArtSolution.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Coupon_Scripts : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Effective = c.DateTime(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Used = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        DiscountCode = c.String(),
                        LastModificationTime = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Coupon_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.Coupons", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "DiscountCode", c => c.String());
            DropColumn("dbo.Coupons", "Discount");
            DropColumn("dbo.Coupons", "SystemName");
            DropColumn("dbo.Coupons", "Promotion");
            DropColumn("dbo.Coupons", "MaxCount");
            DropColumn("dbo.Coupons", "Enabled");
            DropColumn("dbo.Coupons", "IsDeleted");
            DropColumn("dbo.Coupons", "LastModifierUserId");
            DropColumn("dbo.Coupons", "CreatorUserId");
            DropTable("dbo.CouponLists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CouponLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CouponId = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        CustomerId = c.Int(nullable: false),
                        DiscountCode = c.String(),
                        OrderId = c.Int(nullable: false),
                        Used = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Coupons", "CreatorUserId", c => c.Long());
            AddColumn("dbo.Coupons", "LastModifierUserId", c => c.Long());
            AddColumn("dbo.Coupons", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Coupons", "Enabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Coupons", "MaxCount", c => c.Int(nullable: false));
            AddColumn("dbo.Coupons", "Promotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Coupons", "SystemName", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Coupons", "Discount", c => c.Int(nullable: false));
            DropColumn("dbo.Coupons", "DiscountCode");
            DropColumn("dbo.Coupons", "OrderId");
            DropColumn("dbo.Coupons", "CustomerId");
            AlterTableAnnotations(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Effective = c.DateTime(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Used = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        DiscountCode = c.String(),
                        LastModificationTime = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Coupon_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
    }
}
