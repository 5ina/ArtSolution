using ArtSolution.Migrations.SeedData;
using System.Data.Entity.Migrations;

namespace ArtSolution.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ArtSolution.EntityFramework.ArtSolutionDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ArtSolution";
        }

        protected override void Seed(ArtSolution.EntityFramework.ArtSolutionDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
            new InitialHostDbBuilder(context).Create();
        }
    }
}
