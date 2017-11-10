namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Delete_ProductAttribute_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SpecialQuantity", c => c.Int(nullable: false));
            DropColumn("dbo.OrderItems", "ProductAttributeId");
            DropColumn("dbo.Products", "SpecialMaxQuantity");
            DropColumn("dbo.ShoppingCartItems", "ProductAttributeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShoppingCartItems", "ProductAttributeId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "SpecialMaxQuantity", c => c.Int());
            AddColumn("dbo.OrderItems", "ProductAttributeId", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "SpecialQuantity");
        }
    }
}
