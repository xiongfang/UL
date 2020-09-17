using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class ULToCS
    {
        public static string To(ULTypeInfo typeInfo)
        {
            StringBuilder sb = new StringBuilder();

            if(!string.IsNullOrEmpty( typeInfo.Namespace))
            {
                sb.AppendLine("using " + typeInfo.Namespace);
            }

            sb.AppendLine("class " + typeInfo.Name);
            sb.AppendLine("{");

            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
