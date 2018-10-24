namespace System
{
    public abstract class Array
    {
        public abstract int Length
        {
            get;
        }
    }

    public class ArrayT<T>:Array
    {
        public ArrayT(int len);

        public extern T this[int i]
        {
            get;
            set;
        }
    }
}
