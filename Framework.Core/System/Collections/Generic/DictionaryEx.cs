﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Theraot;

namespace System.Collections.Generic
{
    [Serializable]
    [ComVisible(false)]
    [DebuggerNonUserCode]
    [DebuggerDisplay("Count={" + nameof(Count) + "}")]
    public class DictionaryEx<TKey, TValue> : Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public DictionaryEx()
        {
        }

        public DictionaryEx(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        public DictionaryEx(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        public DictionaryEx(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public DictionaryEx(int capacity)
            : base(capacity)
        {
        }

        public DictionaryEx(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

#if GREATERTHAN_NETCOREAPP20 || NETSTANDARD2_0 || TARGETS_NET

        protected DictionaryEx(SerializationInfo info, StreamingContext context)
            : base(info, context)

#else
        [Obsolete("This target platform does not support binary serialization.")]
        protected DictionaryEx(SerializationInfo info, StreamingContext context)
#endif
        {
            No.Op(info);
            No.Op(context);
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
    }
}