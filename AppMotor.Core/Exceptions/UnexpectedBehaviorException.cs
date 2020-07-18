﻿#region License
// Copyright 2020 AppMotor Framework (https://github.com/skrysmanski/AppMotor)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

using JetBrains.Annotations;

namespace AppMotor.Core.Exceptions
{
    /// <summary>
    /// This exception is for places that code should never reach (because of the logical control flow)
    /// but where the compiler can't detect this (and thus generates a compiler error).
    /// </summary>
    public class UnexpectedBehaviorException : Exception
    {
        [PublicAPI]
        public UnexpectedBehaviorException([NotNull] string message)
            : base(message)
        {
        }

        [PublicAPI]
        public UnexpectedBehaviorException([NotNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
