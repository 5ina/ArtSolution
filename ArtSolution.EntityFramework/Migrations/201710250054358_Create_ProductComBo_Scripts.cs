namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_ProductComBo_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComBoProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        ComBoProductImage = c.String(maxLength: 500),
                        StockQuantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Market = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComBoProductMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComBoId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ComBoProductMappings");
            DropTable("dbo.ComBoProducts");
        }
    }
}
