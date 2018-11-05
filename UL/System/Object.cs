namespace ul
{
    namespace System
    {
        public class Object
        {
            public extern virtual Type GetType();

            public extern virtual bool Equals(object v);

            public extern virtual string ToString();

            public extern static bool operator ==(Object a, Object b);
            public static bool operator !=(Object a, Object b)
            {
                return !(a == b);
            }

            public extern static bool ReferenceEquals(Object a, Object b);
        }
    }
}