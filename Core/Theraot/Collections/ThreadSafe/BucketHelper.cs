﻿// Needed for NET40

using System;

namespace Theraot.Collections.ThreadSafe
{
    public static class BucketHelper
    {
        private static readonly object _null;

        static BucketHelper()
        {
            _null = new object();
        }

        internal static object Null
        {
            get
            {
                return _null;
            }
        }

        /// <summary>
        /// Inserts or replaces the item at the specified index.
        /// </summary>
        /// <param name="bucket">The bucket on which to operate.</param>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        /// <param name="itemUpdateFactory"></param>
        /// <param name="check">A predicate to decide if a particular item should be replaced.</param>
        /// <param name="stored">the item that was left at the specified index.</param>
        /// <param name="isNew">if set to <c>true</c> the index was not previously used.</param>
        /// <returns>
        ///   <c>true</c> if the item or repalced inserted; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index;index must be greater or equal to 0 and less than capacity</exception>
        /// <remarks>
        /// The operation will be attempted as long as check returns true - this operation may starve.
        /// </remarks>
        public static bool InsertOrUpdate<T>(this Bucket<T> bucket, int index, T item, Func<T, T> itemUpdateFactory, Predicate<T> check, out T stored, out bool isNew)
        {
            if (index < 0 || index >= bucket.Capacity)
            {
                throw new ArgumentOutOfRangeException("index", "index must be greater or equal to 0 and less than capacity");
            }
            stored = default(T);
            isNew = true;
            while (true)
            {
                if (isNew)
                {
                    var result = item;
                    if (bucket.InsertInternal(index, result, out stored))
                    {
                        return true;
                    }
                    isNew = false;
                }
                else
                {
                    if (check(stored))
                    {
                        var result = itemUpdateFactory.Invoke(stored);
                        if (bucket.UpdateInternal(index, result, stored, out stored, out isNew))
                        {
                            stored = result;
                            return true;
                        }
                    }
                    else
                    {
                        return false; // returns false only when check returns false
                    }
                }
            }
        }

        /// <summary>
        /// Inserts or replaces the item at the specified index.
        /// </summary>
        /// <param name="bucket">The bucket on which to operate.</param>
        /// <param name="index">The index.</param>
        /// <param name="itemFactory">The item factory to create the item to insert.</param>
        /// <param name="itemUpdateFactory">The item factory to create the item to replace with.</param>
        /// <param name="check">A predicate to decide if a particular item should be replaced.</param>
        /// <param name="stored">the item that was left at the specified index.</param>
        /// <param name="isNew">if set to <c>true</c> the index was not previously used.</param>
        /// <returns>
        ///   <c>true</c> if the item or repalced inserted; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index;index must be greater or equal to 0 and less than capacity</exception>
        /// <remarks>
        /// The operation will be attempted as long as check returns true - this operation may starve.
        /// </remarks>
        public static bool InsertOrUpdate<T>(this Bucket<T> bucket, int index, Func<T> itemFactory, Func<T, T> itemUpdateFactory, Predicate<T> check, out T stored, out bool isNew)
        {
            if (index < 0 || index >= bucket.Capacity)
            {
                throw new ArgumentOutOfRangeException("index", "index must be greater or equal to 0 and less than capacity");
            }
            stored = default(T);
            isNew = true;
            while (true)
            {
                if (isNew)
                {
                    var result = itemFactory.Invoke();
                    itemFactory = () => result;
                    if (bucket.InsertInternal(index, result, out stored))
                    {
                        return true;
                    }
                    isNew = false;
                }
                else
                {
                    if (check(stored))
                    {
                        var result = itemUpdateFactory.Invoke(stored);
                        if (bucket.UpdateInternal(index, result, stored, out stored, out isNew))
                        {
                            stored = result;
                            return true;
                        }
                    }
                    else
                    {
                        return false; // returns false only when check returns false
                    }
                }
            }
        }
    }
}