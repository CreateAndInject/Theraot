﻿#if LESSTHAN_NET35

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions.Compiler;
using System.Reflection;
using Theraot.Collections;

namespace Theraot.Reflection
{
    public static class DelegateHelper
    {
        private const MethodAttributes _ctorAttributes = MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public;

        private const MethodImplAttributes _implAttributes = (MethodImplAttributes)((int)MethodImplAttributes.Runtime | (int)MethodImplAttributes.Managed);

        private const MethodAttributes _invokeAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual;

        private const int _maximumArity = 17;

        private static readonly TypeInfo _delegateCache = new TypeInfo();

        private static readonly Type[] _delegateCtorSignature = { typeof(object), typeof(IntPtr) };

        internal static Type MakeDelegateTypeInternal(params Type[] types)
        {
            lock (_delegateCache)
            {
                // arguments & return type
                var curTypeInfo = types.Aggregate(_delegateCache, (current, type) => NextTypeInfo(type, current));

                // see if we have the delegate already
                // clone because MakeCustomDelegate can hold onto the array.
                return curTypeInfo.DelegateType ??= curTypeInfo.DelegateType = MakeNewDelegate((Type[])types.Clone());
            }
        }

        private static Type MakeNewCustomDelegate(Type[] types)
        {
            var returnType = types[types.Length - 1];
            var parameters = types.RemoveLast();

            var builder = AssemblyGen.DefineDelegateType("Delegate" + types.Length);
            builder.DefineConstructor(_ctorAttributes, CallingConventions.Standard, _delegateCtorSignature).SetImplementationFlags(_implAttributes);
            builder.DefineMethod("Invoke", _invokeAttributes, returnType, parameters).SetImplementationFlags(_implAttributes);
            return builder.CreateType();
        }

        private static Type MakeNewDelegate(Type[] types)
        {
            Debug.Assert(types.Length > 0);

            // Can only used predefined delegates if we have no byref types and
            // the arity is small enough to fit in Func<...> or Action<...>

            var needCustom = types.Length > _maximumArity || types.Any(type => type.IsByRef || /*type.IsByRefLike ||*/ type.IsPointer);

            if (needCustom)
            {
                return MakeNewCustomDelegate(types);
            }

            return types[types.Length - 1] == typeof(void) ? DelegateBuilder.GetActionType(types.RemoveLast())! : DelegateBuilder.GetFuncType(types)!;
        }

        private static TypeInfo NextTypeInfo(Type initialArg, TypeInfo curTypeInfo)
        {
            var lookingUp = initialArg;
            if (curTypeInfo.TypeChain == null)
            {
                curTypeInfo.TypeChain = new Dictionary<Type, TypeInfo>();
            }

            if (curTypeInfo.TypeChain.TryGetValue(lookingUp, out var nextTypeInfo))
            {
                return nextTypeInfo;
            }

            nextTypeInfo = new TypeInfo();
            curTypeInfo.TypeChain[lookingUp] = nextTypeInfo;

            return nextTypeInfo;
        }

        private class TypeInfo
        {
            public Type? DelegateType;
            public Dictionary<Type, TypeInfo>? TypeChain;
        }
    }
}

#endif