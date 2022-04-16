using BigSolution.Domain;

namespace BigSolution.Application.Commands;

public abstract class RepositoryCommandBase<TEntity>
    where TEntity : class, IAggregateRoot
{
    protected readonly IRepository<TEntity> Repository;

    protected readonly IUnitOfWork UnitOfWork;

    protected RepositoryCommandBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
    {
        Requires.Argument(repository, nameof(repository))
            .IsNotNull()
            .Check();
        Requires.Argument(unitOfWork, nameof(unitOfWork))
            .IsNotNull()
            .Check();

        Repository = repository;
        UnitOfWork = unitOfWork;
    }
}