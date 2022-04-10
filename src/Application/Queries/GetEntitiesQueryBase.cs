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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries
{
    public abstract class
        GetEntitiesQueryBase<TAggregate, TEntity, TResult> :
            RepositoryQueryBase<TAggregate, TEntity, TResult>,
            IQuery<IEnumerable<TResult>>,
            IQueryAsync<IEnumerable<TResult>>
        where TEntity : class, IEntity
        where TAggregate : class, IAggregateRoot
    {
        protected GetEntitiesQueryBase(IRepository<TAggregate> repository, IMapper<TEntity, TResult> mapper)
            : base(repository, mapper) { }

        #region IQuery<IEnumerable<TResult>> Members

        public IEnumerable<TResult> Execute()
        {
            return ExecuteAsync().Result;
        }

        #endregion

        #region IQueryAsync<IEnumerable<TResult>> Members

        public async Task<IEnumerable<TResult>> ExecuteAsync()
        {
            return (await FilterAsync(Entities))
                .Select(Map);
        }

        #endregion

        protected abstract Task<IEnumerable<TEntity>> FilterAsync(IQueryable<TAggregate> entities);
    }

    public abstract class
        GetEntitiesQueryBase<TAggregate, TEntity, TResult, TParameter> :
            RepositoryQueryBase<TAggregate, TEntity, TResult>,
            IQuery<IEnumerable<TResult>, TParameter>,
            IQueryAsync<IEnumerable<TResult>, TParameter>
        where TEntity : class, IEntity
        where TAggregate : class, IAggregateRoot
        where TParameter : EntityFilter<TEntity>
    {
        protected GetEntitiesQueryBase(IRepository<TAggregate> repository, IMapper<TEntity, TResult> mapper)
            : base(repository, mapper) { }

        #region IQuery<IEnumerable<TResult>,TParameter> Members

        public IEnumerable<TResult> Execute(TParameter parameter)
        {
            var task = ExecuteAsync(parameter);
            task.Wait();
            return task.Result;
        }

        #endregion

        #region IQueryAsync<IEnumerable<TResult>,TParameter> Members

        public async Task<IEnumerable<TResult>> ExecuteAsync(TParameter parameter)
        {
            return (await parameter.ApplyAsync(SelectEntities(Entities, parameter)))
                .AsEnumerable()
                .Select(Map);
        }

        #endregion

        protected abstract IQueryable<TEntity> SelectEntities(IQueryable<TAggregate> entities, TParameter parameter);
    }
}
