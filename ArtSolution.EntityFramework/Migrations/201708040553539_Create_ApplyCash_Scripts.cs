namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_ApplyCash_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplyCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Allowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Audit = c.Int(nullable: false),
                        AuditReason = c.String(maxLength: 80),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplyCashes");
        }
    }
}
