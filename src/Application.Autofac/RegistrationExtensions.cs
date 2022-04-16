using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using BigSolution.Application.Commands;
using BigSolution.Application.Queries;

namespace BigSolution.Application;

public static class RegistrationExtensions
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsQuery<TLimit,
        TScanningActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
        where TScanningActivatorData : ScanningActivatorData
    {
        Requires.Argument(registration, nameof(registration))
            .IsNotNull()
            .Check();
        return registration.Where(t => t.Is<IQuery>());
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsCommand<TLimit,
        TScanningActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
        where TScanningActivatorData : ScanningActivatorData
    {
        Requires.Argument(registration, nameof(registration))
            .IsNotNull()
            .Check();
        return registration.Where(t => t.Is<ICommand>());
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
    public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsCommandOrQuery<TLimit,
        TScanningActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
        where TScanningActivatorData : ScanningActivatorData
    {
        Requires.Argument(registration, nameof(registration))
            .IsNotNull()
            .Check();
        return registration.Where(t => t.Is<ICommand>() || t.Is<IQuery>());
    }
}