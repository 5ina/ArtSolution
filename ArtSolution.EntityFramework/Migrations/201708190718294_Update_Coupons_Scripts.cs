namespace ArtSolution.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Coupons_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "SystemName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Coupons", "Name", c => c.String(nullable: false, maxLength: 100));
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionNaires",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_QuestionNaire_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.QuestionOptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 20),
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
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionNaireId = c.Int(nullable: false),
                        QuestionTitle = c.String(nullable: false, maxLength: 60),
                        AttributeTypeId = c.Int(nullable: false),
                        CustomValue = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Coupons", "Name", c => c.String());
            DropColumn("dbo.Coupons", "SystemName");
        }
    }
}
