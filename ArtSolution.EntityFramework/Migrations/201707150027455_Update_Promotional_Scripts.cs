namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Promotional_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotionals", "Image", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotionals", "Image");
        }
    }
}
