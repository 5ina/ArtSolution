namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Loan_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        OrderTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReturnAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusId = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Mobile = c.String(nullable: false, maxLength: 11),
                        Sex = c.Int(nullable: false),
                        SchoolName = c.String(nullable: false, maxLength: 30),
                        Location = c.String(maxLength: 200),
                        IdCardPositive = c.String(),
                        IdCardSide = c.String(),
                        Quota = c.Int(nullable: false),
                        Cycle = c.Int(nullable: false),
                        Audit = c.Int(nullable: false),
                        AuditReason = c.String(maxLength: 200),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Loans");
            DropTable("dbo.Commissions");
        }
    }
}
