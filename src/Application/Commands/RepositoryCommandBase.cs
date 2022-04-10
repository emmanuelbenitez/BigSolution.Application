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

using BigSolution.Domain;

namespace BigSolution.Application.Commands
{
    public abstract class RepositoryCommandBase<TEntity>
        where TEntity : class, IAggregateRoot
    {
        protected RepositoryCommandBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            Requires.Argument(repository, nameof(repository))
                .IsNotNull()
                .Check();
            Requires.Argument(unitOfWork, nameof(unitOfWork))
                .IsNotNull()
                .Check();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        protected readonly IRepository<TEntity> _repository;

        protected readonly IUnitOfWork _unitOfWork;
    }
}
