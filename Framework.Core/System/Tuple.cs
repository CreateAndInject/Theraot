﻿#if LESSTHAN_NET40

#pragma warning disable CA1036 // Override methods on comparable types

using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System
{
    public static class Tuple
    {
        public static Tuple<T1> Create<T1>(T1 item1)
        {
            return new Tuple<T1>(item1);
        }

        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
        }
    }

    [Serializable]
    public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1)
        {
            Item1 = item1;
        }

        public T1 Item1 { get; }

        int ITuple.Length => 1;

        object? ITuple.this[int index] => index is 0 ? Item1 : throw new IndexOutOfRangeException();

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return other is Tuple<T1> tuple && comparer.Equals(Item1, tuple.Item1);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Item1);
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0})", Item1);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            return comparer.Compare(Item1, tuple.Item1);
        }
    }

    [Serializable]
    public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        int ITuple.Length => 2;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            _ => throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            return (hash << 5) - hash + comparer.GetHashCode(Item2);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1})", Item1, Item2);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        int ITuple.Length => 3;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            _ => throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2)
                       && comparer.Equals(Item3, tuple.Item3);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", Item1, Item2, Item3);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        int ITuple.Length => 4;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            _ => throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3, T4> tuple)
            {
                return
                    comparer.Equals(Item1, tuple.Item1)
                    && comparer.Equals(Item2, tuple.Item2)
                    && comparer.Equals(Item3, tuple.Item3)
                    && comparer.Equals(Item4, tuple.Item4);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item4);
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", Item1, Item2, Item3, Item4);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3, T4> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item4, tuple.Item4);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4, T5> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        int ITuple.Length => 5;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            _ => throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3, T4, T5> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2)
                       && comparer.Equals(Item3, tuple.Item3)
                       && comparer.Equals(Item4, tuple.Item4)
                       && comparer.Equals(Item5, tuple.Item5);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item4);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item5);
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3}, {4})", Item1, Item2, Item3, Item4, Item5);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3, T4, T5> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item4, tuple.Item4);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item5, tuple.Item5);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4, T5, T6> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        int ITuple.Length => 6;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            _ => throw new IndexOutOfRangeException()
        };


        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3, T4, T5, T6> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2)
                       && comparer.Equals(Item3, tuple.Item3)
                       && comparer.Equals(Item4, tuple.Item4)
                       && comparer.Equals(Item5, tuple.Item5)
                       && comparer.Equals(Item6, tuple.Item6);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item4);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item5);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item6);
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3}, {4}, {5})", Item1, Item2, Item3, Item4, Item5, Item6);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3, T4, T5, T6> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item4, tuple.Item4);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item5, tuple.Item5);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item6, tuple.Item6);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4, T5, T6, T7> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        public T7 Item7 { get; }

        int ITuple.Length => 7;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            6 => Item7,
            _ => throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2)
                       && comparer.Equals(Item3, tuple.Item3)
                       && comparer.Equals(Item4, tuple.Item4)
                       && comparer.Equals(Item5, tuple.Item5)
                       && comparer.Equals(Item6, tuple.Item6)
                       && comparer.Equals(Item7, tuple.Item7);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item4);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item5);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item6);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item7);
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3}, {4}, {5}, {6})", Item1, Item2, Item3, Item4, Item5, Item6, Item7);
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item4, tuple.Item4);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item5, tuple.Item5);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item6, tuple.Item6);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item7, tuple.Item7);
            }

            return result;
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IStructuralEquatable, IStructuralComparable, IComparable, ITuple
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
        {
            CheckType(rest);
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Rest = rest;
        }

        public T1 Item1 { get; }

        public T2 Item2 { get; }

        public T3 Item3 { get; }

        public T4 Item4 { get; }

        public T5 Item5 { get; }

        public T6 Item6 { get; }

        public T7 Item7 { get; }

        public TRest Rest { get; }

        int ITuple.Length => Rest is ITuple rest ? 7 + rest.Length : 8;

        object? ITuple.this[int index] => index switch
        {
            0 => Item1,
            1 => Item2,
            2 => Item3,
            3 => Item4,
            4 => Item5,
            5 => Item6,
            6 => Item7,
            _ => Rest is ITuple rest ? rest[index - 7] : index is 7 ? Rest : throw new IndexOutOfRangeException()
        };

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            return CompareTo(other, comparer);
        }

        int IComparable.CompareTo(object obj)
        {
            return CompareTo(obj, Comparer<object>.Default);
        }

        public override bool Equals(object obj)
        {
            return ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other is Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            {
                return comparer.Equals(Item1, tuple.Item1)
                       && comparer.Equals(Item2, tuple.Item2)
                       && comparer.Equals(Item3, tuple.Item3)
                       && comparer.Equals(Item4, tuple.Item4)
                       && comparer.Equals(Item5, tuple.Item5)
                       && comparer.Equals(Item6, tuple.Item6)
                       && comparer.Equals(Item7, tuple.Item7)
                       && comparer.Equals(Rest, tuple.Rest);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var hash = comparer.GetHashCode(Item1);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item2);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item3);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item4);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item5);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item6);
            hash = (hash << 5) - hash + comparer.GetHashCode(Item7);
            hash = (hash << 5) - hash + comparer.GetHashCode(Rest);
            return hash;
        }

        public override string ToString()
        {
            var restString = Rest!.ToString();
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", Item1, Item2, Item3, Item4, Item5, Item6, Item7, restString.Substring(1, restString.Length - 2));
        }

        private static void CheckType(TRest rest)
        {
            if (rest == null || !typeof(TRest).IsGenericType)
            {
                throw new ArgumentException("The last element of an eight element tuple must be a Tuple.", nameof(rest));
            }

            var type = typeof(TRest).GetGenericTypeDefinition();
            if
            (
                type == typeof(Tuple<>)
                || type == typeof(Tuple<,>)
                || type == typeof(Tuple<,,>)
                || type == typeof(Tuple<,,,>)
                || type == typeof(Tuple<,,,,>)
                || type == typeof(Tuple<,,,,,>)
                || type == typeof(Tuple<,,,,,,>)
                || type == typeof(Tuple<,,,,,,,>)
            )
            {
                return;
            }

            throw new ArgumentException("The last element of an eight element tuple must be a Tuple.", nameof(rest));
        }

        private int CompareTo(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is not Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            {
                throw new ArgumentException(string.Empty, nameof(other));
            }

            var result = comparer.Compare(Item1, tuple.Item1);
            if (result == 0)
            {
                result = comparer.Compare(Item2, tuple.Item2);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item3, tuple.Item3);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item4, tuple.Item4);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item5, tuple.Item5);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item6, tuple.Item6);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item7, tuple.Item7);
            }

            if (result == 0)
            {
                result = comparer.Compare(Item7, tuple.Item7);
            }

            return result;
        }
    }
}

#endif