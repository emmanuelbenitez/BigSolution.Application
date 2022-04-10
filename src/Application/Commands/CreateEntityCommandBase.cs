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
using System.Threading.Tasks;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Commands
{
    public abstract class CreateEntityCommandBase<TEntity, TAggregateRoot, TParameter, TResult> : RepositoryCommandBase<TAggregateRoot>,
        ICommand<TParameter, TResult>,
        ICommandAsync<TParameter, TResult>
        where TAggregateRoot : class, IAggregateRoot
        where TEntity : IEntity
    {
        protected CreateEntityCommandBase(IRepository<TAggregateRoot> repository, IUnitOfWork unitOfWork, IMapper<TEntity, TResult> mapper)
            : base(repository, unitOfWork)
        {
            Requires.Argument(mapper, nameof(mapper))
                .IsNotNull()
                .Check();

            _mapper = mapper;
        }

        #region ICommand<TParameter,TResult> Members

        public TResult Execute(TParameter parameter)
        {
            var task = ExecuteAsync(parameter);
            task.Wait();

            return task.Result;
        }

        #endregion

        #region ICommandAsync<TParameter,TResult> Members

        public async Task<TResult> ExecuteAsync(TParameter parameter)
        {
            Requires.Argument(parameter, nameof(parameter))
                .IsNotNull()
                .Check();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var aggregateRoot = FinAggregateRoot(parameter);
                Ensures.NotNull(aggregateRoot, $"The root aggregate '{typeof(TAggregateRoot).Name}' is not found");
                var entity = CreateEntity(parameter, aggregateRoot);

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();

                return _mapper.Map(entity);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        #endregion

        protected abstract TAggregateRoot FinAggregateRoot(TParameter parameter);

        protected abstract TEntity CreateEntity(TParameter parameter, TAggregateRoot aggregateRoot);

        private readonly IMapper<TEntity, TResult> _mapper;
    }
}
