namespace System
{
    public class String
    {
        public int Length
        {
            get;
            set;
        }


        public static string Format(
            string format,
            params object[] args
        );
    }
}
