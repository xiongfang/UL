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
            "public",
            "namespace",
            "class",
            "void",
            "protected",
            "private",
            "override",
            "static"

        };
        ULTypeInfo typeInfo;

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
        void BeginAppendLine()
        {
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
        }

        void EndAppendLine()
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
            sb.typeInfo = typeInfo;
            sb.ToType();

            return sb.ToString();
        }

        void ToType()
        {
            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                AppendLine("namespace " + typeInfo.Namespace);
                AppendLine("{");
                depth++;
            }

            BeginAppendLine();

            if (typeInfo.ExportType == EModifier.Public)
            {
                Append("public ");
            }
            else if (typeInfo.ExportType == EModifier.Protected)
            {
                Append("protected ");
            }
            else
            {
                Append("private ");
            }

            Append("class " + typeInfo.Name);
            EndAppendLine();

            AppendLine("{");
            depth++;
            foreach (var m in typeInfo.Members)
            {
                
                ToMember(m);
            }
            depth--;
            AppendLine("}");

            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                depth--;
                AppendLine("}");
            }
        }

        void ToMember(ULMemberInfo memberInfo)
        {
            BeginAppendLine();

            if (memberInfo.Modifier == EModifier.Public)
            {
                Append("public ");
            }
            else if (memberInfo.Modifier == EModifier.Protected)
            {
                Append("protected ");
            }
            else
            {
                Append("private ");
            }

            if(memberInfo.IsStatic)
            {
                Append("static ");
            }

            Append(memberInfo.Type == null ? "void" :memberInfo.Type.FullName);
            Append(" ");
            Append(memberInfo.Name);
            switch (memberInfo.MemberType)
            {
                case ULMemberInfo.EMemberType.Field:
                    Append(";");
                    break;
                case ULMemberInfo.EMemberType.Property:
                    Append("{ get; set;}");
                    break;
                case ULMemberInfo.EMemberType.Method:
                    Append("()");
                    break;
            }
            EndAppendLine();

            if(memberInfo.MemberType == ULMemberInfo.EMemberType.Method)
            {
                ToStatement(memberInfo.MethodBody);
            }
        }
        void ToBody(ULNodeBlock block)
        {
            AppendLine("{");
            depth++;

            foreach(var s in block.statements)
            {
                ToStatement(s);
            }
            depth--;
            AppendLine("}");

        }

        void ToStatement(ULStatement s)
        {
            if (s == null)
                return;
            if(s is ULNodeBlock)
            {
                ToBody(s as ULNodeBlock);
            }
            else if(s is ULStatementIf)
            {
                ToStatement(s as ULStatementIf);
            }
            else if (s is ULCall)
            {
                ToStatement(s as ULCall);
            }
            else
            {
                Console.Error.WriteLine("unknow statement " + s.GetType().Name);
            }
        }

        void ToStatement(ULStatementIf s)
        {
            AppendLine("if(" + s.condition + ")");
            ToBody(s.trueBlock);
            AppendLine("else");
            ToBody(s.falseBlock);
        }

        void ToStatement(ULCall s)
        {
            BeginAppendLine();
            if(s.callType == ULCall.ECallType.Assign)
            {
                Append(s.Args[0] +" = "+ s.Args[1]+";");
            }
            else
            {
                //Append(s.Name);
                //Append("(");
                //for (int i = 0; i < s.Args.Count; i++)
                //{
                //    Append(s.Args[i]);
                //    if (i != s.Args.Count - 1)
                //    {
                //        Append(",");
                //    }
                //}
                //Append(");");
            }

            EndAppendLine();
        }
    }
}
