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

        ULTypeInfo currentType { get { return types.Peek(); } }

        ULMemberInfo currentMember;

        class CallFrameInfo
        {
            public CallFrameInfo preFrame;
            public ULNodeBlock block;
            public Dictionary<string, string> variables = new Dictionary<string, string>();
        }
        Stack<CallFrameInfo> frames = new Stack<CallFrameInfo>();
        ULNodeBlock currentBlock
        {
            get
            {
                if (frames.Count == 0)
                    return null;
                var f = frames.Peek();
                while (f != null && f.block == null)
                {
                    f = f.preFrame;
                }
                if (f != null)
                    return f.block;
                return null;
            }
        }
        ULNodeBlock preBlock { get { return currentBlock.Parent; } }

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
            foreach (var alist in c.AttributeLists)
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
                nameSpace = ModelData.GloableNamespaceName;
            }
            name = c.Identifier.Text;



            if (step == ECompilerStet.ScanType)
            {
                type_list.Add(new ULTypeInfo());
                types.Push(type_list[type_list.Count - 1]);

                currentType.Namespace = nameSpace;
                currentType.Name = name;

                ModelData.UpdateType(currentType);
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
                property.DeclareTypeName = currentType.FullName;
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
                currentType.Members.Add(property);
            }
        }

        void ExportVariable(BaseFieldDeclarationSyntax v)
        {
            if (step == ECompilerStet.ScanMember)
            {
                var vtype = GetType(v.Declaration.Type);

                foreach (var ve in v.Declaration.Variables)
                {
                    var dB_Member = new ULMemberInfo();
                    dB_Member.Name = ve.Identifier.Text;
                    dB_Member.IsStatic = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                    dB_Member.DeclareTypeName = currentType.FullName;
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
                    currentType.Members.Add(dB_Member);
                }
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

        public static ULTypeInfo GetType(TypeSyntax typeSyntax)
        {
            if (typeSyntax == null)
                return null;

            if (typeSyntax is PredefinedTypeSyntax)
            {
                PredefinedTypeSyntax predefinedTypeSyntax = typeSyntax as PredefinedTypeSyntax;
                string typeName = GetKeywordTypeName(predefinedTypeSyntax.Keyword.Text);
                return Model.ModelData.FindTypeByFullName("System." + typeName);
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

        static Dictionary<BaseMethodDeclarationSyntax, ULMemberInfo> MemberMap = new Dictionary<BaseMethodDeclarationSyntax, ULMemberInfo>();
        void ExportMethod(BaseMethodDeclarationSyntax v)
        {
            if (step == ECompilerStet.ScanMember)
            {
                MethodDeclarationSyntax method = v as MethodDeclarationSyntax;

                var methodInfo = new Model.ULMemberInfo();
                methodInfo.DeclareTypeName = this.currentType.FullName;
                methodInfo.Name = method.Identifier.ValueText;
                methodInfo.IsStatic = ContainModifier(method.Modifiers, "static");
                methodInfo.Modifier = GetModifier(method.Modifiers);
                var memberType = GetType(method.ReturnType);
                methodInfo.TypeName = memberType != null ? memberType.FullName : "";
                methodInfo.MemberType = ULMemberInfo.EMemberType.Method;
                currentType.Members.Add(methodInfo);
                MemberMap[v] = methodInfo;
            }
            else if (step == ECompilerStet.Compile)
            {
                currentMember = MemberMap[v];
                ExportBody(v.Body);
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

                currentType.Namespace = nameSpace;
                currentType.Name = name;
                currentType.IsValueType = true;
                ModelData.UpdateType(currentType);
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

                currentType.Namespace = nameSpace;
                currentType.Name = name;
                currentType.IsInterface = true;
                ModelData.UpdateType(currentType);
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

                currentType.Namespace = nameSpace;
                currentType.Name = name;
                currentType.IsEnum = true;
                ModelData.UpdateType(currentType);
            }
            else
            {
                types.Push(ModelData.FindTypeByFullName(nameSpace + "." + name));
            }

            if (step == ECompilerStet.ScanMember)
            {
                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<EnumMemberDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    var dB_Member = new ULMemberInfo();
                    dB_Member.Name = v.Identifier.Text;
                    dB_Member.IsStatic = false;
                    dB_Member.DeclareTypeName = currentType.FullName;
                    dB_Member.MemberType = ULMemberInfo.EMemberType.Enum;

                    currentType.Members.Add(dB_Member);
                }
            }

            types.Pop();
        }
        void ExportDelegate(DelegateDeclarationSyntax c)
        {

        }


        void ExportBody(BlockSyntax bs)
        {
            ExportStatement(bs);
        }

        void ExportStatement(StatementSyntax node)
        {
            if (node is IfStatementSyntax)
            {
                ExportStatement(node as IfStatementSyntax);
            }
            else if (node is ExpressionStatementSyntax)
            {
                ExportStatement(node as ExpressionStatementSyntax);
            }
            else if (node is BlockSyntax)
            {
                ExportStatement(node as BlockSyntax);
            }
            //else if (node is LocalDeclarationStatementSyntax)
            //{
            //    ExportStatement(node as LocalDeclarationStatementSyntax);
            //}
            //else if (node is ForStatementSyntax)
            //{
            //    ExportStatement(node as ForStatementSyntax);
            //}
            //else if (node is DoStatementSyntax)
            //{
            //    ExportStatement(node as DoStatementSyntax);
            //}
            else if (node is WhileStatementSyntax)
            {
                ExportStatement(node as WhileStatementSyntax);
            }
            //else if (node is SwitchStatementSyntax)
            //{
            //    ExportStatement(node as SwitchStatementSyntax);
            //}
            //else if (node is BreakStatementSyntax)
            //{
            //    ExportStatement(node as BreakStatementSyntax);
            //}
            //else if (node is ReturnStatementSyntax)
            //{
            //    ExportStatement(node as ReturnStatementSyntax);
            //}
            //else if (node is TryStatementSyntax)
            //{
            //    ExportStatement(node as TryStatementSyntax);
            //}
            //else if (node is ThrowStatementSyntax)
            //{
            //    ExportStatement(node as ThrowStatementSyntax);
            //}
            else
            {
                Console.Error.WriteLine("error:Unsopproted StatementSyntax" + node);
            }
        }

        ULNodeBlock ExportStatement(BlockSyntax node)
        {
            var frame = new CallFrameInfo();

            frame.block = new ULNodeBlock();

            if (frames.Count > 0)
            {
                frame.preFrame = frames.Peek();
            }
            else
            {
                currentMember.MethodBody = frame.block;
            }

            frame.block.Parent = currentBlock;
            frames.Push(frame);
            foreach (var s in node.Statements)
            {
                ExportStatement(s);
            }
            frames.Pop();

            return frame.block;
        }

        void ExportStatement(IfStatementSyntax node)
        {
            var cond = ExportExp(node.Condition);

            var ifStatement = new ULStatementIf();
            ifStatement.Parent = currentBlock;
            ifStatement.condition = cond.GetOutputName(0);
            if (node.Statement is BlockSyntax)
                ifStatement.trueBlock = ExportStatement(node.Statement as BlockSyntax);
            if (node.Else.Statement is BlockSyntax)
                ifStatement.falseBlock = ExportStatement(node.Else.Statement as BlockSyntax);
        }
        void ExportStatement(WhileStatementSyntax node)
        {
            var cond = ExportExp(node.Condition);

            var ifStatement = new ULStatementWhile();
            ifStatement.Parent = currentBlock;
            ifStatement.condition = cond.GetOutputName(0);
            if (node.Statement is BlockSyntax)
                ifStatement.block = ExportStatement(node.Statement as BlockSyntax);
        }

        void ExportStatement(ExpressionStatementSyntax node)
        {
            ExportExp(node.Expression);
        }

        ULCall ExportExp(ExpressionSyntax es)
        {
            if (es is LiteralExpressionSyntax)
            {
                return ExportExp(es as LiteralExpressionSyntax);
            }
            else if (es is ThisExpressionSyntax)
            {
                return ExportExp(es as ThisExpressionSyntax);
            }
            else if (es is ObjectCreationExpressionSyntax)
            {
                return ExportExp(es as ObjectCreationExpressionSyntax);
            }
            else if (es is InvocationExpressionSyntax)
            {
                return ExportExp(es as InvocationExpressionSyntax);
            }
            else if (es is MemberAccessExpressionSyntax)
            {
                return ExportExp(es as MemberAccessExpressionSyntax);
            }
            else if (es is IdentifierNameSyntax)
            {
                return ExportExp(es as IdentifierNameSyntax);
            }
            else if (es is AssignmentExpressionSyntax)
            {
                return ExportExp(es as AssignmentExpressionSyntax);
            }
            else if (es is BinaryExpressionSyntax)
            {
                return ExportExp(es as BinaryExpressionSyntax);
            }
            else if (es is PostfixUnaryExpressionSyntax)
            {
                return ExportExp(es as PostfixUnaryExpressionSyntax);
            }
            else if (es is ArrayCreationExpressionSyntax)
            {
                return ExportExp(es as ArrayCreationExpressionSyntax);
            }
            else if (es is PrefixUnaryExpressionSyntax)
            {
                return ExportExp(es as PrefixUnaryExpressionSyntax);
            }
            else if (es is BaseExpressionSyntax)
            {
                return ExportExp(es as BaseExpressionSyntax);
            }
            //else if (es is ThrowExpressionSyntax)
            //{
            //    return ExportExp(es as ThrowExpressionSyntax);
            //}
            else if (es is ParenthesizedExpressionSyntax)
            {
                return ExportExp(es as ParenthesizedExpressionSyntax);
            }
            else if (es is ElementAccessExpressionSyntax)
            {
                return ExportExp(es as ElementAccessExpressionSyntax);
            }
            else
            {
                Console.Error.WriteLine(string.Format("error:不支持的表达式 {0} {1}", es.GetType().Name, es.ToString()));
            }
            return null;
        }

        ULCall ExportExp(LiteralExpressionSyntax e)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Const;
            currentBlock.statements.Add(node);
            node.Args.Add(e.Token.Text);
            return node;
        }
        ULCall ExportExp(ThisExpressionSyntax e)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.GetThis;
            currentBlock.statements.Add(node);
            return node;
        }
        ULCall ExportExp(AssignmentExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Assign;
            node.Args.Add(ExportExp(es.Left).GetOutputName(0));
            node.Args.Add(ExportExp(es.Right).GetOutputName(0));
            currentBlock.statements.Add(node);
            return node;
        }
        ULCall ExportExp(ObjectCreationExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Constructor;


            if (es.ArgumentList != null)
            {
                foreach (var a in es.ArgumentList.Arguments)
                {
                    node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
                }
            }

            node.Name = GetType(es.Type).FullName;

            currentBlock.statements.Add(node);
            return node;
        }

        ULCall ExportExp(InvocationExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Method;


            //if (es.Expression is MemberAccessExpressionSyntax)
            //{
            //    MemberAccessExpressionSyntax maes = es.Expression as MemberAccessExpressionSyntax;
            //    db_les.Name = (maes).Name.Identifier.Text;
            //    db_les.Caller = ExportExp(maes.Expression);
            //}
            //else if (es.Expression is IdentifierNameSyntax)
            //{
            //    IdentifierNameSyntax nameSyntax = es.Expression as IdentifierNameSyntax;
            //    db_les.Name = nameSyntax.Identifier.Text;
            //    db_les.Caller = new Metadata.Expression.ThisExp();
            //}
            //else
            //{
            //    Console.Error.WriteLine("不支持的方法调用表达式 " + es.ToString());
            //}

            node.Name = ExportExp(es.Expression).GetOutputName(0);

            foreach (var a in es.ArgumentList.Arguments)
            {
                node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
            }
            currentBlock.statements.Add(node);
            return node;
        }

        ULCall ExportExp(MemberAccessExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.GetField;

            node.Args.Add(ExportExp(es.Expression).GetOutputName(0));
            node.Name = es.Name.Identifier.Text;
            currentBlock.statements.Add(node);
            return node;
        }
        ULCall ExportExp(IdentifierNameSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Identifier;

            node.Name = es.Identifier.Text;

            currentBlock.statements.Add(node);
            return node;
        }

        string GetBinaryOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "+":
                    return "op_Add";
                case "-":
                    return "op_Sub";
                case "*":
                    return "op_Mul";
                case "/":
                    return "op_Div";
                default:
                    return "error";
            }
        }
        string GetPostfixOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "++":
                    return "op_AddAddPost";
                case "--":
                    return "op_SubSubPost";
                default:
                    return "error";
            }
        }
        string GetPrefixOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "++":
                    return "op_AddAdd";
                case "--":
                    return "op_SubSub";
                default:
                    return "error";
            }
        }

        ULCall ExportExp(BinaryExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Method;

            node.Name = GetBinaryOperatorTokenMethodName(es.OperatorToken.Text);

            var Left = ExportExp(es.Left);
            var Right = ExportExp(es.Right);

            node.Args.Add(Left.GetOutputName(0));
            node.Args.Add(Right.GetOutputName(1));

            currentBlock.statements.Add(node);


            return node;
        }

        ULCall ExportExp(PostfixUnaryExpressionSyntax es)
        {

            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Method;

            node.Name = GetPostfixOperatorTokenMethodName(es.OperatorToken.Text);

            var Left = ExportExp(es.Operand);

            node.Args.Add(Left.GetOutputName(0));


            currentBlock.statements.Add(node);


            return node;
        }

        ULCall ExportExp(PrefixUnaryExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.Method;

            node.Name = GetPrefixOperatorTokenMethodName(es.OperatorToken.Text);

            var Left = ExportExp(es.Operand);

            node.Args.Add(Left.GetOutputName(0));


            currentBlock.statements.Add(node);


            return node;
        }

        ULCall ExportExp(ArrayCreationExpressionSyntax es)
        {
            var db_les = new ULCall();
            db_les.Parent = currentBlock;
            db_les.callType = ULCall.ECallType.CreateArray;

            db_les.Args.Add( GetType(es.Type).FullName);
            foreach (var p in es.Type.RankSpecifiers)
            {
                foreach (var s in p.Sizes)
                    db_les.Args.Add(ExportExp(s).GetOutputName(0));
            }

            currentBlock.statements.Add(db_les);

            return db_les;
        }

        ULCall ExportExp(BaseExpressionSyntax es)
        {
            var db_les = new ULCall();
            db_les.Parent = currentBlock;
            db_les.callType = ULCall.ECallType.GetBase;
            currentBlock.statements.Add(db_les);
            return db_les;
        }

        ULCall ExportExp(ElementAccessExpressionSyntax es)
        {
            ULCall node = new ULCall();
            node.Parent = currentBlock;
            node.callType = ULCall.ECallType.ElementAccess;

            var exp = ExportExp(es.Expression);
            node.Args.Add(exp.GetOutputName(0));
            foreach (var a in es.ArgumentList.Arguments)
            {
                node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
            }

            return node;
        }

        ULCall ExportExp(ParenthesizedExpressionSyntax es)
        {
            return ExportExp(es.Expression);
        }
    }
}
