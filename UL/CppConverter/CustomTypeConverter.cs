using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    //类定义
    //类标识符，访问成员
    //作为类成员
    //作为函数参数
    //作为函数返回类型
    //表达式返回类型
    //隐式类型转换
    //强制类型转换

    class CustomTypeConverter : ITypeConverter
    {
        public  int priority
        {
            get { return -1; }
        }
        public bool GetCppTypeName(IConverter Converter, Metadata.DB_Type type, out string name)
        {
            switch (type.name)
            {
                case "int32":
                    name = "UE_Int32";
                    break;
                case "Object":
                    name = "UObject";
                    break;
                default:
                    name = "";
                    return false;
            }
            return true;
        }
        public bool SupportType(IConverter Converter, Metadata.DB_Type type)
        {
            switch(type.name)
            {
                case "int32":
                case "Object":
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
            //if(type.name == "Object")
            //{
            //    if(me.Caller == "ToString")
            //    {
            //        exp_string = string.Format("Object::ToString({1})", Converter.ExpressionToString(me.Caller));
            //        return true;
            //    }
            //}
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
