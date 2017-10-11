namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_SignLog_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        NumberEntries = c.Int(nullable: false),
                        Reward = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SignLogs");
        }
    }
}
