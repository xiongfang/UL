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
    public class CompileNode
    {
        public CompileNode Parent;
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

        public virtual ULTypeInfo GetTypeInfo(TypeSyntax typeSyntax) { return null; }
    }

    public class CompileNode_Globle :CompileNode
    {
        public List<string> usingList = new List<string>();

        public List<ULTypeInfo> GetChildrenTypes(CompileNode baseNode)
        {
            List<ULTypeInfo> result = new List<ULTypeInfo>();

            foreach (var c in baseNode.Children)
            {
                if(c is CompileNode_Class)
                {
                    result.Add((c as CompileNode_Class).type);
                    GetChildrenTypes(c);
                }
            }
            return result;
        }

        //public List<ULMemberInfo> GetChildrenMembers(CompileNode baseNode)
        //{
        //    List<ULMemberInfo> result = new List<ULMemberInfo>();

        //    foreach (var c in baseNode.Children)
        //    {
        //        if (c is CompileNode_Class)
        //        {
        //            result.AddRange((c as CompileNode_Class).memberInfos);
        //            GetChildrenTypes(c);
        //        }
        //    }
        //    return result;
        //}
    }

    class CompileNode_Class:CompileNode
    {
        public ULTypeInfo type;
        //public List<ULMemberInfo> memberInfos = new List<ULMemberInfo>();

        public void Compile(ClassDeclarationSyntax classDeclaration)
        {
            type = new ULTypeInfo();

        }

        string GetOrCreateGuid(SyntaxList<AttributeListSyntax> attributeLists)
        {
            foreach (var alist in attributeLists)
            {
                foreach (var a in alist.Attributes)
                {
                    if (a.Name.ToFullString() == "GUID")
                    {
                        LiteralExpressionSyntax exp = a.ArgumentList.Arguments[0].Expression as LiteralExpressionSyntax;
                        return exp.Token.ValueText;
                    }
                }
            }

            return Guid.NewGuid().ToString();
        }

        void ExportClass(ClassDeclarationSyntax c)
        {

            string name = "";
            string nameSpace = "";

            NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
            if (namespaceDeclarationSyntax != null)
            {
                nameSpace = namespaceDeclarationSyntax.Name.ToString();
            }
            else
            {
                nameSpace = "gloable";
            }
            name = c.Identifier.Text;

            //type.ID = GetOrCreateGuid(c.AttributeLists);
            type.Name = name;
            type.Namespace = nameSpace;

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
                ExportProperty(v);
            }


            //导出所有方法
            var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
            foreach (var f in funcNodes)
            {
                //ExportMethod(f);
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
        }

        void ExportProperty(BasePropertyDeclarationSyntax v)
        {
            var v_type = GetTypeInfo(v.Type);

            if (v_type == null)
            {
                Console.Error.WriteLine("无法识别的类型 " + v);
                return;
            }

            string name = "";
            if (v is PropertyDeclarationSyntax)
            {
                name = ((PropertyDeclarationSyntax)v).Identifier.Text;
            }
            else if (v is EventDeclarationSyntax)
            {
                name = ((EventDeclarationSyntax)v).Identifier.Text;
            }
            else if (v is IndexerDeclarationSyntax)
            {
                name = "Index";
            }

            //if (step == ECompilerStet.ScanMember)
            //{
            var property = new ULMemberInfo();
            property.Name = name;
            property.DeclareTypeID = type.ID;
            property.MemberType = ULMemberInfo.EMemberType.Property;
            property.IsStatic = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
            property.Modifier = GetModifier(v.Modifiers);
            property.TypeID = v_type.ID;
            type.Members.Add(property);


            //    foreach (var ve in v.AccessorList.Accessors)
            //    {
            //        var dB_Member = new ULMemberInfo();
            //        dB_Member.DeclareTypeName = currentType.FullName;
            //        dB_Member.TypeName = v_type.FullName;
            //        dB_Member.IsStatic = property.IsStatic;
            //        dB_Member.Modifier = property.Modifier;
            //        //dB_Member.method_abstract = ContainModifier(v.Modifiers, "abstract");
            //        //dB_Member.method_virtual = ContainModifier(v.Modifiers, "virtual");
            //        //dB_Member.method_override = ContainModifier(v.Modifiers, "override");
            //        if (ve.Keyword.Text == "get")
            //        {
            //            dB_Member.MemberType = ULMemberInfo.EMemberType.PropertyGet;

            //            dB_Member.Name = property.Name_PropertyGet;
            //            if (v is IndexerDeclarationSyntax)
            //            {
            //                IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
            //                dB_Member.Args = new List<ULMemberInfo.MethodArg>();
            //                foreach (var a in indexerDeclarationSyntax.ParameterList.Parameters)
            //                {
            //                    dB_Member.Args.Add(GetArgument(a));
            //                }
            //            }
            //        }
            //        else if (ve.Keyword.Text == "set")
            //        {
            //            dB_Member.MemberType = ULMemberInfo.EMemberType.PropertySet;
            //            dB_Member.Name = property.Name_PropertySet;
            //            if (v is IndexerDeclarationSyntax)
            //            {
            //                IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
            //                dB_Member.Args = new List<ULMemberInfo.MethodArg>();
            //                foreach (var a in indexerDeclarationSyntax.ParameterList.Parameters)
            //                {
            //                    dB_Member.Args.Add(GetArgument(a));
            //                }
            //                var arg = new ULMemberInfo.MethodArg();
            //                arg.ArgName = "value";
            //                arg.TypeName = v_type.FullName;
            //                dB_Member.Args.Add(arg);
            //            }
            //            else
            //            {
            //                var arg = new ULMemberInfo.MethodArg();
            //                arg.ArgName = "value";
            //                arg.TypeName = v_type.FullName;
            //                dB_Member.Args.Add(arg);
            //            }
            //        }
            //        else if (ve.Keyword.Text == "add")
            //        {
            //            dB_Member.MemberType = ULMemberInfo.EMemberType.PropertyAdd;
            //            dB_Member.Name = property.Name_PropertyAdd;
            //            var arg = new ULMemberInfo.MethodArg();
            //            arg.ArgName = "value";
            //            arg.TypeName = v_type.FullName;
            //            dB_Member.Args.Add(arg);
            //        }
            //        else if (ve.Keyword.Text == "remove")
            //        {
            //            dB_Member.MemberType = ULMemberInfo.EMemberType.PropertyRemove;
            //            dB_Member.Name = property.Name_PropertyRemove;
            //            var arg = new ULMemberInfo.MethodArg();
            //            arg.ArgName = "value";
            //            arg.TypeName = v_type.FullName;
            //            dB_Member.Args.Add(arg);
            //        }
            //        currentType.Members.Add(dB_Member);
            //    }

            //}
            //else if (step == ECompilerStet.Compile)
            //{
            //    currentMember = currentType.Members.Find(m => m.Name == name);
            //    foreach (var ve in v.AccessorList.Accessors)
            //    {
            //        if (ve.Keyword.Text == "get")
            //        {
            //            currentMember = currentType.Members.Find(m => m.Name == currentMember.Name_PropertyGet);
            //            ExportBody(ve.Body);
            //        }
            //        else if(ve.Keyword.Text == "set")
            //        {
            //            currentMember = currentType.Members.Find(m => m.Name == currentMember.Name_PropertySet);
            //            ExportBody(ve.Body);
            //        }
            //        else if(ve.Keyword.Text == "add")
            //        {
            //            currentMember = currentType.Members.Find(m => m.Name == currentMember.Name_PropertyAdd);
            //            ExportBody(ve.Body);
            //        }
            //        else if (ve.Keyword.Text == "remove")
            //        {
            //            currentMember = currentType.Members.Find(m => m.Name == currentMember.Name_PropertyRemove);
            //            ExportBody(ve.Body);
            //        }
            //    }
            //}
        }

        void ExportVariable(BaseFieldDeclarationSyntax v)
        {
            var vtype = GetTypeInfo(v.Declaration.Type);

            foreach (var ve in v.Declaration.Variables)
            {
                var dB_Member = new ULMemberInfo();
                dB_Member.Name = ve.Identifier.Text;
                dB_Member.IsStatic = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                dB_Member.DeclareTypeID = type.ID;
                dB_Member.TypeID = vtype.ID;

                if (v is FieldDeclarationSyntax)
                    dB_Member.MemberType = ULMemberInfo.EMemberType.Field;
                else if (v is EventFieldDeclarationSyntax)
                {
                    dB_Member.MemberType = ULMemberInfo.EMemberType.Event;
                }
                else
                {
                    Console.Error.WriteLine("无法识别的类成员 " + v);
                }
                dB_Member.Modifier = GetModifier(v.Modifiers);
                //if (ve.Initializer != null)
                //    dB_Member.field_initializer = ExportExp(ve.Initializer.Value);

                //dB_Member.attributes = ExportAttributes(v.AttributeLists);
                //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                //Model.AddMember(type.static_full_name, dB_Member);
                type.Members.Add(dB_Member);
            }
        }

        static bool ContainModifier(SyntaxTokenList Modifiers, string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }
        static EModifier GetModifier(SyntaxTokenList Modifiers)
        {
            bool isPublic = ContainModifier(Modifiers, "public");
            bool isProtected = ContainModifier(Modifiers, "protected");
            bool isPrivate = !isPublic && !isProtected;

            if (isProtected)
                return EModifier.Protected;
            if (isPublic)
                return EModifier.Public;
            if (isPrivate)
                return EModifier.Private;

            return EModifier.Private;
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
                case "void":
                    return "";
                default:
                    return kw;
            }
        }
    }

    public class CSToUL
    {
        public static CompileNode_Globle Convert(string code)
        {
            CompileNode_Globle globleNode = new CompileNode_Globle();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
            //导出所有类
            var classNodes = nodes.OfType<MemberDeclarationSyntax>();


            if (root.Usings != null)
            {
                foreach (var u in root.Usings)
                {
                    globleNode.usingList.Add(u.Name.ToString());
                }
            }

            foreach (var c in classNodes)
            {
                if (c is ClassDeclarationSyntax)
                {
                    var classNode = new CompileNode_Class();
                    classNode.Parent = globleNode;
                    globleNode.Children.Add(classNode);
                    classNode.Compile(c as ClassDeclarationSyntax);
                }
                
            }

            return globleNode;
        }
    }
}
