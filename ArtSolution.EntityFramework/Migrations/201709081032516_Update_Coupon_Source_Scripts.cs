namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Coupon_Source_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "SourceOrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coupons", "SourceOrderId");
        }
    }
}
