using System.Diagnostics.CodeAnalysis;

namespace BigSolution.Application.Queries;

public interface IQuery
{
}

public interface IQuery<out TResult> : IQuery
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    TResult Execute();
}

public interface IQuery<out TResult, in TParameter> : IQuery
{
    TResult Execute(TParameter parameter);
}