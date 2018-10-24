using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyntaxLook
{
    public delegate bool TestDel(string v);

    static class Program
    {

        static void TestDel()
        {
            TestDel v = TestDel;
            v("测试委托");
        }

        static bool TestDel(string v)
        {
            Console.WriteLine(v);
            return true;
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
