using System.Diagnostics.CodeAnalysis;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Commands;

public abstract class UpdateAggregateRootCommandBase<TAggregate, TModel> : RepositoryCommandBase<TAggregate>,
    ICommand<TModel, TModel>, ICommandAsync<TModel, TModel>
    where TAggregate : class, IAggregateRoot
{
    private readonly IMapper<TModel, TAggregate> _entityMapper;
    private readonly IMapper<TAggregate, TModel> _modelMapper;

    protected UpdateAggregateRootCommandBase(
        IRepository<TAggregate> repository,
        IUnitOfWork unitOfWork,
        IMapper<TModel, TAggregate> entityMapper,
        IMapper<TAggregate, TModel> modelMapper)
        : base(repository, unitOfWork)
    {
        Requires.Argument(entityMapper, nameof(entityMapper))
            .IsNotNull()
            .Check();

        _entityMapper = entityMapper;
        _modelMapper = modelMapper;
    }

    #region ICommand<TParameter> Members

    public TModel Execute(TModel parameter)
    {
        var task = ExecuteAsync(parameter);
        task.Wait();
        return task.Result;
    }

    #endregion

    #region ICommandAsync<TParameter> Members

    public async Task<TModel> ExecuteAsync(TModel parameter)
    {
        Requires.Argument(parameter, nameof(parameter))
            .IsNotNull()
            .Check();

        using var transaction = UnitOfWork.BeginTransaction();
        try
        {
            var aggregate = await FindAggregateAsync(parameter);

            Ensures.NotNull(aggregate, "The aggregate does not exist.");

            Update(aggregate!, parameter);

            await UnitOfWork.SaveAsync();
            await transaction.CommitAsync();
            return _modelMapper.Map(aggregate!);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion

    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "Public API.")]
    protected virtual void Update(TAggregate aggregate, TModel parameter)
    {
        _entityMapper.Map(parameter, aggregate);
    }

    protected abstract Task<TAggregate?> FindAggregateAsync(TModel parameter);
}