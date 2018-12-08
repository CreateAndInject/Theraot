﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Runtime.CompilerServices
{
    public partial class RuntimeOps
    {
#if NET20 || NET30 || NET35 || NET40

        internal sealed class RuntimeVariables : IRuntimeVariables
        {
            private readonly IStrongBox[] _boxes;

            internal RuntimeVariables(IStrongBox[] boxes)
            {
                _boxes = boxes;
            }

            int IRuntimeVariables.Count => _boxes.Length;

            object IRuntimeVariables.this[int index]
            {
                get => _boxes[index].Value;
                set => _boxes[index].Value = value;
            }
        }

#endif
    }
}