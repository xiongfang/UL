namespace ul
{
    using ul.System;

    namespace System.Collections.Generic
    {
        public class List<T>
        {
            public int Count
            {
                get;
            }

            public T this[int index]
            {
                get;
                set;
            }
            public extern void Add(
                T item
            );
            public extern void Remove(
                T item
            );
            public extern void RemoveAll(
                T item
            );
            public extern void Clear();

            public extern T[] ToArray();
        }
    }
}