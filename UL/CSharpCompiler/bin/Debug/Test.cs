

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
        public int a = 6;
    }
    public class Program
    {
        public static int a = 6;

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

            switch ("5")
            {
                case  "haha":
                    break;
                case TestE.B:
                case TestE.C:
                    break;
            }
        }
    }
}