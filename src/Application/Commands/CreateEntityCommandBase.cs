using BigSolution.Domain;
using BigSolution.Infra.Mapping;
using JetBrains.Annotations;

namespace BigSolution.Application.Commands;

[UsedImplicitly]
public abstract class CreateEntityCommandBase<TEntity, TAggregateRoot, TParameter, TResult> :
    RepositoryCommandBase<TAggregateRoot>,
    ICommand<TParameter, TResult>,
    ICommandAsync<TParameter, TResult>
    where TAggregateRoot : class, IAggregateRoot
    where TEntity : IEntity
{
    private readonly IMapper<TEntity, TResult> _mapper;

    protected CreateEntityCommandBase(IRepository<TAggregateRoot> repository, IUnitOfWork unitOfWork,
        IMapper<TEntity, TResult> mapper)
        : base(repository, unitOfWork)
    {
        Requires.Argument(mapper, nameof(mapper))
            .IsNotNull()
            .Check();

        _mapper = mapper;
    }

    #region ICommand<TParameter,TResult> Members

    public TResult Execute(TParameter parameter)
    {
        var task = ExecuteAsync(parameter);
        task.Wait();

        return task.Result;
    }

    #endregion

    #region ICommandAsync<TParameter,TResult> Members

    public async Task<TResult> ExecuteAsync(TParameter parameter)
    {
        Requires.Argument(parameter, nameof(parameter))
            .IsNotNull()
            .Check();

        using var transaction = UnitOfWork.BeginTransaction();
        try
        {
            var aggregateRoot = FinAggregateRoot(parameter);
            Ensures.NotNull(aggregateRoot, $"The root aggregate '{typeof(TAggregateRoot).Name}' is not found");
            var entity = CreateEntity(parameter, aggregateRoot!);

            await UnitOfWork.SaveAsync();
            await transaction.CommitAsync();

            return _mapper.Map(entity);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion

    protected abstract TAggregateRoot? FinAggregateRoot(TParameter parameter);

    protected abstract TEntity CreateEntity(TParameter parameter, TAggregateRoot aggregateRoot);
}