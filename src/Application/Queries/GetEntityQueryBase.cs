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
using System.Threading.Tasks;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Queries
{
    public abstract class GetEntityQueryBase<TAggregate, TEntity, TResult, TParameter> :
        RepositoryQueryBase<TAggregate, TEntity, TResult>,
        IQuery<TResult, TParameter>,
        IQueryAsync<TResult, TParameter>
        where TAggregate : class, IAggregateRoot
        where TEntity : class, IEntity
    {
        protected GetEntityQueryBase(IRepository<TAggregate> repository, IMapper<TEntity, TResult> mapper)
            : base(repository, mapper) { }

        #region IQuery<TResult,TParameter> Members

        public TResult Execute(TParameter parameter)
        {
            var task = ExecuteAsync(parameter);
            task.Wait();
            return task.Result;
        }

        #endregion

        #region IQueryAsync<TResult,TParameter> Members

        public async Task<TResult> ExecuteAsync(TParameter parameter)
        {
            return Map(await FilterAsync(Entities, parameter));
        }

        #endregion

        protected abstract Task<TEntity> FilterAsync(IQueryable<TAggregate> entities, TParameter parameter);
    }
}
