using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries;

public abstract class GetEntityQueryBase<TAggregate, TEntity, TResult, TParameter> :
    RepositoryQueryBase<TAggregate, TEntity, TResult>,
    IQuery<TResult?, TParameter>,
    IQueryAsync<TResult?, TParameter>
    where TAggregate : class, IAggregateRoot
    where TEntity : class, IEntity
{
    protected GetEntityQueryBase(IRepository<TAggregate> repository, IMapper<TEntity?, TResult?> mapper)
        : base(repository, mapper)
    {
    }

    #region IQuery<TResult,TParameter> Members

    public TResult? Execute(TParameter parameter)
    {
        var task = ExecuteAsync(parameter);
        task.Wait();
        return task.Result;
    }

    #endregion

    #region IQueryAsync<TResult,TParameter> Members

    public async Task<TResult?> ExecuteAsync(TParameter parameter)
    {
        return Map(await FilterAsync(Entities, parameter));
    }

    #endregion

    protected abstract Task<TEntity?> FilterAsync(IQueryable<TAggregate> entities, TParameter parameter);
}