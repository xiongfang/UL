
namespace System
{
    public interface IComparable
    {
        int CompareTo(object obj);
    }
    public struct Int32: IComparable
    {
        public Int32 op_Equals(Int32 b)
        {
            return 1;
        }
        public Boolean op_Small(Int32 b)
        {
            return 1;
        }
        public Int32 op_Assign(Int32 b)
        {
            return 1;
        }

        public Int32 op_PlusPlus(Int32 b)
        {
            return 1;
        }
    }

    public struct Boolean
    {

    }

    public class String
    {

    }

    public class Object
    {

    }

    public class Array<T>
    {
        public Array(int len) { }
    }

    public class Console
    {
        public static void WriteLine(string v);
    }

    //
    // 摘要:
    //     
    public enum MemberTypes
    {
        //
        // 摘要:
        //     指定该成员是一个构造函数
        Constructor = 1,
        //
        // 摘要:
        //     指定该成员是一个事件。
        Event = 2,
        //
        // 摘要:
        //     指定该成员是一个字段。
        Field = 4,
        //
        // 摘要:
        //     指定该成员是一种方法。
        Method = 8,
        //
        // 摘要:
        //     指定成员是属性。
        Property = 16,
        //
        // 摘要:
        //     指定该成员是一种类型。
        TypeInfo = 32,
        //
        // 摘要:
        //     指定该成员是自定义成员的指针类型。
        Custom = 64,
        //
        // 摘要:
        //     指定该成员是嵌套的类型。
        NestedType = 128,
        //
        // 摘要:
        //     指定所有成员类型。
        All = 191
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
    enum TestE
    {
        A,
        B,
        C
    }

    public class Program
    {
        public static int a = 6;

        public TestGeneric<int, string> v;
        public int[] s;

        public static void Main()
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

        void Print(ref HelloWorld.TestInt hello)
        {
            TestInt c = new TestInt();
            c.a = 7;
            Console.WriteLine("Print");

            MemberTypes testEnum = MemberTypes.Event;

            int[] v = new int[6];

            for (int i=0,c=6;i<3;i++)
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