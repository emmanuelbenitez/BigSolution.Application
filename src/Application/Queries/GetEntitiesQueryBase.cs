using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries;

public abstract class
    GetEntitiesQueryBase<TAggregate, TEntity, TResult> :
        RepositoryQueryBase<TAggregate, TEntity, TResult>,
        IQuery<IEnumerable<TResult>>,
        IQueryAsync<IEnumerable<TResult>>
    where TEntity : class, IEntity
    where TAggregate : class, IAggregateRoot
{
    protected GetEntitiesQueryBase(IRepository<TAggregate> repository, IMapper<TEntity?, TResult?> mapper)
        : base(repository, mapper)
    {
    }

    #region IQuery<IEnumerable<TResult>> Members

    public IEnumerable<TResult> Execute()
    {
        return ExecuteAsync().Result;
    }

    #endregion

    #region IQueryAsync<IEnumerable<TResult>> Members

    public async Task<IEnumerable<TResult>> ExecuteAsync()
    {
        return (await FilterAsync(Entities)).Select(Map)!;
    }

    #endregion

    protected abstract Task<IEnumerable<TEntity>> FilterAsync(IQueryable<TAggregate> entities);
}

public abstract class
    GetEntitiesQueryBase<TAggregate, TEntity, TResult, TParameter> :
        RepositoryQueryBase<TAggregate, TEntity, TResult>,
        IQuery<IEnumerable<TResult>, TParameter>,
        IQueryAsync<IEnumerable<TResult>, TParameter>
    where TEntity : class, IEntity
    where TAggregate : class, IAggregateRoot
    where TParameter : EntityFilter<TEntity>
{
    protected GetEntitiesQueryBase(IRepository<TAggregate> repository, IMapper<TEntity?, TResult?> mapper)
        : base(repository, mapper)
    {
    }

    #region IQuery<IEnumerable<TResult>,TParameter> Members

    public IEnumerable<TResult> Execute(TParameter parameter)
    {
        var task = ExecuteAsync(parameter);
        task.Wait();
        return task.Result;
    }

    #endregion

    #region IQueryAsync<IEnumerable<TResult>,TParameter> Members

    public async Task<IEnumerable<TResult>> ExecuteAsync(TParameter parameter)
    {
        return (await parameter.ApplyAsync(SelectEntities(Entities, parameter)))
            .AsEnumerable()
            .Select(Map)!;
    }

    #endregion

    protected abstract IQueryable<TEntity> SelectEntities(IQueryable<TAggregate> entities, TParameter parameter);
}