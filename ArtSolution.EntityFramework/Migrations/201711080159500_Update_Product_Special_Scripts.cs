namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Special_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SpecialMaxQuantity", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SpecialMaxQuantity");
        }
    }
}
