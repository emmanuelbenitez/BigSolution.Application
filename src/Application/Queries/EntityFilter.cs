using System.Linq.Expressions;
using BigSolution.Domain;

namespace BigSolution.Application.Queries;

public abstract class EntityFilter<TEntity> : IEntityFiler<TEntity>
    where TEntity : IEntity
{
    protected abstract IEnumerable<Expression<Func<TEntity, bool>>> Filters { get; }

    #region IEntityFiler<TEntity> Members

    public IQueryable<TEntity> Apply(IQueryable<TEntity> entities)
    {
        Requires.Argument(entities, nameof(entities))
            .IsNotNull()
            .Check();

        return Filters.Aggregate(entities, (current, filter) => current.Where(filter).AsQueryable());
    }

    #endregion

    public Task<IQueryable<TEntity>> ApplyAsync(IQueryable<TEntity> entities)
    {
        Requires.Argument(entities, nameof(entities))
            .IsNotNull()
            .Check();

        return Task.FromResult(Apply(entities));
    }
}