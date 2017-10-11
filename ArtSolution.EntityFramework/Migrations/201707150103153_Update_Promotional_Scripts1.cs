namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Promotional_Scripts1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotionals", "PromotionalImage", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.Promotionals", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promotionals", "Image", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.Promotionals", "PromotionalImage");
        }
    }
}
