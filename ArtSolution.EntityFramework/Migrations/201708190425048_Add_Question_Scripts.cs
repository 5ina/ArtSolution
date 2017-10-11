namespace ArtSolution.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Question_Scripts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionNaireId = c.Int(nullable: false),
                        QuestionTitle = c.String(nullable: false, maxLength: 60),
                        AttributeTypeId = c.Int(nullable: false),
                        CustomValue = c.String(maxLength: 100),
                        IsAdvance = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionNaires",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Description = c.String(maxLength: 120),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QuestionNaire_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.ReturnOrders", "OrderItemId");
            DropTable("dbo.Loans");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.ReturnOrders", "OrderItemId", c => c.Int(nullable: false));
            DropTable("dbo.QuestionOptions");
            DropTable("dbo.QuestionNaires",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QuestionNaire_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Questions");
        }
    }
}
