

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


