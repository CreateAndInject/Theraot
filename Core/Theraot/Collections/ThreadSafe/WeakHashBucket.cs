﻿#if FAT

using System;
using System.Collections.Generic;
using Theraot.Collections.Specialized;
using Theraot.Core;
using Theraot.Threading;
using Theraot.Threading.Needles;

namespace Theraot.Collections.ThreadSafe
{
    [global::System.Diagnostics.DebuggerNonUserCode]
    [System.Diagnostics.DebuggerDisplay("Count={Count}")]
    public class WeakHashBucket<TKey, TValue, TNeedle> : IEnumerable<KeyValuePair<TKey, TValue>>, IEqualityComparer<TKey>
        where TKey : class
        where TNeedle : WeakNeedle<TKey>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly SafeDictionary<TNeedle, TValue> _wrapped;

        private StructNeedle<WeakNeedle<EventHandler>> _eventHandler;

        public WeakHashBucket()
            : this(null as IEqualityComparer<TKey>, true)
        {
            //Empty
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype)
            : this(Check.NotNullArgument(prototype, "prototype").Length, null as IEqualityComparer<TKey>, true)
        {
            AddRange(prototype);
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, IEqualityComparer<TKey> comparer)
            : this(Check.NotNullArgument(prototype, "prototype").Length, comparer, true)
        {
            AddRange(prototype);
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, bool autoRemoveDeadItems)
            : this(Check.NotNullArgument(prototype, "prototype").Length, null as IEqualityComparer<TKey>, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems)
            : this(Check.NotNullArgument(prototype, "prototype").Length, comparer, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype)
            : this(null as IEqualityComparer<TKey>, true)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer)
            : this(comparer, true)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, bool autoRemoveDeadItems)
            : this(null as IEqualityComparer<TKey>, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems)
            : this(comparer, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEqualityComparer<TKey> comparer)
            : this(comparer, true)
        {
            //Empty
        }

        public WeakHashBucket(bool autoRemoveDeadItems)
            : this(null as IEqualityComparer<TKey>, autoRemoveDeadItems)
        {
            //Empty
        }

        public WeakHashBucket(IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems)
        {
            var defaultComparer = EqualityComparerHelper<TKey>.Default;
            IEqualityComparer<TNeedle> needleComparer;
            if (ReferenceEquals(comparer, null) || ReferenceEquals(comparer, defaultComparer))
            {
                _comparer = defaultComparer;
                needleComparer = EqualityComparerHelper<TNeedle>.Default;
            }
            else
            {
                _comparer = comparer;
                needleComparer = new NeedleConversionEqualityComparer<TNeedle, TKey>(_comparer);
            }
            _wrapped = new SafeDictionary<TNeedle, TValue>(needleComparer);
            if (autoRemoveDeadItems)
            {
                RegisterForAutoRemoveDeadItemsExtracted();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }

        public WeakHashBucket(int capacity)
            : this(capacity, null as IEqualityComparer<TKey>, true)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype)
            : this(capacity, null as IEqualityComparer<TKey>, true)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer)
            : this(capacity, comparer, true)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, bool autoRemoveDeadItems)
            : this(capacity, null as IEqualityComparer<TKey>, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems)
            : this(capacity, comparer, autoRemoveDeadItems)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEqualityComparer<TKey> comparer)
            : this(capacity, comparer, true)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, bool autoRemoveDeadItems)
            : this(capacity, null as IEqualityComparer<TKey>, autoRemoveDeadItems)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems)
        {
            var defaultComparer = EqualityComparerHelper<TKey>.Default;
            IEqualityComparer<TNeedle> needleComparer;
            if (ReferenceEquals(comparer, null) || ReferenceEquals(comparer, defaultComparer))
            {
                _comparer = defaultComparer;
                needleComparer = EqualityComparerHelper<TNeedle>.Default;
            }
            else
            {
                _comparer = comparer;
                needleComparer = new NeedleConversionEqualityComparer<TNeedle, TKey>(_comparer);
            }
            _wrapped = new SafeDictionary<TNeedle, TValue>(capacity, needleComparer);
            if (autoRemoveDeadItems)
            {
                RegisterForAutoRemoveDeadItemsExtracted();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, int maxProbing)
            : this(Check.NotNullArgument(prototype, "prototype").Length, null as IEqualityComparer<TKey>, true, maxProbing)
        {
            AddRange(prototype);
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, IEqualityComparer<TKey> comparer, int maxProbing)
            : this(Check.NotNullArgument(prototype, "prototype").Length, comparer, true, maxProbing)
        {
            AddRange(prototype);
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, bool autoRemoveDeadItems, int maxProbing)
            : this(Check.NotNullArgument(prototype, "prototype").Length, null as IEqualityComparer<TKey>, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(KeyValuePair<TKey, TValue>[] prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems, int maxProbing)
            : this(Check.NotNullArgument(prototype, "prototype").Length, comparer, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, int maxProbing)
            : this(null as IEqualityComparer<TKey>, true, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, int maxProbing)
            : this(comparer, true, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, bool autoRemoveDeadItems, int maxProbing)
            : this(null as IEqualityComparer<TKey>, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems, int maxProbing)
            : this(comparer, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(IEqualityComparer<TKey> comparer, int maxProbing)
            : this(comparer, true, maxProbing)
        {
            //Empty
        }

        public WeakHashBucket(bool autoRemoveDeadItems, int maxProbing)
            : this(null as IEqualityComparer<TKey>, autoRemoveDeadItems, maxProbing)
        {
            //Empty
        }

        public WeakHashBucket(IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems, int maxProbing)
        {
            var defaultComparer = EqualityComparerHelper<TKey>.Default;
            IEqualityComparer<TNeedle> needleComparer;
            if (ReferenceEquals(comparer, null) || ReferenceEquals(comparer, defaultComparer))
            {
                _comparer = defaultComparer;
                needleComparer = EqualityComparerHelper<TNeedle>.Default;
            }
            else
            {
                _comparer = comparer;
                needleComparer = new NeedleConversionEqualityComparer<TNeedle, TKey>(_comparer);
            }
            _wrapped = new SafeDictionary<TNeedle, TValue>(needleComparer, maxProbing);
            if (autoRemoveDeadItems)
            {
                RegisterForAutoRemoveDeadItemsExtracted();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }

        public WeakHashBucket(int capacity, int maxProbing)
            : this(capacity, null as IEqualityComparer<TKey>, true, maxProbing)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, int maxProbing)
            : this(capacity, null as IEqualityComparer<TKey>, true, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, int maxProbing)
            : this(capacity, comparer, true, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, bool autoRemoveDeadItems, int maxProbing)
            : this(capacity, null as IEqualityComparer<TKey>, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEnumerable<KeyValuePair<TKey, TValue>> prototype, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems, int maxProbing)
            : this(capacity, comparer, autoRemoveDeadItems, maxProbing)
        {
            AddRange(Check.NotNullArgument(prototype, "prototype"));
        }

        public WeakHashBucket(int capacity, IEqualityComparer<TKey> comparer, int maxProbing)
            : this(capacity, comparer, true, maxProbing)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, bool autoRemoveDeadItems, int maxProbing)
            : this(capacity, null as IEqualityComparer<TKey>, autoRemoveDeadItems, maxProbing)
        {
            //Empty
        }

        public WeakHashBucket(int capacity, IEqualityComparer<TKey> comparer, bool autoRemoveDeadItems, int maxProbing)
        {
            var defaultComparer = EqualityComparerHelper<TKey>.Default;
            IEqualityComparer<TNeedle> needleComparer;
            if (ReferenceEquals(comparer, null) || ReferenceEquals(comparer, defaultComparer))
            {
                _comparer = defaultComparer;
                needleComparer = EqualityComparerHelper<TNeedle>.Default;
            }
            else
            {
                _comparer = comparer;
                needleComparer = new NeedleConversionEqualityComparer<TNeedle, TKey>(_comparer);
            }
            _wrapped = new SafeDictionary<TNeedle, TValue>(capacity, needleComparer, maxProbing);
            if (autoRemoveDeadItems)
            {
                RegisterForAutoRemoveDeadItemsExtracted();
            }
            else
            {
                GC.SuppressFinalize(this);
            }
        }

        ~WeakHashBucket()
        {
            UnRegisterForAutoRemoveDeadItemsExtracted();
        }

        public bool AutoRemoveDeadItems
        {
            get
            {
                return _eventHandler.IsAlive;
            }
            set
            {
                if (value)
                {
                    RegisterForAutoRemoveDeadItems();
                }
                else
                {
                    UnRegisterForAutoRemoveDeadItems();
                }
            }
        }

        public int Count
        {
            get
            {
                return _wrapped.Count;
            }
        }

        protected SafeDictionary<TNeedle, TValue> Wrapped
        {
            get
            {
                return _wrapped;
            }
        }

        public bool Add(KeyValuePair<TKey, TValue> item)
        {
            TNeedle needle = NeedleHelper.CreateNeedle<TKey, TNeedle>(item.Key);
            if (_wrapped.TryAdd(needle, item.Value))
            {
                return true;
            }
            else
            {
                needle.Dispose();
                return false;
            }
        }

        public void Add(TKey key, TValue value)
        {
            TNeedle needle = NeedleHelper.CreateNeedle<TKey, TNeedle>(key);
            if (!_wrapped.TryAdd(needle, value))
            {
                needle.Dispose();
            }
        }

        public int AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            int count = 0;
            foreach (var item in Check.NotNullArgument(items, "items"))
            {
                if (Add(item))
                {
                    count++;
                }
            }
            return count;
        }

        public KeyValuePair<TKey, TValue> CharyAdd(TKey key, TValue value)
        {
            var needle = NeedleHelper.CreateNeedle<TKey, TNeedle>(key);
            KeyValuePair<TNeedle, TValue> result;
            _wrapped.TryAdd(needle, value, out result);
            if (ReferenceEquals(result.Key, null))
            {
                return new KeyValuePair<TKey, TValue>(null, result.Value);
            }
            else
            {
                needle.Dispose();
                return new KeyValuePair<TKey, TValue>(result.Key.Value, result.Value);
            }
        }

        public void Clear()
        {
            var displaced = _wrapped.ClearEnumerable();
            foreach (var item in displaced)
            {
                item.Key.Dispose();
            }
        }

        public WeakHashBucket<TKey, TValue, TNeedle> Clone()
        {
            return OnClone();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var key = item.Key;
            TValue value;
            TNeedle needle = NeedleHelper.CreateNeedle<TKey, TNeedle>(key);
            if (_wrapped.TryGetValue(needle, out value))
            {
                needle.Dispose();
                return EqualityComparer<TValue>.Default.Equals(value, item.Value);
            }
            else
            {
                needle.Dispose();
                return false;
            }
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var _item in this)
            {
                if (_comparer.Equals(_item.Key, key))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Extensions.CopyTo(this, array, arrayIndex);
        }

        public bool Equals(TKey x, TKey y)
        {
            return _comparer.Equals(x, y);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _wrapped.ConvertProgressiveFiltered(input => new KeyValuePair<TKey, TValue>(input.Key.Value, input.Value), input => input.Key.IsAlive).GetEnumerator();
        }

        public int GetHashCode(TKey obj)
        {
            return _comparer.GetHashCode(obj);
        }

        public bool Remove(TKey key)
        {
            TValue found;
            return _wrapped.Remove(GetHashCode(key), _key => Equals(key, _key.Value), out found);
        }

        public int RemoveDeadItems()
        {
            return _wrapped.RemoveWhereKey(key => !key.IsAlive);
        }

        public int RemoveWhereKey(Predicate<TKey> predicate)
        {
            return _wrapped.RemoveWhereKey
                   (
                       key =>
                       {
                           if (key.IsAlive)
                           {
                               return predicate.Invoke(key.Value);
                           }
                           else
                           {
                               return false;
                           }
                       }
                   );
        }

        public void Set(TKey key, TValue value)
        {
            var needle = new TNeedle[1];
            // TODO: upgrade to AddOrUpdate if possible
            _wrapped.Set(needle[0], value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryAdd(TKey key, TValue value)
        {
            TNeedle needle = NeedleHelper.CreateNeedle<TKey, TNeedle>(key);
            if (_wrapped.TryAdd(needle, value))
            {
                return true;
            }
            else
            {
                needle.Dispose();
                return false;
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            return _wrapped.TryGetValue(GetHashCode(key), _key => Equals(key, _key.Value), out value);
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _wrapped.TryGetValue(GetHashCode(key), _key => Equals(key, _key.Value), out value);
        }

        protected virtual WeakHashBucket<TKey, TValue, TNeedle> OnClone()
        {
            return new WeakHashBucket<TKey, TValue, TNeedle>(this, _comparer);
        }

        private void GarbageCollected(object sender, EventArgs e)
        {
            RemoveDeadItems();
        }

        private void RegisterForAutoRemoveDeadItems()
        {
            if (RegisterForAutoRemoveDeadItemsExtracted())
            {
                GC.ReRegisterForFinalize(this);
            }
        }

        private bool RegisterForAutoRemoveDeadItemsExtracted()
        {
            bool result = false;
            EventHandler eventHandler;
            if (ReferenceEquals(_eventHandler.Value, null))
            {
                eventHandler = GarbageCollected;
                _eventHandler = new WeakNeedle<EventHandler>(eventHandler);
                result = true;
            }
            else
            {
                eventHandler = _eventHandler.Value.Value;
                if (!_eventHandler.IsAlive)
                {
                    eventHandler = GarbageCollected;
                    _eventHandler.Value = eventHandler;
                    result = true;
                }
            }
            GCMonitor.Collected += eventHandler;
            return result;
        }

        private void UnRegisterForAutoRemoveDeadItems()
        {
            if (UnRegisterForAutoRemoveDeadItemsExtracted())
            {
                GC.SuppressFinalize(this);
            }
        }

        private bool UnRegisterForAutoRemoveDeadItemsExtracted()
        {
            EventHandler eventHandler;
            if (_eventHandler.Value.Retrieve(out eventHandler))
            {
                GCMonitor.Collected -= eventHandler;
                _eventHandler.Value = null;
                return true;
            }
            else
            {
                _eventHandler.Value = null;
                return false;
            }
        }
    }
}

#endif