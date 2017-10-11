namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Order_Pre_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "PreSell", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShoppingCartItems", "PreSell", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingCartItems", "PreSell");
            DropColumn("dbo.OrderItems", "PreSell");
        }
    }
}
