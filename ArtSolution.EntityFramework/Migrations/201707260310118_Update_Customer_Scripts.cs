namespace ArtSolution.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Customer_Scripts : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mobile = c.String(maxLength: 15),
                        NickName = c.String(maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 60),
                        OpenId = c.String(maxLength: 60),
                        CustomerRoleId = c.Int(nullable: false),
                        PasswordSalt = c.String(nullable: false, maxLength: 6),
                        LastModificationTime = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                        VerificationCode = c.String(maxLength: 200),
                        Promoter = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Customer_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.Customers", "Promoter", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "IsDeleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Customers", "Promoter");
            AlterTableAnnotations(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mobile = c.String(maxLength: 15),
                        NickName = c.String(maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 60),
                        OpenId = c.String(maxLength: 60),
                        CustomerRoleId = c.Int(nullable: false),
                        PasswordSalt = c.String(nullable: false, maxLength: 6),
                        LastModificationTime = c.DateTime(),
                        CreationTime = c.DateTime(nullable: false),
                        VerificationCode = c.String(maxLength: 200),
                        Promoter = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Customer_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
    }
}
