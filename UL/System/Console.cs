namespace ul
{
    namespace System
    {
        public class Console
        {
            public extern static void Write(string value);

            public static void Write(object value)
            {
                Write(value.ToString());
            }
            public static void Write(char value)
            {
                Write(value.ToString());
            }

            public static void Write(bool value)
            {
                Write(value.ToString());
            }

            public static void Write(int value)
            {
                Write(value.ToString());
            }
            public static void Write(long value)
            {
                Write(value.ToString());
            }

            public static void Write(float value)
            {
                Write(value.ToString());
            }

            public static void Write(double value)
            {
                Write(value.ToString());
            }

            public static void Write(byte value)
            {
                Write(value.ToString());
            }

            public static void WriteLine()
            {
                Write("\r\n");
            }

            public static void WriteLine(char value)
            {
                Write(value.ToString());
                WriteLine();
            }

            public static void WriteLine(bool value)
            {
                Write(value.ToString());
                WriteLine();
            }

            public static void WriteLine(int value)
            {
                Write(value.ToString());
                WriteLine();
            }
            public static void WriteLine(long value)
            {
                Write(value.ToString());
                WriteLine();
            }

            public static void WriteLine(float value)
            {
                Write(value.ToString());
                WriteLine();
            }

            public static void WriteLine(double value)
            {
                Write(value.ToString());
                WriteLine();
            }

            public static void WriteLine(byte value)
            {
                Write(value.ToString());
                WriteLine();
            }
            public static void WriteLine(string value)
            {
                Write(value);
                WriteLine();
            }
            public static void WriteLine(object value)
            {
                Write(value.ToString());
                WriteLine();
            }
        }
    }
}