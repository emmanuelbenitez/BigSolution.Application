using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Commands;

public abstract class CreateAggregateCommandBase<TEntity, TParameter, TResult> : RepositoryCommandBase<TEntity>,
    ICommand<TParameter, TResult>,
    ICommandAsync<TParameter, TResult>
    where TEntity : class, IAggregateRoot
{
    private readonly IMapper<TEntity, TResult> _mapper;

    protected CreateAggregateCommandBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork,
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
            var entity = await CreateEntityAsync(parameter);

            Repository.Add(entity);

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

    protected abstract Task<TEntity> CreateEntityAsync(TParameter parameter);
}

public abstract class CreateAggregateCommandBase<TEntity, TModel> : CreateAggregateCommandBase<TEntity, TModel, TModel>
    where TEntity : class, IAggregateRoot
{
    private readonly IMapper<TModel, TEntity> _entityMapper;

    protected CreateAggregateCommandBase(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IMapper<TEntity, TModel> modelMapper,
        IMapper<TModel, TEntity> entityMapper)
        : base(repository, unitOfWork, modelMapper)
    {
        Requires.Argument(entityMapper, nameof(entityMapper))
            .IsNotNull()
            .Check();

        _entityMapper = entityMapper;
    }

    #region Base Class Member Overrides

    protected sealed override Task<TEntity> CreateEntityAsync(TModel parameter)
    {
        var entity = _entityMapper.Map(parameter);
        return Task.FromResult(entity);
    }

    #endregion
}