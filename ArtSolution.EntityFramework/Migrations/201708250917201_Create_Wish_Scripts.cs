namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Wish_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        BrandName = c.String(nullable: false, maxLength: 60),
                        ProductName = c.String(nullable: false, maxLength: 160),
                        ProductImages = c.String(nullable: false, maxLength: 500),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WishOrders");
        }
    }
}
