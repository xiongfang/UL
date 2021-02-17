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
            ExportClass(classDeclaration);
        }

        public override ULTypeInfo GetTypeInfo(TypeSyntax typeSyntax)
        {

            return base.GetTypeInfo(typeSyntax);
        }

        public override IdentifierInfo GetIdentifierInfo(string identifier)
        {
            if(identifier==type.Name)
            {
                var info = new IdentifierInfo();
                info.type = IdentifierInfo.EIdentifierType.Type;
                info.TypeFullName = type.ID;
                return info;
            }
            return base.GetIdentifierInfo(identifier);
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

        ULArg GetArgument(ParameterSyntax a)
        {
            var Arg = new ULArg();
            Arg.Name = a.Identifier.Text;
            Arg.TypeID = GetTypeInfo(a.Type).ID;
            return Arg;
        }

        void ExportMethod(BaseMethodDeclarationSyntax v)
        {

            MethodDeclarationSyntax method = v as MethodDeclarationSyntax;

            var methodInfo = new Model.ULMemberInfo();
            methodInfo.DeclareTypeID = this.type.ID;
            methodInfo.Name = method.Identifier.ValueText;
            methodInfo.IsStatic = ContainModifier(method.Modifiers, "static");
            methodInfo.Modifier = GetModifier(method.Modifiers);
            var memberType = GetTypeInfo(method.ReturnType);
            methodInfo.TypeID = memberType != null ? memberType.ID : "";
            methodInfo.MemberType = ULMemberInfo.EMemberType.Method;
            type.Members.Add(methodInfo);

            if(memberType!=null)
            {
                methodInfo.Graph.Outputs.Add(new ULArg() { Name="ret", TypeID=memberType.ID });
            }

            foreach (var a in method.ParameterList.Parameters)
            {
                if (ContainModifier(a.Modifiers, "ref"))
                {
                    methodInfo.Graph.Outputs.Add(GetArgument(a));
                    methodInfo.Graph.Args.Add(GetArgument(a));
                }

                else if (ContainModifier(a.Modifiers,"out"))
                {
                    methodInfo.Graph.Outputs.Add(GetArgument(a));
                }
                else
                {
                    methodInfo.Graph.Args.Add(GetArgument(a));
                }
                
            }

            CompileNode_ExportBody exportBody = new CompileNode_ExportBody();
            exportBody.Parent = this;
            Children.Add(exportBody);
            exportBody.ExportBody(v.Body, methodInfo.Graph);
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
        
    }

    public class CompileNode_ExportBody:CompileNode
    {
        public ULGraph graph;
        ULNode lastNode;

        public void ExportBody(BlockSyntax bs, ULGraph graph)
        {
            this.graph = graph;
            var entry = new ULNode();
            entry.NodeID = Guid.NewGuid().ToString();
            entry.Name = ULNode.name_entry;
            lastNode = entry;
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
            //else if (node is WhileStatementSyntax)
            //{
            //    ExportStatement(node as WhileStatementSyntax);
            //}
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

        void ExportStatement(BlockSyntax node)
        {
            foreach (var s in node.Statements)
            {
                ExportStatement(s);
            }
        }

        void ExportStatement(IfStatementSyntax node)
        {
            ULNode nodeIf = new ULNode();
            nodeIf.NodeID = Guid.NewGuid().ToString();
            nodeIf.Name = ULNode.name_if;
            lastNode.LinkControlTo(nodeIf, 0, 0);

            var cond = ExportExp(node.Condition);
            nodeIf.Inputs.Add(cond.NodeID + "." + 0);
            lastNode = nodeIf;
            if (node.Statement !=null)
            {
                ExportStatement(node.Statement);
                nodeIf.ControlOutputs[0] = lastNode.NodeID + "." + "0";
            }
            if(node.Else.Statement!=null)
            {
                ExportStatement(node.Else.Statement as BlockSyntax);
                nodeIf.ControlOutputs[1] = lastNode.NodeID + "." + "0";
            }
        }
        void ExportStatement(WhileStatementSyntax node)
        {
            //var cond = ExportExp(node.Condition);

            //var ifStatement = new ULStatementWhile();
            //ifStatement.Parent = currentBlock;
            //ifStatement.arg = cond.GetOutputName(0);
            //if (node.Statement is BlockSyntax)
            //    ifStatement.block = ExportStatement(node.Statement as BlockSyntax);
            //currentBlock.statements.Add(ifStatement);
        }

        void ExportStatement(ExpressionStatementSyntax node)
        {
            var call = ExportExp(node.Expression);

        }

        void ExportStatement(LocalDeclarationStatementSyntax ss)
        {
            //var Type = GetType(ss.Declaration.Type);
            //foreach (var v in ss.Declaration.Variables)
            //{
            //    var vName = v.Identifier.Text;
            //    frames.Peek().variables[vName] = Type.FullName;
            //    if (v.Initializer != null)
            //    {
            //        ULCall node = new ULCall();
            //        node.Parent = currentBlock;
            //        node.callType = ULCall.ECallType.Assign;
            //        node.Args.Add("local." + vName);
            //        node.Args.Add(ExportExp(v.Initializer.Value).GetOutputName(0));
            //        currentBlock.statements.Add(node);
            //    }
            //}
        }

        void ExportStatement(ForStatementSyntax ss)
        {
            //var db_ss = new ULStatementFor();
            //db_ss.Condition = ExportExp(ss.Condition).GetOutputName(0);
            //db_ss.Declaration = ExportExp(ss.Declaration).GetOutputName(0);
            //foreach (var inc in ss.Incrementors)
            //{
            //    db_ss.Incrementors.Add(ExportExp(inc).GetOutputName(0));
            //}
            //db_ss.block = ExportStatement(ss.Statement as BlockSyntax);
        }

        void ExportStatement(DoStatementSyntax ss)
        {
            //var cond = ExportExp(ss.Condition);

            //var ifStatement = new ULStatementWhile();
            //ifStatement.Parent = currentBlock;
            //ifStatement.arg = cond.GetOutputName(0);
            //if (ss.Statement is BlockSyntax)
            //    ifStatement.block = ExportStatement(ss.Statement as BlockSyntax);
            //currentBlock.statements.Add(ifStatement);
        }

        void ExportStatement(BreakStatementSyntax ss)
        {
            //var node = new ULStatementBreak();
            //node.Parent = currentBlock;
            //currentBlock.statements.Add(node);
        }

        void ExportStatement(ReturnStatementSyntax ss)
        {
            //var node = new ULStatementReturn();
            //node.Parent = currentBlock;
            //node.Arg = ExportExp(ss.Expression).GetOutputName(0);
            //currentBlock.statements.Add(node);
        }

        void ExportStatement(SwitchStatementSyntax ss)
        {
            //var node = new ULStatementSwitch();
            //node.Parent = currentBlock;
            //node.Condition = ExportExp(ss.Expression).GetOutputName(0);
            //node.Sections = new List<ULStatementSwitch.Section>();
            //foreach (var s in ss.Sections)
            //{
            //    var section = new ULStatementSwitch.Section();
            //    section.Labels = new List<string>();
            //    section.Statements = new List<ULNodeBlock>();
            //    node.Sections.Add(section);

            //    foreach (var l in s.Labels)
            //    {
            //        if (l is CaseSwitchLabelSyntax)
            //        {
            //            CaseSwitchLabelSyntax csls = l as CaseSwitchLabelSyntax;
            //            section.Labels.Add(ExportExp(csls.Value).GetOutputName(0));
            //        }
            //    }

            //    foreach (var statement in s.Statements)
            //    {
            //        section.Statements.Add(ExportStatement(statement as BlockSyntax));
            //    }
            //}



            //currentBlock.statements.Add(node);
        }

        //ULNode ExportExp(VariableDeclarationSyntax es)
        //{
        //    var typeName = GetType(es.Type).FullName;
        //    ULCall node = null;
        //    foreach (var v in es.Variables)
        //    {
        //        node = new ULCall();
        //        node.Parent = currentBlock;
        //        node.callType = ULCall.ECallType.DeclarationLocal;
        //        node.Args.Add(typeName);
        //        node.Args.Add(v.Identifier.Text);
        //        if (v.Initializer != null)
        //            node.Args.Add(ExportExp(v.Initializer.Value).GetOutputName(0));
        //    }

        //    return node;
        //}

        ULNode ExportExp(ExpressionSyntax es)
        {
            if (es is LiteralExpressionSyntax)
            {
                return ExportExp(es as LiteralExpressionSyntax);
            }
            else if (es is ThisExpressionSyntax)
            {
                return ExportExp(es as ThisExpressionSyntax);
            }
            //else if (es is ObjectCreationExpressionSyntax)
            //{
            //    return ExportExp(es as ObjectCreationExpressionSyntax);
            //}
            //else if (es is InvocationExpressionSyntax)
            //{
            //    return ExportExp(es as InvocationExpressionSyntax);
            //}
            //else if (es is MemberAccessExpressionSyntax)
            //{
            //    return ExportExp(es as MemberAccessExpressionSyntax);
            //}
            //else if (es is IdentifierNameSyntax)
            //{
            //    return ExportExp(es as IdentifierNameSyntax);
            //}
            //else if (es is AssignmentExpressionSyntax)
            //{
            //    return ExportExp(es as AssignmentExpressionSyntax);
            //}
            //else if (es is BinaryExpressionSyntax)
            //{
            //    return ExportExp(es as BinaryExpressionSyntax);
            //}
            //else if (es is PostfixUnaryExpressionSyntax)
            //{
            //    return ExportExp(es as PostfixUnaryExpressionSyntax);
            //}
            //else if (es is ArrayCreationExpressionSyntax)
            //{
            //    return ExportExp(es as ArrayCreationExpressionSyntax);
            //}
            //else if (es is PrefixUnaryExpressionSyntax)
            //{
            //    return ExportExp(es as PrefixUnaryExpressionSyntax);
            //}
            //else if (es is BaseExpressionSyntax)
            //{
            //    return ExportExp(es as BaseExpressionSyntax);
            //}
            //else if (es is ThrowExpressionSyntax)
            //{
            //    return ExportExp(es as ThrowExpressionSyntax);
            //}
            //else if (es is ParenthesizedExpressionSyntax)
            //{
            //    return ExportExp(es as ParenthesizedExpressionSyntax);
            //}
            //else if (es is ElementAccessExpressionSyntax)
            //{
            //    return ExportExp(es as ElementAccessExpressionSyntax);
            //}
            else
            {
                Console.Error.WriteLine(string.Format("error:不支持的表达式 {0} {1}", es.GetType().Name, es.ToString()));
            }
            return null;
        }

        ULNode ExportExp(LiteralExpressionSyntax e)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            node.Name = ULNode.name_const;
            node.Type = ULNode.ENodeType.Control;
            node.Inputs.Add(e.Token.Text);
            //currentBlock.statements.Add(node);
            //node.Inputs = new string[1];

            return node;
        }
        ULNode ExportExp(ThisExpressionSyntax e)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            node.Name = ULNode.name_getthis;
            node.Type = ULNode.ENodeType.Control;
            return node;
        }
        //ULCall ExportExp(AssignmentExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Assign;
        //    node.Args.Add(ExportExp(es.Left).GetOutputName(0));
        //    node.Args.Add(ExportExp(es.Right).GetOutputName(0));
        //    //currentBlock.statements.Add(node);
        //    return node;
        //}
        //ULCall ExportExp(ObjectCreationExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Constructor;


        //    if (es.ArgumentList != null)
        //    {
        //        foreach (var a in es.ArgumentList.Arguments)
        //        {
        //            node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
        //        }
        //    }

        //    node.Name = GetType(es.Type).FullName;

        //    //currentBlock.statements.Add(node);
        //    return node;
        //}

        //ULCall ExportExp(InvocationExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;


        //    //if (es.Expression is MemberAccessExpressionSyntax)
        //    //{
        //    //    MemberAccessExpressionSyntax maes = es.Expression as MemberAccessExpressionSyntax;
        //    //    db_les.Name = (maes).Name.Identifier.Text;
        //    //    db_les.Caller = ExportExp(maes.Expression);
        //    //}
        //    //else if (es.Expression is IdentifierNameSyntax)
        //    //{
        //    //    IdentifierNameSyntax nameSyntax = es.Expression as IdentifierNameSyntax;
        //    //    db_les.Name = nameSyntax.Identifier.Text;
        //    //    db_les.Caller = new Metadata.Expression.ThisExp();
        //    //}
        //    //else
        //    //{
        //    //    Console.Error.WriteLine("不支持的方法调用表达式 " + es.ToString());
        //    //}

        //    node.Name = ExportExp(es.Expression).GetOutputName(0);

        //    foreach (var a in es.ArgumentList.Arguments)
        //    {
        //        node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
        //    }
        //    //currentBlock.statements.Add(node);
        //    return node;
        //}

        //ULCall ExportExp(MemberAccessExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.GetField;

        //    node.Args.Add(ExportExp(es.Expression).GetOutputName(0));
        //    node.Name = es.Name.Identifier.Text;
        //    //currentBlock.statements.Add(node);
        //    return node;
        //}
        //ULCall ExportExp(IdentifierNameSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Identifier;

        //    node.Name = es.Identifier.Text;

        //    //currentBlock.statements.Add(node);
        //    return node;
        //}

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

        //ULCall ExportExp(BinaryExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetBinaryOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Left);
        //    var Right = ExportExp(es.Right);

        //    node.Args.Add(Left.GetOutputName(0));
        //    node.Args.Add(Right.GetOutputName(1));

        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

        //ULCall ExportExp(PostfixUnaryExpressionSyntax es)
        //{

        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetPostfixOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Operand);

        //    node.Args.Add(Left.GetOutputName(0));


        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

        //ULCall ExportExp(PrefixUnaryExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetPrefixOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Operand);

        //    node.Args.Add(Left.GetOutputName(0));


        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

        //ULCall ExportExp(ArrayCreationExpressionSyntax es)
        //{
        //    var db_les = new ULCall();
        //    db_les.Parent = currentBlock;
        //    db_les.callType = ULCall.ECallType.CreateArray;

        //    db_les.Args.Add(GetType(es.Type).FullName);
        //    foreach (var p in es.Type.RankSpecifiers)
        //    {
        //        foreach (var s in p.Sizes)
        //            db_les.Args.Add(ExportExp(s).GetOutputName(0));
        //    }

        //    //currentBlock.statements.Add(db_les);

        //    return db_les;
        //}

        //ULCall ExportExp(BaseExpressionSyntax es)
        //{
        //    var db_les = new ULCall();
        //    db_les.Parent = currentBlock;
        //    db_les.callType = ULCall.ECallType.GetBase;
        //    //currentBlock.statements.Add(db_les);
        //    return db_les;
        //}

        //ULCall ExportExp(ElementAccessExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.ElementAccess;

        //    var exp = ExportExp(es.Expression);
        //    node.Args.Add(exp.GetOutputName(0));
        //    foreach (var a in es.ArgumentList.Arguments)
        //    {
        //        node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
        //    }

        //    return node;
        //}

        //ULCall ExportExp(ParenthesizedExpressionSyntax es)
        //{
        //    return ExportExp(es.Expression);
        //}
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
