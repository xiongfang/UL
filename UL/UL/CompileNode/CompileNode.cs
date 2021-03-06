using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace UL.CompileNode
{
    public class CompileNode
    {
        CompileNode _parent;
        public CompileNode Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                if (_parent != null)
                {
                    _parent.Children.Add(this);
                }
            }
        }
        public List<CompileNode> Children = new List<CompileNode>();


        public class IdentifierInfo
        {
            public enum EIdentifierType
            {
                Local,
                Arg,
                Field,
                Method,
                Type,
                Namesapce
            }
            public EIdentifierType type;
            public string TypeFullName;
        }

        public virtual IdentifierInfo GetIdentifierInfo(string identifier)
        {
            return null;
        }

        protected static string GetKeywordTypeName(string kw)
        {
            switch (kw)
            {
                case "char":
                    return "Char";
                case "sbyte":
                    return "SByte";
                case "int":
                    return "Int32";
                case "string":
                    return "String";
                case "short":
                    return "Int16";
                case "byte":
                    return "Byte";
                case "float":
                    return "Single";
                case "double":
                    return "Double";
                case "object":
                    return "Object";
                case "bool":
                    return "Boolean";
                case "uint":
                    return "UInt32";
                case "ulong":
                    return "UInt64";
                case "long":
                    return "Int64";
                case "ushort":
                    return "UInt16";
                case "void":
                    return "";
                default:
                    return kw;
            }
        }
        public virtual ULTypeInfo GetTypeInfo(TypeSyntax typeSyntax)
        {
            if (typeSyntax == null)
                return null;

            if (typeSyntax is PredefinedTypeSyntax)
            {
                PredefinedTypeSyntax predefinedTypeSyntax = typeSyntax as PredefinedTypeSyntax;
                string typeName = GetKeywordTypeName(predefinedTypeSyntax.Keyword.Text);
                return Data.GetType("System." + typeName);
            }
            else if (typeSyntax is IdentifierNameSyntax)
            {
                IdentifierNameSyntax ts = typeSyntax as IdentifierNameSyntax;
                var identifier = ts.Identifier.Text;
                var info = GetIdentifierInfo(identifier);
                if (info != null)
                {
                    return Data.GetType(info.TypeFullName);
                }
            }
            else if (typeSyntax is QualifiedNameSyntax)
            {
                QualifiedNameSyntax qns = typeSyntax as QualifiedNameSyntax;
                string name_space = qns.Left.ToString();
                var name = qns.Right.Identifier.Text;
                //Metadata.Expression.QualifiedNameSyntax my_qns = new Metadata.Expression.QualifiedNameSyntax();
                //my_qns.Left = GetTypeSyntax(qns.Left) as Metadata.Expression.NameSyntax;

                return Data.GetType(name_space + "." + name);
            }
            else
            {
                Console.Error.WriteLine("不支持的类型语法 " + typeSyntax.GetType().FullName);
            }
            return null;
        }
    }
}
