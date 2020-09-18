using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class ULToCS
    {
        public readonly static string[] keywords = new string[]
        {
            "namespace",
            "class",
            "public",
            "protected",
            "private",
            "override",

        };

        int depth = 0;
        StringBuilder sb = new StringBuilder();

        void AppendLine(string t)
        {
            for(int i=0;i<depth;i++)
            {
                sb.Append("\t");
            }
            sb.AppendLine(t);
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public static string To(ULTypeInfo typeInfo)
        {
            var sb = new ULToCS();

            if(!string.IsNullOrEmpty( typeInfo.Namespace))
            {
                sb.AppendLine("namespace " + typeInfo.Namespace);
                sb.AppendLine("{");
                sb.depth++;
            }

            sb.AppendLine("class " + typeInfo.Name);
            sb.AppendLine("{");

            sb.AppendLine("}");

            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                sb.depth--;
                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }
}
