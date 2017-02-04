using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIL
{
    public class Class1
    {
        public static void Main(int a)
        {
            A aobj = new A();
            B b = new B();
            a =(int) Test(a,aobj,out b);
        }

        static object Test(object b,object a,out B c)
        {
            b = 6;
            c = new B();
            return 0;
        }

        struct A
        {
            int b;
            int c;
        }

        class B
        {
            int a;
            int b;
        }
    }
}
