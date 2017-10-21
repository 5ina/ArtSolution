namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Product_Reward_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AllowReward", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AllowReward");
        }
    }
}
