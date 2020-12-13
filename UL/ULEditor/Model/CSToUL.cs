using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class CSToUL
    {


        public static List<ULTypeInfo> Convert(string code)
        {
            List<ULTypeInfo> results = new List<ULTypeInfo>();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
            //导出所有类
            var classNodes = nodes.OfType<MemberDeclarationSyntax>();
            foreach (var c in classNodes)
            {
                var type = ExportType(c);
                if(type!=null)
                    results.Add(type);
            }

            return results;
        }


        ULTypeInfo type;

        static ULTypeInfo ExportType(MemberDeclarationSyntax c)
        {
            var cs = new CSToUL();

            if (c is ClassDeclarationSyntax)
            {
                return cs.ExportClass(c as ClassDeclarationSyntax);
            }
            else if (c is StructDeclarationSyntax)
            {
                return cs.ExportStruct(c as StructDeclarationSyntax);
            }
            else if (c is InterfaceDeclarationSyntax)
            {
                return cs.ExportInterface(c as InterfaceDeclarationSyntax);
            }
            else if (c is EnumDeclarationSyntax)
            {
                return cs.ExportEnum(c as EnumDeclarationSyntax);
            }
            else if (c is DelegateDeclarationSyntax)
            {
                return cs.ExportDelegate(c as DelegateDeclarationSyntax);
            }

            return null;
        }


        public CSToUL()
        {
            type = new ULTypeInfo();
        }

        string GetOrCreateGuid(ClassDeclarationSyntax c)
        {
            foreach(var alist in c.AttributeLists)
            {
                foreach(var a in alist.Attributes)
                {
                    if(a.Name.ToFullString() == "GUID")
                    {
                        LiteralExpressionSyntax exp = a.ArgumentList.Arguments[0].Expression as LiteralExpressionSyntax;
                        return exp.Token.ValueText;
                    }
                }
            }

            return Guid.NewGuid().ToString();
        }

        ULTypeInfo ExportClass(ClassDeclarationSyntax c)
        {
            NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
            if (namespaceDeclarationSyntax != null)
            {
                type.Namespace = namespaceDeclarationSyntax.Name.ToString();
            }
            else
            {
                type.Namespace = ModelData.GloableNamespaceName;
            }
            type.Name = c.Identifier.Text;

            //导出所有变量
            var virableNodes = c.ChildNodes().OfType<BaseFieldDeclarationSyntax>();
            foreach (var v in virableNodes)
            {
                ExportVariable(v);
            }

            //导出所有属性
            var propertyNodes = c.ChildNodes().OfType<BasePropertyDeclarationSyntax>();
            foreach (var v in propertyNodes)
            {
                //ExportProperty(v, type);
            }


            //导出所有方法
            var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
            foreach (var f in funcNodes)
            {
                ExportMethod(f);
            }

            var operatorNodes = c.ChildNodes().OfType<OperatorDeclarationSyntax>();
            foreach (var f in operatorNodes)
            {
                //ExportOperator(f, type);
            }
            var conversion_operatorNodes = c.ChildNodes().OfType<ConversionOperatorDeclarationSyntax>();
            foreach (var f in conversion_operatorNodes)
            {
                //ExportConversionOperator(f, type);
            }
            return type;
        }

        void ExportVariable(BaseFieldDeclarationSyntax v)
        {
            
        }
        static bool ContainModifier(SyntaxTokenList Modifiers, string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }
        static EExportScope GetModifier(SyntaxTokenList Modifiers)
        {
            bool isPublic =  ContainModifier(Modifiers, "public");
            bool isProtected = ContainModifier(Modifiers, "protected");
            bool isPrivate = !isPublic && !isProtected;

            if (isProtected)
                return EExportScope.Protected;
            if (isPublic)
                return EExportScope.Public;
            if (isPrivate)
                return EExportScope.Private;

            return EExportScope.Private;
        }
        static string GetKeywordTypeName(string kw)
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
                default:
                    return "Void";
            }
        }

        public static ULTypeInfo GetType(TypeSyntax typeSyntax)
        {
            if (typeSyntax == null)
                return null;

            if (typeSyntax is PredefinedTypeSyntax)
            {
                PredefinedTypeSyntax predefinedTypeSyntax = typeSyntax as PredefinedTypeSyntax;
                string typeName = GetKeywordTypeName(predefinedTypeSyntax.Keyword.Text);
                return Model.ModelData.FindTypeByFullName("System."+typeName);
            }
            //else if (typeSyntax is ArrayTypeSyntax)
            //{
            //    ArrayTypeSyntax ts = typeSyntax as ArrayTypeSyntax;
            //    Metadata.Expression.TypeSyntax elementType = GetTypeSyntax(ts.ElementType, "");
            //    List<Metadata.Expression.TypeSyntax> parameters = new List<Metadata.Expression.TypeSyntax>();
            //    parameters.Add(elementType);
            //    Metadata.Expression.TypeSyntax gns = new Metadata.Expression.TypeSyntax();
            //    gns.Name = "ArrayT";
            //    gns.args = parameters.ToArray();
            //    gns.isGenericType = true;
            //    if (elementType.isGenericParameter)
            //    {
            //        gns.isGenericTypeDefinition = true;
            //    }
            //    //Metadata.Expression.QualifiedNameSyntax qns = new Metadata.Expression.QualifiedNameSyntax();
            //    //qns.Left = new Metadata.Expression.IdentifierNameSyntax() { Identifier = "System" };
            //    //qns.Right = gns;
            //    gns.name_space = "System";
            //    //Metadata.DB_Type arrayType = Model.GetType("System.ArrayT[1]");
            //    return gns;
            //}
            //else if (typeSyntax is IdentifierNameSyntax)
            //{
            //    IdentifierNameSyntax ts = typeSyntax as IdentifierNameSyntax;
            //    Metadata.DB_Type type = Model.Instance.GetIndifierInfo(ts.Identifier.Text, ns, Metadata.Model.EIndifierFlag.IF_Type).type;


            //    //Metadata.DB_Type type = Model.Instance.GetIndifierInfo(Identifier).type;
            //    //Model.Instance.GetIndifierInfo(ts.Identifier.Text).type;
            //    if (type.is_generic_paramter)
            //    {
            //        Metadata.Expression.TypeSyntax ins = new Metadata.Expression.TypeSyntax();

            //        ins.Name = (ts.Identifier.Text);
            //        ins.name_space = type._namespace;
            //        ins.isGenericParameter = true;
            //        //ins.declare_type = type.static_full_name;
            //        return ins;
            //    }
            //    else
            //    {
            //        Metadata.Expression.TypeSyntax ins = new Metadata.Expression.TypeSyntax();

            //        ins.Name = (ts.Identifier.Text);
            //        ins.name_space = type._namespace;
            //        return ins;
            //    }

            //}
            //else if (typeSyntax is GenericNameSyntax)
            //{
            //    GenericNameSyntax ts = typeSyntax as GenericNameSyntax;
            //    string Name = ts.Identifier.Text;
            //    List<Metadata.Expression.TypeSyntax> parameters = new List<Metadata.Expression.TypeSyntax>();
            //    foreach (var p in ts.TypeArgumentList.Arguments)
            //    {
            //        parameters.Add(GetTypeSyntax(p, ""));
            //    }
            //    Metadata.Expression.TypeSyntax gns = new Metadata.Expression.TypeSyntax();
            //    gns.Name = Name;
            //    gns.args = parameters.ToArray();
            //    gns.isGenericType = true;
            //    Metadata.DB_Type type = Model.Instance.GetIndifierInfo(gns.GetTypeDefinitionName()).type;
            //    gns.name_space = type._namespace;
            //    return gns;
            //}
            //else if (typeSyntax is QualifiedNameSyntax)
            //{
            //    QualifiedNameSyntax qns = typeSyntax as QualifiedNameSyntax;
            //    string name_space = qns.Left.ToString();
            //    if (!string.IsNullOrEmpty(ns))
            //    {
            //        ns = ns + "." + name_space;
            //    }
            //    else
            //    {
            //        ns = name_space;
            //    }
            //    //Metadata.Expression.QualifiedNameSyntax my_qns = new Metadata.Expression.QualifiedNameSyntax();
            //    //my_qns.Left = GetTypeSyntax(qns.Left) as Metadata.Expression.NameSyntax;
            //    Metadata.Expression.TypeSyntax ts = GetTypeSyntax(qns.Right, ns);
            //    ts.name_space = ns;
            //    return ts;
            //}
            else
            {
                Console.Error.WriteLine("不支持的类型语法 " + typeSyntax.GetType().FullName);
            }

            return null;
        }

        void ExportMethod(BaseMethodDeclarationSyntax v)
        {
            MethodDeclarationSyntax method = v as MethodDeclarationSyntax;
            var methodInfo = new Model.ULMemberInfo();
            methodInfo.ReflectTypeName = this.type.FullName;
            methodInfo.Name = method.Identifier.ValueText;
            methodInfo.IsStatic = ContainModifier(method.Modifiers, "static");
            methodInfo.ExportType = GetModifier(method.Modifiers);
            var memberType = GetType(method.ReturnType);
            methodInfo.MemberTypeName = memberType!=null? memberType.FullName:"";
            methodInfo.MemberMark = ULMemberInfo.EMemberMark.Method;
            type.Members.Add(methodInfo);
        }

        ULTypeInfo ExportStruct(StructDeclarationSyntax c)
        {
            return type;
        }
        ULTypeInfo ExportInterface(InterfaceDeclarationSyntax c)
        {
            return type;
        }
        ULTypeInfo ExportEnum(EnumDeclarationSyntax c)
        {
            return type;
        }
        ULTypeInfo ExportDelegate(DelegateDeclarationSyntax c)
        {
            return type;
        }

    }
}
