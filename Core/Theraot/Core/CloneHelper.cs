#if FAT

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Theraot.Core
{
    public static class CloneHelper<T>
    {
        public static ICloner<T> GetCloner()
        {
            Type type = typeof(T);
            if (type.IsImplementationOf(typeof(ICloneable<T>)))
            {
                return GenericCloner.Instance;
            }
            else if (type.IsImplementationOf(typeof(ICloneable)))
            {
                return Cloner.Instance;
            }
            else if (type.IsSerializable)
            {
                return SerializerCloner.Instance;
            }
            else
            {
                return null;
            }
        }

        private class Cloner : ICloner<T>
        {
            private static ICloner<T> _instance = new Cloner();

            private Cloner()
            {
                //Empty
            }

            public static ICloner<T> Instance
            {
                get
                {
                    return _instance;
                }
            }

            public T Clone(T target)
            {
                return (T)(target as ICloneable).Clone();
            }
        }

        private class GenericCloner : ICloner<T>
        {
            private static ICloner<T> _instance = new GenericCloner();

            private GenericCloner()
            {
                //Empty
            }

            public static ICloner<T> Instance
            {
                get
                {
                    return _instance;
                }
            }

            public T Clone(T target)
            {
                return (target as ICloneable<T>).Clone();
            }
        }

        private class SerializerCloner : ICloner<T>
        {
            private static ICloner<T> _instance = new SerializerCloner();

            private SerializerCloner()
            {
                //Empty
            }

            public static ICloner<T> Instance
            {
                get
                {
                    return _instance;
                }
            }

            public T Clone(T target)
            {
                var formatter = new BinaryFormatter();
                var stream = new MemoryStream();
                formatter.Serialize(stream, target);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}

#endif