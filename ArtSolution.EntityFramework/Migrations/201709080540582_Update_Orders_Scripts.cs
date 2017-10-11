namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Orders_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CouponId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Preferential", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Preferential");
            DropColumn("dbo.Orders", "CouponId");
        }
    }
}
