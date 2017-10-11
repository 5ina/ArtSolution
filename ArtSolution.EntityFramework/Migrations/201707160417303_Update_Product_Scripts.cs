namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Market", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Market");
        }
    }
}
