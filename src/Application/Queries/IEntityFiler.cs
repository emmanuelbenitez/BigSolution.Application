using System.Diagnostics.CodeAnalysis;
using BigSolution.Domain;

namespace BigSolution.Application.Queries;

public interface IEntityFiler<TEntity>
    where TEntity : IEntity
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    IQueryable<TEntity> Apply(IQueryable<TEntity> entities);
}