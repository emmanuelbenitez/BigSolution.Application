#region Copyright & License

// Copyright © 2020 - 2021 Emmanuel Benitez
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Linq;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries
{
    public abstract class GetAggregateRootsQueryBase<TAggregate, TResult> : GetEntitiesQueryBase<TAggregate, TAggregate, TResult>
        where TAggregate : class, IAggregateRoot
    {
        protected GetAggregateRootsQueryBase(IRepository<TAggregate> repository, IMapper<TAggregate, TResult> mapper)
            : base(repository, mapper) { }
    }

    public abstract class GetAggregateRootsQueryBase<TAggregate, TResult, TParameter> : GetEntitiesQueryBase<TAggregate, TAggregate, TResult, TParameter>
        where TAggregate : class, IAggregateRoot
        where TParameter : EntityFilter<TAggregate>
    {
        protected GetAggregateRootsQueryBase(IRepository<TAggregate> repository, IMapper<TAggregate, TResult> mapper)
            : base(repository, mapper) { }

        #region Base Class Member Overrides

        protected sealed override IQueryable<TAggregate> SelectEntities(IQueryable<TAggregate> entities, TParameter parameter)
        {
            return Prepare(entities);
        }

        #endregion

        protected virtual IQueryable<TAggregate> Prepare(IQueryable<TAggregate> entities)
        {
            return entities;
        }
    }
}
