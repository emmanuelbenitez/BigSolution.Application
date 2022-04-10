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
    public abstract class RepositoryQueryBase<TAggregate, TEntity, TResult>
        where TAggregate : class, IAggregateRoot
        where TEntity : class, IEntity
    {
        protected RepositoryQueryBase(IRepository<TAggregate> repository, IMapper<TEntity, TResult> mapper)
        {
            Requires.Argument(repository, nameof(repository))
                .IsNotNull()
                .Check();
            Requires.Argument(mapper, nameof(mapper))
                .IsNotNull()
                .Check();

            _repository = repository;
            _mapper = mapper;
        }

        protected IQueryable<TAggregate> Entities => _repository.Entities;

        protected TResult Map(TEntity entity)
        {
            return _mapper.Map(entity);
        }

        private readonly IMapper<TEntity, TResult> _mapper;

        private readonly IRepository<TAggregate> _repository;
    }
}
