namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Related_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "RelatedProductIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "RelatedProductIds");
        }
    }
}
