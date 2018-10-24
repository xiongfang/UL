namespace System
{
    public class Object
    {
        public virtual Type GetType();

        public virtual bool Equals(object v);

        public virtual string ToString();

        public static bool operator==(Object a, Object b);
        public static bool operator !=(Object a, Object b)
        {
            return !(a == b);
        }

        public extern static bool ReferenceEquals(Object a, Object b);
    }
}
