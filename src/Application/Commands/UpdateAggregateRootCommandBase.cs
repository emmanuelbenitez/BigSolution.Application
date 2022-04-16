using System.Diagnostics.CodeAnalysis;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Commands;

public abstract class UpdateAggregateRootCommandBase<TAggregate, TParameter> :
    RepositoryCommandBase<TAggregate>,
    ICommand<TParameter>, ICommandAsync<TParameter>
    where TAggregate : class, IAggregateRoot
{
    private readonly IMapper<TParameter, TAggregate> _mapper;

    protected UpdateAggregateRootCommandBase(IRepository<TAggregate> repository, IUnitOfWork unitOfWork,
        IMapper<TParameter, TAggregate> mapper) : base(
        repository,
        unitOfWork)
    {
        Requires.Argument(mapper, nameof(mapper))
            .IsNotNull()
            .Check();

        _mapper = mapper;
    }

    #region ICommand<TParameter> Members

    public void Execute(TParameter parameter)
    {
        var task = ExecuteAsync(parameter);
        task.Wait();
    }

    #endregion

    #region ICommandAsync<TParameter> Members

    public async Task ExecuteAsync(TParameter parameter)
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
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion

    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "Public API.")]
    protected virtual void Update(TAggregate aggregate, TParameter parameter)
    {
        _mapper.Map(parameter, aggregate);
    }

    protected abstract Task<TAggregate?> FindAggregateAsync(TParameter parameter);
}