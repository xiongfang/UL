using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    
    class Program
    {
        static void Main(string[] args)
        {
            CppConverter cc = new CppConverter();
            //cc.RegistTypeConverter(new CustomTypeConverter());
            cc.GO(args);
        }
    }

}
