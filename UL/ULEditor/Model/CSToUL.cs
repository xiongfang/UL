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

            var cs = new CSToUL();
            cs.step = ECompilerStet.ScanType;

            foreach (var c in classNodes)
            {
                cs.ExportType(c);
            }
            cs.step = ECompilerStet.ScanMember;
            foreach (var c in classNodes)
            {
                cs.ExportType(c);
            }
            cs.step = ECompilerStet.Compile;
            foreach (var c in classNodes)
            {
                cs.ExportType(c);
            }

            results.AddRange(cs.type_list);

            return results;
        }

        enum ECompilerStet
        {
            ScanType,
            ScanMember,
            Compile
        }

        ECompilerStet step;
        List<ULTypeInfo> type_list = new List<ULTypeInfo>();
        Stack<ULTypeInfo> types = new Stack<ULTypeInfo>();

        ULTypeInfo current { get { return types.Peek(); } }

        void ExportType(MemberDeclarationSyntax c)
        {
            if (c is ClassDeclarationSyntax)
            {
                ExportClass(c as ClassDeclarationSyntax);
            }
            else if (c is StructDeclarationSyntax)
            {
                ExportStruct(c as StructDeclarationSyntax);
            }
            else if (c is InterfaceDeclarationSyntax)
            {
                ExportInterface(c as InterfaceDeclarationSyntax);
            }
            else if (c is EnumDeclarationSyntax)
            {
                ExportEnum(c as EnumDeclarationSyntax);
            }
            else if (c is DelegateDeclarationSyntax)
            {
                ExportDelegate(c as DelegateDeclarationSyntax);
            }
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
                nameSpace = ModelData.GloableNamespaceName;
            }
            name = c.Identifier.Text;



            if (step == ECompilerStet.ScanType)
            {
                type_list.Add(new ULTypeInfo());
                types.Push(type_list[type_list.Count - 1]);

                current.Namespace = nameSpace;
                current.Name = name;

                ModelData.UpdateType(current);
            }
            else
            {
                types.Push(ModelData.FindTypeByFullName(nameSpace+"."+name));
            }

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


            types.Pop();


        }

        void ExportProperty(BasePropertyDeclarationSyntax v)
        {
            var v_type = GetType(v.Type);

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

            if (step == ECompilerStet.ScanMember)
            {
                var property = new ULMemberInfo();
                property.Name = name;
                property.DeclareTypeName = current.FullName;
                property.MemberType = ULMemberInfo.EMemberType.Property;
                property.IsStatic = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                property.Modifier = GetModifier(v.Modifiers);
                property.TypeName = v_type.FullName;
                //foreach (var ve in v.AccessorList.Accessors)
                //{
                //    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                //    dB_Member.declaring_type = type.static_full_name;
                //    dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                //    dB_Member.is_static = property.is_static;
                //    dB_Member.modifier = property.modifier;
                //    dB_Member.method_abstract = ContainModifier(v.Modifiers, "abstract");
                //    dB_Member.method_virtual = ContainModifier(v.Modifiers, "virtual");
                //    dB_Member.method_override = ContainModifier(v.Modifiers, "override");
                //    if (ve.Keyword.Text == "get")
                //    {
                //        dB_Member.type = v_type.GetRefType();
                //        dB_Member.name = property.property_get;
                //        dB_Member.method_is_property_get = true;
                //        if (v is IndexerDeclarationSyntax)
                //        {
                //            IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
                //            List<Metadata.DB_Member.Argument> args = new List<Metadata.DB_Member.Argument>();
                //            foreach (var a in indexerDeclarationSyntax.ParameterList.Parameters)
                //            {
                //                args.Add(GetArgument(a));
                //            }
                //            dB_Member.method_args = args.ToArray();
                //        }
                //        else
                //        {
                //            dB_Member.method_args = new Metadata.DB_Member.Argument[0];
                //        }

                //    }
                //    else if (ve.Keyword.Text == "set")
                //    {
                //        dB_Member.method_is_property_set = true;
                //        dB_Member.name = property.property_set;
                //        if (v is IndexerDeclarationSyntax)
                //        {
                //            IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
                //            List<Metadata.DB_Member.Argument> args = new List<Metadata.DB_Member.Argument>();
                //            foreach (var a in indexerDeclarationSyntax.ParameterList.Parameters)
                //            {
                //                args.Add(GetArgument(a));
                //            }
                //            Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                //            arg.name = "value";
                //            arg.type = v_type.GetRefType();
                //            args.Add(arg);
                //            dB_Member.method_args = args.ToArray();
                //        }
                //        else
                //        {
                //            Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                //            arg.name = "value";
                //            arg.type = v_type.GetRefType();
                //            dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                //        }
                //    }
                //    else if (ve.Keyword.Text == "add")
                //    {
                //        dB_Member.method_is_event_add = true;
                //        dB_Member.name = property.property_add;
                //        Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                //        arg.name = "value";
                //        arg.type = v_type.GetRefType();
                //        dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                //    }
                //    else if (ve.Keyword.Text == "remove")
                //    {
                //        dB_Member.method_is_event_remove = true;
                //        dB_Member.name = property.property_remove;
                //        Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                //        arg.name = "value";
                //        arg.type = v_type.GetRefType();
                //        dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                //    }
                //    Model.AddMember(type.static_full_name, dB_Member);
                //}
                current.Members.Add(property);
            }
        }

        void ExportVariable(BaseFieldDeclarationSyntax v)
        {
            if(step == ECompilerStet.ScanMember)
            {
                var vtype = GetType(v.Declaration.Type);

                foreach (var ve in v.Declaration.Variables)
                {
                    var dB_Member = new ULMemberInfo();
                    dB_Member.Name = ve.Identifier.Text;
                    dB_Member.IsStatic = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                    dB_Member.DeclareTypeName = current.FullName;
                    dB_Member.TypeName = vtype.FullName;

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
                    current.Members.Add(dB_Member);
                }
            }
            
            
        }
        static bool ContainModifier(SyntaxTokenList Modifiers, string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }
        static EModifier GetModifier(SyntaxTokenList Modifiers)
        {
            bool isPublic =  ContainModifier(Modifiers, "public");
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
            if(step == ECompilerStet.ScanMember)
            {
                MethodDeclarationSyntax method = v as MethodDeclarationSyntax;
                var methodInfo = new Model.ULMemberInfo();
                methodInfo.DeclareTypeName = this.current.FullName;
                methodInfo.Name = method.Identifier.ValueText;
                methodInfo.IsStatic = ContainModifier(method.Modifiers, "static");
                methodInfo.Modifier = GetModifier(method.Modifiers);
                var memberType = GetType(method.ReturnType);
                methodInfo.TypeName = memberType != null ? memberType.FullName : "";
                methodInfo.MemberType = ULMemberInfo.EMemberType.Method;
                current.Members.Add(methodInfo);
            }

        }

        void ExportStruct(StructDeclarationSyntax c)
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
                nameSpace = ModelData.GloableNamespaceName;
            }
            name = c.Identifier.Text;



            if (step == ECompilerStet.ScanType)
            {
                type_list.Add(new ULTypeInfo());
                types.Push(type_list[type_list.Count - 1]);

                current.Namespace = nameSpace;
                current.Name = name;
                current.IsValueType = true;
                ModelData.UpdateType(current);
            }
            else
            {
                types.Push(ModelData.FindTypeByFullName(nameSpace + "." + name));
            }

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
            types.Pop();
        }
        void ExportInterface(InterfaceDeclarationSyntax c)
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
                nameSpace = ModelData.GloableNamespaceName;
            }
            name = c.Identifier.Text;



            if (step == ECompilerStet.ScanType)
            {
                type_list.Add(new ULTypeInfo());
                types.Push(type_list[type_list.Count - 1]);

                current.Namespace = nameSpace;
                current.Name = name;
                current.IsInterface = true;
                ModelData.UpdateType(current);
            }
            else
            {
                types.Push(ModelData.FindTypeByFullName(nameSpace + "." + name));
            }

            ////导出所有变量
            //var virableNodes = c.ChildNodes().OfType<BaseFieldDeclarationSyntax>();
            //foreach (var v in virableNodes)
            //{
            //    ExportVariable(v);
            //}

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

            types.Pop();
        }

        void ExportEnum(EnumDeclarationSyntax c)
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
                nameSpace = ModelData.GloableNamespaceName;
            }
            name = c.Identifier.Text;

            if (step == ECompilerStet.ScanType)
            {
                type_list.Add(new ULTypeInfo());
                types.Push(type_list[type_list.Count - 1]);

                current.Namespace = nameSpace;
                current.Name = name;
                current.IsEnum = true;
                ModelData.UpdateType(current);
            }
            else
            {
                types.Push(ModelData.FindTypeByFullName(nameSpace + "." + name));
            }

            if(step == ECompilerStet.ScanMember)
            {
                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<EnumMemberDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    var dB_Member = new ULMemberInfo();
                    dB_Member.Name = v.Identifier.Text;
                    dB_Member.IsStatic = false;
                    dB_Member.DeclareTypeName = current.FullName;
                    dB_Member.MemberType = ULMemberInfo.EMemberType.Enum;

                    current.Members.Add(dB_Member);
                }
            }

        }
        void ExportDelegate(DelegateDeclarationSyntax c)
        {

        }

    }
}
