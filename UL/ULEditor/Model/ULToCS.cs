﻿using System;
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

            if (typeInfo.ExportType == EExportScope.Public)
            {
                Append("public ");
            }
            else if (typeInfo.ExportType == EExportScope.Protected)
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

            if (memberInfo.ExportType == EExportScope.Public)
            {
                Append("public ");
            }
            else if (memberInfo.ExportType == EExportScope.Protected)
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

            Append(memberInfo.MemberType == null ? "void" :memberInfo.MemberType.FullName);
            Append(" ");
            Append(memberInfo.Name);
            switch (memberInfo.MemberMark)
            {
                case ULMemberInfo.EMemberMark.Field:
                    Append(";");
                    break;
                case ULMemberInfo.EMemberMark.Property:
                    Append("{ get; set;}");
                    break;
                case ULMemberInfo.EMemberMark.Method:
                    Append("()");
                    break;
            }
            EndAppendLine();

            if(memberInfo.MemberMark == ULMemberInfo.EMemberMark.Method)
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

        void ToStatement(UIStatement s)
        {
            if (s == null)
                return;
            if(s is ULNodeBlock)
            {
                ToBody(s as ULNodeBlock);
            }
        }

    }
}
