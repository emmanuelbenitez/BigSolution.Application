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
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using BigSolution.Application.Commands;
using BigSolution.Application.Queries;

namespace BigSolution.Application
{
    public static class RegistrationExtensions
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsQuery<TLimit, TScanningActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
            where TScanningActivatorData : ScanningActivatorData
        {
            Requires.Argument(registration, nameof(registration))
                .IsNotNull()
                .Check();
            return registration.Where(t => t.Is<IQuery>());
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsCommand<TLimit, TScanningActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
            where TScanningActivatorData : ScanningActivatorData
        {
            Requires.Argument(registration, nameof(registration))
                .IsNotNull()
                .Check();
            return registration.Where(t => t.Is<ICommand>());
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API")]
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> IsCommandOrQuery<TLimit, TScanningActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration)
            where TScanningActivatorData : ScanningActivatorData
        {
            Requires.Argument(registration, nameof(registration))
                .IsNotNull()
                .Check();
            return registration.Where(t => t.Is<ICommand>() || t.Is<IQuery>());
        }
    }
}
