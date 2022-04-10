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
    public abstract class CreateAggregateCommandBase<TEntity, TParameter, TResult> : RepositoryCommandBase<TEntity>,
        ICommand<TParameter, TResult>,
        ICommandAsync<TParameter, TResult>
        where TEntity : class, IAggregateRoot
    {
        protected CreateAggregateCommandBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper<TEntity, TResult> mapper)
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
                var entity = await CreateEntityAsync(parameter);

                _repository.Add(entity);

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

        protected abstract Task<TEntity> CreateEntityAsync(TParameter parameter);

        private readonly IMapper<TEntity, TResult> _mapper;
    }

    public abstract class CreateAggregateCommandBase<TEntity, TModel> : CreateAggregateCommandBase<TEntity, TModel, TModel>
        where TEntity : class, IAggregateRoot
    {
        protected CreateAggregateCommandBase(
            IRepository<TEntity> repository,
            IUnitOfWork unitOfWork,
            IMapper<TEntity, TModel> modelMapper,
            IMapper<TModel, TEntity> entityMapper)
            : base(repository, unitOfWork, modelMapper)
        {
            Requires.Argument(entityMapper, nameof(entityMapper))
                .IsNotNull()
                .Check();

            _entityMapper = entityMapper;
        }

        #region Base Class Member Overrides

        protected sealed override Task<TEntity> CreateEntityAsync(TModel parameter)
        {
            var entity = _entityMapper.Map(parameter);
            return Task.FromResult(entity);
        }

        #endregion

        private readonly IMapper<TModel, TEntity> _entityMapper;
    }
}
