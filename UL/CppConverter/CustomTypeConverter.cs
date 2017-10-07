using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    class CustomTypeConverter : ITypeConverter
    {
        public  int priority
        {
            get { return -1; }
        }
        public bool GetCppTypeName(out string name)
        {
            name = "UE_Int32";
            return true;
        }
        public bool SupportType(IConverter Converter, Metadata.DB_Type type)
        {
            switch(type.name)
            {
                case "int32":
                    return true;
            }

            return false;
        }
        public bool ConvertTypeHeader(IConverter Converter, Metadata.DB_Type type, out string header)
        {
            header = "";
            return false;
        }
        public bool ConvertTypeCpp(IConverter Converter, Metadata.DB_Type type, out string cpp)
        {
            cpp = "";
            return false;
        }
        public bool ConvertMethodExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.MethodExp me, out string exp_string)
        {
            exp_string = "";
            return false;

        }
        public bool ConvertFieldExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.FieldExp fe, out string exp_string)
        {
            exp_string = "";
            return false;
        }
        //public bool ConvertIdentifierExp(IConverter Converter, Metadata.DB_Type type, Metadata.Expression.IndifierExp fe, out string exp_string)
        //{
        //    exp_string = "UE_"+type.name;
        //    return true;
        //}
    }
}
