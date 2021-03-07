using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UL.CompileNode
{
    class CompileNode_Class : CompileNode
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
            if (identifier == type.Name)
            {
                var info = new IdentifierInfo();
                info.type = IdentifierInfo.EIdentifierType.Type;
                info.TypeID = type.ID;
                return info;
            }

            var member = type.Members.Find((v) => v.Name == identifier);
            if(member!=null)
            {
                var info = new IdentifierInfo();
                info.type = IdentifierInfo.EIdentifierType.Member;
                info.Member = member;
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

            if (memberType != null)
            {
                methodInfo.Graph.Outputs.Add(new ULArg() { Name = "ret", TypeID = memberType.ID });
            }

            foreach (var a in method.ParameterList.Parameters)
            {
                if (ContainModifier(a.Modifiers, "ref"))
                {
                    methodInfo.Graph.Outputs.Add(GetArgument(a));
                    methodInfo.Graph.Args.Add(GetArgument(a));
                }

                else if (ContainModifier(a.Modifiers, "out"))
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
}
