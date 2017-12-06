
namespace System
{
    public delegate bool TestDel(string v);

    class Test
    {
        public static void Run()
        {
            TestInt();
            TestString();
            TestDel();
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

            Console.WriteLine(a-5);
            Console.WriteLine(a % 5);
            Console.WriteLine(a ++);
            Console.WriteLine(++a);
            Console.WriteLine(-a);
        }

        static void TestString()
        {
            string v = "你好";
            Console.WriteLine(v);
            Console.WriteLine(v.Length);
        }

        static void TestDel()
        {
            TestDel v = Test.TestDel;
            v("测试委托");

            Test t = new Test();
            TestDel v2 = t.TestDel2;
            v2("测试委托2");
        }

        static bool TestDel(string v)
        {
            Console.WriteLine(v);
            return true;
        }

        bool TestDel2(string v)
        {
            Console.WriteLine(v);
            return true;
        }
    }
}
