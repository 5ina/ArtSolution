namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "PreSell", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "PreSell");
        }
    }
}
