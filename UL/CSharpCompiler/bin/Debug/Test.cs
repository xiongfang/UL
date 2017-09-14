

using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class Int32
    {

    }

    public class String
    {

    }
}
namespace HelloWorld
{

    public class TestInt
    {

    }
    public class Program
    {
        public static int a = 6;

        static void Main(string arg)
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

        void Print(ref TestInt hello)
        {
            TestInt c = new TestInt();
            Console.WriteLine("Print");
        }
    }
}