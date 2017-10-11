using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace ArtSolution.EntityFramework.Repositories
{
    public abstract class ArtSolutionRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ArtSolutionDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ArtSolutionRepositoryBase(IDbContextProvider<ArtSolutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ArtSolutionRepositoryBase<TEntity> : ArtSolutionRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ArtSolutionRepositoryBase(IDbContextProvider<ArtSolutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
