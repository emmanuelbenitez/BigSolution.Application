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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BigSolution.Domain;

namespace BigSolution.Application.Queries
{
    public abstract class EntityFilter<TEntity> : IEntityFiler<TEntity>
        where TEntity : IEntity
    {
        #region IEntityFiler<TEntity> Members

        public IQueryable<TEntity> Apply(IQueryable<TEntity> entities)
        {
            Requires.Argument(entities, nameof(entities))
                .IsNotNull()
                .Check();

            return Filters.Aggregate(entities, (current, filter) => current.Where(filter).AsQueryable());
        }

        #endregion

        protected abstract IEnumerable<Expression<Func<TEntity, bool>>> Filters { get; }

        public Task<IQueryable<TEntity>> ApplyAsync(IQueryable<TEntity> entities)
        {
            Requires.Argument(entities, nameof(entities))
                .IsNotNull()
                .Check();

            return Task.FromResult(Apply(entities));
        }
    }
}
