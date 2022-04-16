using BigSolution.Domain;

namespace BigSolution.Application.Commands;

public abstract class DeleteAggregateRootCommandBase<TAggregateRoot, TParameter> :
    RepositoryCommandBase<TAggregateRoot>, ICommand<TParameter>,
    ICommandAsync<TParameter>
    where TAggregateRoot : class, IAggregateRoot
{
    protected DeleteAggregateRootCommandBase(IRepository<TAggregateRoot> repository, IUnitOfWork unitOfWork) : base(
        repository, unitOfWork)
    {
    }

    #region ICommand<TParameter> Members

    public void Execute(TParameter parameter)
    {
        ExecuteAsync(parameter).Wait();
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
            Repository.Delete(aggregate!);

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

    protected abstract Task<TAggregateRoot?> FindAggregateAsync(TParameter parameter);
}