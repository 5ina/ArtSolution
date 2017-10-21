namespace ArtSolution.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Order_Reward_Scripts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsRewardOrder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsRewardOrder");
        }
    }
}
