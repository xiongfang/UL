﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class ULToLua
    {
        public readonly static string[] keywords = new string[]
        {
            "local",
            "function",
            "table",
            "class"

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
            var sb = new ULToLua();
            sb.typeInfo = typeInfo;
            sb.ToType();

            return sb.ToString();
        }

        void ToType()
        {
            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                AppendLine("table " + typeInfo.Namespace);
                AppendLine("{");
                depth++;
            }

            //AppendLine(string.Format("[GUID(\"{0}\")]", typeInfo.Guid));

            BeginAppendLine();

            //if (typeInfo.ExportType == EExportType.Public)
            //{
            //    Append("public ");
            //}
            //else if (typeInfo.ExportType == EExportType.Protected)
            //{
            //    Append("protected ");
            //}
            //else
            //{
            //    Append("private ");
            //}

            Append("table " + typeInfo.Name);
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

            //if (memberInfo.ExportType == EExportType.Public)
            //{
            //    Append("public ");
            //}
            //else if (memberInfo.ExportType == EExportType.Protected)
            //{
            //    Append("protected ");
            //}
            //else
            //{
            //    Append("private ");
            //}

            Append("function ");

            if (memberInfo.IsStatic)
            {
                Append("static ");
            }

            
            Append(" ");
            Append(memberInfo.Name);
            switch (memberInfo.MemberType)
            {
                case ULMemberInfo.EMemberType.Field:
                    Append(";");
                    break;
                //case ULMemberInfo.EMemberType.Property:
                //    Append("{ get; set;}");
                //    break;
                case ULMemberInfo.EMemberType.Method:
                    Append("(");
                    if(!memberInfo.IsStatic)
                    {
                        Append("self");
                        if(memberInfo.Args.Count>0)
                        {
                            Append(",");
                        }
                    }

                    for(int i=0;i<memberInfo.Args.Count;i++)
                    {
                        Append(memberInfo.Args[i].ArgName);
                        if(i!=memberInfo.Args.Count-1)
                        {
                            Append(",");
                        }
                    }

                    Append(")");
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
        }

    }
}
