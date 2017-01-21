using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL;

namespace TestClient
{
    class Program
    {

        static void Main(string[] args)
        {
            TestVM();
        }


        static void TestVM()
        {
            UL.Runtime.VisualMachine vm = new UL.Runtime.VisualMachine();
            vm.RunTest();
        }

        static void TestUL()
        {
            //UL_Type ulType = new UL_Type();
            //ulType.Name = "TestClass";

            //UL_Function ulFunc = new UL_Function();
            //ulFunc.Name = "Add";
            //ulFunc.Modifier = EAccessModifier.Public;
            //ulFunc.Owner = ulType;
            //ulType.Functions.Add(ulFunc);

            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(ulType);
            //Console.WriteLine(json);
            //UL_Type ret = Newtonsoft.Json.JsonConvert.DeserializeObject<UL_Type>(json);

        }
    }
}
