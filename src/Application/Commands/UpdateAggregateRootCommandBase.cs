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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BigSolution.Domain;
using BigSolution.Infra.Mapping;

namespace BigSolution.Application.Commands
{
    public abstract class UpdateAggregateRootCommandBase<TAggregate, TParameter> :
        RepositoryCommandBase<TAggregate>,
        ICommand<TParameter>, ICommandAsync<TParameter>
        where TAggregate : class, IAggregateRoot
    {
        protected UpdateAggregateRootCommandBase(IRepository<TAggregate> repository, IUnitOfWork unitOfWork, IMapper<TParameter, TAggregate> mapper) : base(
            repository,
            unitOfWork)
        {
            Requires.Argument(mapper, nameof(mapper))
                .IsNotNull()
                .Check();

            _mapper = mapper;
        }

        #region ICommand<TParameter> Members

        public void Execute(TParameter parameter)
        {
            var task = ExecuteAsync(parameter);
            task.Wait();
        }

        #endregion

        #region ICommandAsync<TParameter> Members

        public async Task ExecuteAsync(TParameter parameter)
        {
            Requires.Argument(parameter, nameof(parameter))
                .IsNotNull()
                .Check();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var aggregate = await FindAggregateAsync(parameter);

                Ensures.NotNull(aggregate, "The aggregate does not exist.");

                Update(aggregate, parameter);

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        #endregion

        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "Public API.")]
        protected virtual void Update(TAggregate aggregate, TParameter parameter)
        {
            _mapper.Map(parameter, aggregate);
        }

        protected abstract Task<TAggregate> FindAggregateAsync(TParameter parameter);

        private readonly IMapper<TParameter, TAggregate> _mapper;
    }
}
