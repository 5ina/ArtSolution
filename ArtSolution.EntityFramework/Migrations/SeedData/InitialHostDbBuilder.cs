using ArtSolution.EntityFramework;
using EntityFramework.DynamicFilters;

namespace ArtSolution.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly ArtSolutionDbContext _context;

        public InitialHostDbBuilder(ArtSolutionDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();
            new DefaultCustomerData(_context).Create();
            new DefaultCatalogData(_context).Create();
        }
    }
}
