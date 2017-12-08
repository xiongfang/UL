namespace System
{
    public class Array
    {
        public int Length
        {
            get;
        }
    }

    public class ArrayT<T>:Array
    {
        public ArrayT(int len) { }

        public extern T this[int i]
        {
            get;
            set;
        }
    }
}
