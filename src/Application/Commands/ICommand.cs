using System.Diagnostics.CodeAnalysis;

namespace BigSolution.Application.Commands;

public interface ICommand
{
}

public interface ICommand<in TParameter> : ICommand
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
    void Execute(TParameter parameter);
}

public interface ICommand<in TParameter, out TResult> : ICommand
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API")]
    TResult Execute(TParameter parameter);
}