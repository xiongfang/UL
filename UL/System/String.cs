namespace ul
{
    namespace System
    {
        public class String
        {
            public int Length
            {
                get;
            }

            public extern int IndexOf(
                char value
            );
            public extern int IndexOf(
               string value
           );
            public extern int LastIndexOf(
                char value
            );
            public extern int LastIndexOf(
               string value
           );
            public extern static string Format(
                string format,
                params object[] args
            );

            public extern static String operator +(String a, String b);
        }
    }
}