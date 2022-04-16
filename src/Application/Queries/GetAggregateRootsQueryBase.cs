using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries;

public abstract class
    GetAggregateRootsQueryBase<TAggregate, TResult> : GetEntitiesQueryBase<TAggregate, TAggregate, TResult>
    where TAggregate : class, IAggregateRoot
{
    protected GetAggregateRootsQueryBase(IRepository<TAggregate> repository, IMapper<TAggregate?, TResult?> mapper)
        : base(repository, mapper)
    {
    }
}

public abstract class
    GetAggregateRootsQueryBase<TAggregate, TResult, TParameter> : GetEntitiesQueryBase<TAggregate, TAggregate, TResult,
        TParameter>
    where TAggregate : class, IAggregateRoot
    where TParameter : EntityFilter<TAggregate>
{
    protected GetAggregateRootsQueryBase(IRepository<TAggregate> repository, IMapper<TAggregate?, TResult?> mapper)
        : base(repository, mapper)
    {
    }

    #region Base Class Member Overrides

    protected sealed override IQueryable<TAggregate> SelectEntities(IQueryable<TAggregate> entities,
        TParameter parameter)
    {
        return Prepare(entities);
    }

    #endregion

    protected virtual IQueryable<TAggregate> Prepare(IQueryable<TAggregate> entities)
    {
        return entities;
    }
}