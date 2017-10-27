namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ComBoProduct_Scripts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComBoProducts", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComBoProducts", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
