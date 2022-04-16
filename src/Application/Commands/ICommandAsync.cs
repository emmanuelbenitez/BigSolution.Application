namespace BigSolution.Application.Commands;

public interface ICommandAsync<in TParameter> : ICommand
{
    Task ExecuteAsync(TParameter parameter);
}

public interface ICommandAsync<in TParameter, TResult> : ICommand
{
    Task<TResult> ExecuteAsync(TParameter parameter);
}