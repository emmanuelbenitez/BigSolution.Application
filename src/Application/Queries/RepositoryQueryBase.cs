using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries;

public abstract class RepositoryQueryBase<TAggregate, TEntity, TResult>
    where TAggregate : class, IAggregateRoot
    where TEntity : class, IEntity
{
    private readonly IMapper<TEntity?, TResult?> _mapper;

    private readonly IRepository<TAggregate> _repository;

    protected RepositoryQueryBase(IRepository<TAggregate> repository, IMapper<TEntity?, TResult?> mapper)
    {
        Requires.Argument(repository, nameof(repository))
            .IsNotNull()
            .Check();
        Requires.Argument(mapper, nameof(mapper))
            .IsNotNull()
            .Check();

        _repository = repository;
        _mapper = mapper;
    }

    protected IQueryable<TAggregate> Entities => _repository.Entities;

    protected TResult? Map(TEntity? entity)
    {
        return _mapper.Map(entity);
    }
}