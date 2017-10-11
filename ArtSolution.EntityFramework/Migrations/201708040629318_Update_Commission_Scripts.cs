namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Commission_Scripts : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Commissions", "StatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Commissions", "StatusId", c => c.Int(nullable: false));
        }
    }
}
