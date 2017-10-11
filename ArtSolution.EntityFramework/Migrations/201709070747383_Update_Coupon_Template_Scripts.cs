namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Coupon_Template_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CouponTemplates", "EffectiveDays", c => c.Int(nullable: false));
            AddColumn("dbo.CouponTemplates", "IsCurrentDate", c => c.Boolean(nullable: false));
            AddColumn("dbo.CouponTemplates", "CouponTemplateName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CouponTemplates", "StartTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CouponTemplates", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.CouponTemplates", "CouponTemplateName");
            DropColumn("dbo.CouponTemplates", "IsCurrentDate");
            DropColumn("dbo.CouponTemplates", "EffectiveDays");
        }
    }
}
