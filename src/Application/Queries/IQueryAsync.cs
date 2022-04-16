namespace BigSolution.Application.Queries;

public interface IQueryAsync<TResult> : IQuery
{
    Task<TResult> ExecuteAsync();
}

public interface IQueryAsync<TResult, in TParameter> : IQuery
{
    Task<TResult> ExecuteAsync(TParameter parameter);
}