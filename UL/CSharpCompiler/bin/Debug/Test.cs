

using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{

    public class TestInt
    {

    }
    public class Program
    {
        public static int a = 6;

        static void Main(string[] args)
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