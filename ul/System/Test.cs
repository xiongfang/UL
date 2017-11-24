
namespace System
{
    class Test
    {
        public static void Run()
        {
            TestInt();
            TestString();
        }

        static void TestInt()
        {
            Int32 a = 5;
            a = a + 6;
            Console.WriteLine(a);
            a += 7;
            Console.WriteLine(a);

            Int64 b = a;
            Console.WriteLine(b);
        }

        static void TestString()
        {
            string v = "你好";
            Console.WriteLine(v);
        }
    }
}
