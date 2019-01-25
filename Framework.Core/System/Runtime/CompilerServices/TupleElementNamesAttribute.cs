﻿#if LESSTHAN_NET40

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that the use of <see cref="T:System.ValueTuple" /> on a member is meant to be treated as a tuple with element names.
    /// </summary>
    [CLSCompliant(false)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Event)]
    public sealed class TupleElementNamesAttribute : Attribute
    {
        private readonly string[] _transformNames;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.TupleElementNamesAttribute" /> class.
        /// </summary>
        /// <param name="transformNames">
        /// Specifies, in a pre-order depth-first traversal of a type's
        /// construction, which <see cref="T:System.ValueType" /> occurrences are
        /// meant to carry element names.
        /// </param>
        /// <remarks>
        /// This constructor is meant to be used on types that contain an
        /// instantiation of <see cref="T:System.ValueType" /> that contains
        /// element names.  For instance, if <c>C</c> is a generic type with
        /// two type parameters, then a use of the constructed type <c>C{<see cref="T:System.ValueTuple`2" />, <see cref="T:System.ValueTuple`3" /></c> might be intended to
        /// treat the first type argument as a tuple with element names and the
        /// second as a tuple without element names. In which case, the
        /// appropriate attribute specification should use a
        /// <c>transformNames</c> value of <c>{ "name1", "name2", null, null,
        /// null }</c>.
        /// </remarks>
        public TupleElementNamesAttribute(string[] transformNames)
        {
            _transformNames = transformNames ?? throw new ArgumentNullException(nameof(transformNames));
        }

        /// <summary>
        /// Specifies, in a pre-order depth-first traversal of a type's
        /// construction, which <see cref="ValueTuple"/> elements are
        /// meant to carry element names.
        /// </summary>
        public IList<string> TransformNames => _transformNames;
    }
}

#endif