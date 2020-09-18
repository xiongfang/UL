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

        void Append(string t)
        {
            sb.Append(t);
        }
        void BeginAppend()
        {
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
        }

        void EndAppend()
        {
            sb.AppendLine();
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

            sb.BeginAppend();

            if(typeInfo.ExportType == EExportType.Public)
            {
                sb.Append("public ");
            }
            else if(typeInfo.ExportType == EExportType.Protected)
            {
                sb.Append("protected ");
            }
            else
            {
                sb.Append("private ");
            }

            sb.Append("class " + typeInfo.Name);
            sb.EndAppend();

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
