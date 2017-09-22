
namespace System
{
    public class Int32
    {
        public Int32 op_Equals(Int32 b)
        {
            return 1;
        }
        public Int32 op_Small(Int32 b)
        {
            return 1;
        }
        public Int32 op_Assign(Int32 b)
        {
            return 1;
        }
    }

    public class String
    {

    }

    public class Object
    {

    }

    public class Array<T>
    {

    }

    public class Console
    {
        public static void WriteLine(string v)
        {

        }
    }
}
namespace HelloWorld
{

    using System;

    public class TestGeneric<T1,T2> where T1:TestInt
    {
        public T1 a;
        //public T1 TestGenericFunction()
        //{
            //T1 a = new T1();
            //return a;
        //}
    }

    public class TestInt
    {
        public int a = 6;
    }
    public class Program
    {
        public static int a = 6;

        public TestGeneric<int, string> v;
        public int[] s;

        static void Main(string arg,int arg2)
        {
            if(true)
            {
                Console.WriteLine("Hello, World!");
            }
            else
            {
                Console.WriteLine("Hello, World!1");
                Console.WriteLine("Hello, World!2");

            }
        }
        enum TestE
        {
            A,
            B,
            C
        }
        void Print(ref TestInt hello)
        {
            TestInt c = new TestInt();
            c.a = 7;
            Console.WriteLine("Print");

            int[] v = new int[6];

            for(int i=0,c=6;i<3;i++)
            {

            }

            do
            {
                Console.WriteLine("DO");
            } while (true);

            while(true)
            {
                Console.WriteLine("while");
            }
            switch (5)
            {
                case 1:
                    break;
                case 2:
                case 3:
                    break;
            }

            //switch ("5")
            //{
            //    case  "haha":
            //        break;
            //    case TestE.B:
            //    case TestE.C:
            //        break;
            //}

            TestGeneric<int, string> temp = new TestGeneric<int, string>();
        }
    }
}