namespace System
{
    public class Object
    {
        public virtual Type GetType();

        public virtual bool Equals(object v);

        public virtual string ToString();

        public virtual Boolean op_Equals(object b);

        public virtual Object op_Assign(Object b);
    }
}
