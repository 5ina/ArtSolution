using ArtSolution.Domain.Customers;
using ArtSolution.EntityFramework;
using System;
using System.Linq;

namespace ArtSolution.Migrations.SeedData
{
    public class DefaultCatalogData
    {
        private readonly ArtSolutionDbContext _context;

        public DefaultCatalogData(ArtSolutionDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
        }
        
        
    }
}
