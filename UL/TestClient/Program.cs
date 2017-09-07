using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        [DllImport("UL_CPP_VM")]
        static extern void Test();


        static void Main(string[] args)
        {
            string v = "5";
            TestString(v);
            Console.WriteLine("v:{0}", v);
        }

        static void TestString(string v)
        {
            v = "6";
        }

    }
}
