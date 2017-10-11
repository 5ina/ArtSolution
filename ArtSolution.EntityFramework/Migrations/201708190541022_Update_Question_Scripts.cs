namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Question_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionOptions", "IsAdvance", c => c.Boolean(nullable: false));
            DropColumn("dbo.Questions", "IsAdvance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "IsAdvance", c => c.Boolean(nullable: false));
            DropColumn("dbo.QuestionOptions", "IsAdvance");
        }
    }
}
