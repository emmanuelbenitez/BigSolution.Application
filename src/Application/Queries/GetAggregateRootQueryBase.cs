using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries;

public abstract class GetAggregateRootQueryBase<TAggregate, TResult, TParameter> :
    GetEntityQueryBase<TAggregate, TAggregate, TResult, TParameter>
    where TAggregate : class, IAggregateRoot
{
    protected GetAggregateRootQueryBase(IRepository<TAggregate> repository, IMapper<TAggregate?, TResult?> mapper)
        : base(repository, mapper)
    {
    }
}