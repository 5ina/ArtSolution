namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateArtsolutionScritps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnOrders", "AuditStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ReturnOrders", "AuditReason", c => c.String(maxLength: 50));
            AlterColumn("dbo.Products", "ProductCode", c => c.String(maxLength: 20));
            DropColumn("dbo.Orders", "DeliveryId");
            DropColumn("dbo.Orders", "DeliveryCode");
            DropColumn("dbo.ReturnOrders", "ExpressCode");
            DropColumn("dbo.ReturnOrders", "ExpressName");
            DropTable("dbo.Deliveries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Active = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ReturnOrders", "ExpressName", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.ReturnOrders", "ExpressCode", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Orders", "DeliveryCode", c => c.String(maxLength: 50));
            AddColumn("dbo.Orders", "DeliveryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "ProductCode", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.ReturnOrders", "AuditReason");
            DropColumn("dbo.ReturnOrders", "AuditStatus");
        }
    }
}
