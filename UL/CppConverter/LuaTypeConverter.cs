using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    class LuaTypeConverter : IDefaultTypeConverter
    {
        int depth;
        StringBuilder sb = new StringBuilder();
        IConverter Converter;

        HashSet<string> exportedNamespace = new HashSet<string>();

        public LuaTypeConverter(IConverter cv) { this.Converter = cv; }

        Metadata.Model Model
        {
            get { return Converter.GetModel(); }
        }

        public int priority
        {
            get { return 0; }
        }

        void AppendDepth()
        {
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
        }

        void AppendLine(string msg)
        {
            AppendDepth();
            sb.AppendLine(msg);
        }

        void AppendLine(string fmt,params object[] args)
        {
            AppendDepth();
            sb.AppendLine(string.Format(fmt, args));
        }

        void Append(string msg)
        {
            AppendDepth();
            sb.Append(msg);
        }

        string GetLuaImportHeader(Metadata.DB_Type type)
        {
            return type._namespace +"."+type.name;
        }

        public void ConvertTypeHeader(Metadata.DB_Type type)
        {
            Model.EnterNamespace(type._namespace);
            Model.EnterType(type);
            //头文件
            {
                sb.Clear();
                
                {
                    //HashSet<string> depTypes = Converter.GetTypeDependences(type);

                    AppendLine("require \"" + GetNSTableName(type._namespace) + "\"");
                    //foreach (var t in depTypes)
                    //{
                    //    Metadata.DB_Type depType = Model.GetType(t);
                    //    if(depType!=type)
                    //        AppendLine("require \"" + GetLuaImportHeader(depType) + "\"");
                    //}

                    if (!type.base_type.IsVoid /*&& !type.is_value_type*/ || type.interfaces.Count > 0)
                    {
                        Append(string.Format("{0} = class('{1}',{2})", GetTypeTableName(type), GetTypeTableName(type), GetTypeTableName(Model.GetType(type.base_type))));
                        //for(int i=0;i<type.interfaces.Count;i++)
                        //{
                        //    sb.Append(",");
                        //    sb.Append(GetTypeTableName(Model.GetType(type.interfaces[i])));
                        //}
                        //sb.Append(")");
                        sb.AppendLine();
                    }
                    else
                    {
                        AppendLine(string.Format("{0} = class('{1}')", GetTypeTableName(type), GetTypeTableName(type)));
                    }
                    //}

                    //AppendLine("{");
                    {
                        //depth++;

                        if (type.is_enum)
                        {
                            //List<Metadata.DB_Member> members = type.members.Values.ToList();
                            //members.Sort((a, b) => { return a.order <= b.order ? -1 : 1; });
                            //for (int i = 0; i < members.Count; i++)
                            //{
                            //    Append(members[i].name);
                            //    if (i < members.Count - 1)
                            //        sb.Append(",");
                            //    sb.AppendLine();
                            //}
                        }
                        else
                        {
                            //if (HasCppAttribute(type.attributes))
                            //{
                            //    AppendLine("GENERATED_BODY()");
                            //}

                            foreach (var m in type.members.Values)
                            {
                                if (type.is_generic_type_definition && m.member_type == (int)Metadata.MemberTypes.Method && m.method_body == null)
                                    continue;
                                ConvertMemberHeader(m);
                            }
                        }

                        
                        

                    }

                    TypeConfig tc = Converter.GetTypeConfig(type);

                    if (tc != null)
                    {
                        if (!string.IsNullOrEmpty(tc.ext_header))
                        {
                            AppendLine("require \"" + System.IO.Path.GetFileNameWithoutExtension( tc.ext_header) + "\"");
                        }
                    }
                    
                }

                Model.LeaveType();
                Model.LeaveNamespace();
            }

        }

        public void ConvertTypeMetadata(Metadata.DB_Type type)
        {
            AppendLine("{0}_Metadata={{",GetTypeTableName(type));
            depth++;
            AppendLine("Name=\"{0}\",", type.name);
            AppendLine("Namespace=\"{0}\",", type._namespace);
            AppendLine("Comments=\"{0}\",", type.comments);
            AppendLine("Modifier={0},", type.modifier);
            AppendLine("TypeID={0},", type.type);
            AppendLine("IsAbstract={0},", type.is_abstract?"true":"false");
            AppendLine("IsGenericTypeDefinition={0},", type.is_generic_type_definition ? "true" : "false");
            AppendLine("Parent=\"{0}\"", type.base_type.GetTypeDefinitionFullName());
            depth--;
            AppendLine("}");

        }

        //string GetTypeSyntaxObject(Metadata.Expression.TypeSyntax typeSyntax)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if(typeSyntax.args.Length>0)
        //    {
        //        sb.Append("{");
        //        for(int i=0;i<typeSyntax.args.Length;i++)
        //        {
        //            sb.Append(GetTypeSyntaxObject(typeSyntax.args[i]));
        //            if(i<typeSyntax.args.Length-1)
        //            {
        //                sb.Append(",");
        //            }
        //        }

        //        sb.Append("}");
        //    }
        //    return string.Format("ui.System.Metadata.TypeSyntax:new(\"{0}\",\"{1}\",{2},{3},{4},{5})",
        //        typeSyntax.Name, typeSyntax.name_space, typeSyntax.isGenericParameter?"true":"false", typeSyntax.isGenericType?"true":"false", typeSyntax.isGenericTypeDefinition ? "true" : "false", sb.Length==0?"nil":sb.ToString());

        //}


        string GetTypeTableName(Metadata.DB_Type type)
        {
            return type._namespace + "." + type.name;
        }

        string GetNSTableName(string ns)
        {
            return ns;
        }

        string GetNSTablePath(string ns)
        {
            string[] pathname = ns.Split('.');
            List<string> path = new List<string>();
            path.AddRange(pathname);
            path.RemoveAt(path.Count - 1);
            return System.IO.Path.Combine(path.ToArray());
        }

        string GetNSTablePathName(string ns)
        {
            string[] pathname = ns.Split('.');
            return System.IO.Path.Combine(pathname);
        }

        string GetCppTypeWrapName(Metadata.DB_Type type)
        {
            return GetTypeTableName(type);
        }
        string GetCppTypeName(Metadata.DB_Type type)
        {
            return GetTypeTableName(type);
        }

        void WriteFile(string path, string content)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            if (System.IO.File.Exists(path))
            {
                if (System.IO.File.ReadAllText(path, Encoding.Default) == content)
                    return;
            }

            System.IO.File.WriteAllText(path, content, Encoding.Default);

        }

        public void ConvertType(Metadata.DB_Type type)
        {
            string outputDir = Converter.GetProject().output_dir;

            if(!exportedNamespace.Contains(type._namespace))
            {
                

                string[] nsList = type._namespace.Split('.');

                string ns = "";
                for (int i=0;i<nsList.Length;i++)
                {
                    ns = i > 0 ? (ns + "." + nsList[i]) : nsList[i];

                    if(!exportedNamespace.Contains(ns))
                    {
                        exportedNamespace.Add(ns);

                        string ns_dep_ns = null;
                        if (ns.Contains("."))
                        {
                            ns_dep_ns = ns.Substring(0, ns.LastIndexOf("."));
                        }

                        StringBuilder stringBuild = new StringBuilder();
                        if (ns_dep_ns != null)
                            stringBuild.AppendLine("require \"" + GetNSTableName(ns_dep_ns) + "\"");
                        stringBuild.AppendLine(ns + " = {}");

                        WriteFile(System.IO.Path.Combine(outputDir, GetNSTablePathName(ns) + ".lua"), stringBuild.ToString());
                    }
                    

                }

                exportedNamespace.Add(type._namespace);

            }


            ITypeConverter tc = Converter.GetTypeConverter(type);
            if (tc != null)
            {
                sb.Clear();
                string content;
                if (tc.ConvertTypeHeader(Converter, type, out content))
                {
                    sb.Append(content);

                    WriteFile(System.IO.Path.Combine(outputDir, GetTypeHeaderPathName(type) + ".lua"), sb.ToString());
                }
            }
            else
            {
                sb.Clear();

                if(type.is_generic_type_definition)
                {
                    Metadata.Expression.TypeSyntax ts = type.GetRefType();
                    for(int i=0;i<ts.args.Length;i++)
                    {
                        ts.args[i] = new Metadata.Expression.TypeSyntax() { name_space = "ul.System", Name = "Object" };
                    }
                    type = Model.GetType(ts);
                }

                ConvertTypeHeader(type);
                WriteFile(System.IO.Path.Combine(outputDir, GetTypeHeaderPathName(type) + ".lua"), sb.ToString());

            }

            sb.Clear();
            ConvertTypeMetadata(type);
            WriteFile(System.IO.Path.Combine(outputDir, GetTypeHeaderPathName(type) + "_Metadata.lua"), sb.ToString());
        }

        string GetTypeHeaderPathName(Metadata.DB_Type type)
        {
            return System.IO.Path.Combine(type._namespace.Replace(".", "/"), type.name);
        }

        //string GetCppTypeName(Metadata.DB_Type type)
        //{
        //    ITypeConverter tc = Converter.GetTypeConverter(type);
        //    if (tc != null)
        //    {
        //        string name;
        //        if (tc.GetCppTypeName(out name))
        //        {
        //            return name;
        //        }
        //    }
        //    if (type.is_generic_paramter)
        //        return type.name;
        //    if (type.is_generic_type)
        //    {

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(type._namespace.Replace(".", "::"));
        //        sb.Append("::");
        //        sb.Append(type.name);
        //        sb.Append("<");
        //        for (int i = 0; i < type.generic_parameters.Count; i++)
        //        {
        //            sb.Append(GetCppTypeName(Model.GetType(type.generic_parameters[i])));
        //            if (i < type.generic_parameters.Count - 1)
        //                sb.Append(",");
        //        }
        //        sb.Append(">");
        //        return sb.ToString();
        //    }
        //    if (type.is_interface)
        //        return type._namespace.Replace(".", "::") + "::" + type.name;
        //    if (type.is_class)
        //        return type._namespace.Replace(".", "::") + "::" + type.name;
        //    if (type.is_value_type)
        //        return type._namespace.Replace(".", "::") + "::" + type.name;
        //    if (type.is_enum)
        //        return type._namespace.Replace(".", "::") + "::" + type.name;

        //    return type.static_full_name;
        //}

        //string GetCppTypeWrapName(Metadata.DB_Type type)
        //{
        //    if (type.GetRefType().IsVoid)
        //        return "void";
        //    if (type.is_value_type)
        //    {
        //        return GetCppTypeName(type);
        //    }
        //    else
        //    {
        //        return string.Format("Ref<{0}>", GetCppTypeName(type));
        //    }

        //}

        string GetModifierString(int modifier)
        {
            switch ((Metadata.Modifier)modifier)
            {
                case Metadata.Modifier.Private:
                    return "private";
                case Metadata.Modifier.Protected:
                    return "protected";
                case Metadata.Modifier.Public:
                    return "public";
            }

            return "";
        }

        string GetOperatorFuncName(string token, int arg_count = 1)
        {
            switch (token)
            {
                case "+":
                    return arg_count == 2 ? "op_Addition" : "op_UnaryPlus";
                case "-":
                    return arg_count == 2 ? "op_Substraction" : "op_UnaryNegation";
                case "*":
                    return "op_Multiply";
                case "/":
                    return "op_Division";
                case "%":
                    return "op_Modulus";
                case "&":
                    return "op_BitwiseAnd";
                case "|":
                    return "op_BitwiseOr";
                case "~":
                    return "op_OnesComplement";
                case "<<":
                    return "op_LeftShift";
                case ">>":
                    return "op_RightShift";
                case "==":
                    return "op_Equality";
                case "!=":
                    return "op_Inequality";
                case ">":
                    return "op_GreaterThen";
                case "<":
                    return "op_LessThen";
                case "++":
                    return "op_Increment";
                case "--":
                    return "op_Decrement";
                case "!":
                    return "op_LogicNot";
                default:
                    Console.Error.WriteLine("未知的操作符 " + token);
                    return token;
            }
        }

        string MakeMethodDeclareArgs(Metadata.DB_Member member)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool outP = false;
            if (member.method_args != null)
                for (int i = 0; i < member.method_args.Length; i++)
                {
                    if (member.method_args[i].is_out || member.method_args[i].is_ref)
                        outP = true;
                    Metadata.DB_Type arg_Type = Model.GetType(member.method_args[i].type);
                    //string typeName = GetCppTypeName(arg_Type);
                    stringBuilder.Append(string.Format("{0}",member.method_args[i].name));
                    if (i < member.method_args.Length - 1)
                        stringBuilder.Append(",");
                }

            if(outP)
            {
                stringBuilder.Append(",ref_func");
            }
            return stringBuilder.ToString();
        }

        string MakeMethodCallArgs(Metadata.DB_Member member)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (member.method_args != null)
                for (int i = 0; i < member.method_args.Length; i++)
                {
                    Metadata.DB_Type arg_Type = Model.GetType(member.method_args[i].type);
                    //string typeName = GetCppTypeName(arg_Type);
                    stringBuilder.Append(string.Format("{0}", member.method_args[i].name));
                    if (i < member.method_args.Length - 1)
                        stringBuilder.Append(",");
                }

            return stringBuilder.ToString();
        }

        Metadata.DB_Member current_member;
        void ConvertMemberHeader(Metadata.DB_Member member)
        {
            Metadata.DB_Type member_type = Model.GetType(member.typeName);
            if (member.member_type == (int)Metadata.MemberTypes.Field)
            {
                //AppendLine(GetModifierString(member.modifier) + ":");
                //属性
                //AppendLine(ConvertCppAttribute(member.attributes));
                //if (member.is_static)
                //    sb.Append("static ");
                //else
                //    sb.Append("");

                //sb.AppendLine(string.Format("{0} {1};", GetCppTypeWrapName(Model.GetType(member.type)), member.name));
            }
            else if (member.member_type == (int)Metadata.MemberTypes.Event)
            {
                //AppendLine(GetModifierString(member.modifier) + ":");
                ////属性
                ////AppendLine(ConvertCppAttribute(member.attributes));
                //if (member.is_static)
                //    Append("static ");
                //else
                //    Append("");

                //sb.AppendLine(string.Format("{0} {1};", GetCppTypeWrapName(Model.GetType(member.type)), member.name));
            }
            else if (member.member_type == (int)Metadata.MemberTypes.Method)
            {
                //if (member.method_body == null)
                //    return;
                Model.EnterMethod(member);

                Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);

                if (!Model.currentType.is_delegate)
                {
                    string method_name = GetMethodUniqueName(member);
                    sb.Append(string.Format("function {0}{1}{2}",GetCppTypeWrapName(declare_type), member.is_static?".":":", method_name));
                }
                else
                {
                    sb.Append(string.Format("function {0}{1}{2}", GetCppTypeWrapName(declare_type), member.is_static ? "." : ":", member.name));
                }

                sb.AppendFormat("({0})", MakeMethodDeclareArgs(member));
                sb.AppendLine();

                depth++;
                current_member = member;
                if (member.method_body != null)
                    ConvertStatement(member.method_body);

                current_member = null;
                depth--;

                AppendLine("end");
                Model.LeaveMethod();
            }
        }

        //void ConvertMemberCpp(Metadata.DB_Member member)
        //{
        //    Metadata.DB_Type member_type = Model.GetType(member.typeName);
        //    if (member.member_type == (int)Metadata.MemberTypes.Field || member.member_type == (int)Metadata.MemberTypes.Event)
        //    {
        //        if (member.is_static)
        //        {
        //            if (member_type.is_class)
        //                AppendLine("Ref<" + GetCppTypeName(Model.GetType(member.type)) + "> " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name + ";");
        //            else if (member_type.is_value_type)
        //            {
        //                Append(GetCppTypeName(Model.GetType(member.type)) + " " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name);
        //                if (member.field_initializer != null)
        //                {
        //                    sb.Append("=");
        //                    sb.Append(ExpressionToString(member.field_initializer));
        //                }
        //                sb.AppendLine(";");
        //            }
        //        }
        //    }
        //    else if (member.member_type == (int)Metadata.MemberTypes.Method)
        //    {
        //        Model.EnterMethod(member);
        //        Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);
        //        if (!declare_type.is_generic_type_definition && member.method_body != null)
        //        {
        //            if (!member.method_is_constructor)
        //            {
        //                string method_name = member.name;
        //                if (member.method_is_operator)
        //                {
        //                    method_name = GetOperatorFuncName(member.name, member.method_args.Length);
        //                }
        //                sb.Append(string.Format("{0} {1}::{2}", member.type.IsVoid ? "void" : GetCppTypeWrapName(Model.GetType(member.type)), GetCppTypeName(Model.GetType(member.declaring_type)), method_name));
        //            }
        //            else
        //                sb.Append(string.Format("{1}::{2}", "", GetCppTypeName(Model.GetType(member.declaring_type)), member.name));
        //            sb.Append("(");
        //            if (member.method_args != null)
        //            {
        //                for (int i = 0; i < member.method_args.Length; i++)
        //                {
        //                    sb.Append(string.Format("{0} {1} {2}", GetCppTypeWrapName(Model.GetType(member.method_args[i].type)), (member.method_args[i].is_ref || member.method_args[i].is_out) ? "&" : "", member.method_args[i].name));
        //                    if (i < member.method_args.Length - 1)
        //                        sb.Append(",");
        //                }
        //            }
        //            sb.AppendLine(")");

        //            ConvertStatement(member.method_body);
        //        }
        //        Model.LeaveMethod();
        //    }
        //}

        void ConvertStatement(Metadata.DB_StatementSyntax ss)
        {
            if (ss is Metadata.DB_BlockSyntax)
            {
                ConvertStatement((Metadata.DB_BlockSyntax)ss);
            }
            else if (ss is Metadata.DB_IfStatementSyntax)
            {
                ConvertStatement((Metadata.DB_IfStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_ExpressionStatementSyntax)
            {
                ConvertStatement((Metadata.DB_ExpressionStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_LocalDeclarationStatementSyntax)
            {
                ConvertStatement((Metadata.DB_LocalDeclarationStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_ForStatementSyntax)
            {
                ConvertStatement((Metadata.DB_ForStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_DoStatementSyntax)
            {
                ConvertStatement((Metadata.DB_DoStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_WhileStatementSyntax)
            {
                ConvertStatement((Metadata.DB_WhileStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_SwitchStatementSyntax)
            {
                ConvertStatement((Metadata.DB_SwitchStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_BreakStatementSyntax)
            {
                AppendLine("break;");
            }
            else if (ss is Metadata.DB_ReturnStatementSyntax)
            {
                ConvertStatement((Metadata.DB_ReturnStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_TryStatementSyntax)
            {
                ConvertStatement((Metadata.DB_TryStatementSyntax)ss);
            }
            else if (ss is Metadata.DB_ThrowStatementSyntax)
            {
                ConvertStatement((Metadata.DB_ThrowStatementSyntax)ss);
            }
            else
            {
                Console.Error.WriteLine("不支持的语句 " + ss.GetType().ToString());
            }
        }

        void ConvertStatement(Metadata.DB_BlockSyntax bs)
        {
            AppendLine("do");
            depth++;
            Model.EnterBlock();
            foreach (var s in bs.List)
            {
                ConvertStatement(s);
            }
            depth--;
            Model.LeaveBlock();
            AppendLine("end");
        }

        void CheckEnter(Metadata.DB_StatementSyntax ss)
        {
            if (!(ss is Metadata.DB_BlockSyntax))
                depth++;
        }
        void CheckOut(Metadata.DB_StatementSyntax ss)
        {
            if (!(ss is Metadata.DB_BlockSyntax))
                depth--;
        }


        void ConvertStatement(Metadata.DB_ReturnStatementSyntax bs)
        {
            
            if(this.current_member !=null)
            {
                List<int> refArgs = new List<int>();
                for (int i=0;i<current_member.method_args.Length;i++)
                {
                    if(current_member.method_args[i].is_out || current_member.method_args[i].is_ref)
                    {
                        refArgs.Add(i);
                    }
                }

                if(refArgs.Count>0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("ref_func(");
                    for(int i=0;i< refArgs.Count;i++)
                    {
                        sb.Append(current_member.method_args[refArgs[i]].name);
                        if (i < refArgs.Count - 1)
                            sb.Append(",");
                    }
                    sb.Append(");");
                    AppendLine(sb.ToString());
                }
            }

            AppendLine("return " + ExpressionToString(bs.Expression) + ";");
        }

        void ConvertStatement(Metadata.DB_IfStatementSyntax bs)
        {
            AppendLine("if (" + ExpressionToString(bs.Condition) + ")._v then");
            CheckEnter(bs.Statement);
            ConvertStatement(bs.Statement);
            CheckOut(bs.Statement);

            if (bs.Else != null)
            {
                AppendLine("else");
                CheckEnter(bs.Else);
                ConvertStatement(bs.Else);
                CheckOut(bs.Else);
            }

            AppendLine("end");
        }

        void ConvertStatement(Metadata.DB_ExpressionStatementSyntax bs)
        {
            AppendLine(ExpressionToString(bs.Exp) + ";");
        }

        void ConvertStatement(Metadata.DB_LocalDeclarationStatementSyntax bs)
        {
            foreach (var d in bs.Declaration.Variables)
                Model.AddLocal(d.Identifier, Model.GetType(bs.Declaration.Type));

            sb.Append(ExpressionToString(bs.Declaration));

            sb.AppendLine(";");
        }
        void ConvertStatement(Metadata.DB_ForStatementSyntax bs)
        {
            Model.EnterBlock();
            foreach(var d in bs.Declaration.Variables)
                Model.AddLocal(d.Identifier, Model.GetType(bs.Declaration.Type));

            AppendLine("do");
            depth++;
            AppendLine(ExpressionToString(bs.Declaration));
            AppendLine("while ("+ ExpressionToString(bs.Condition) +")._v");
            AppendLine("do");
            depth++;

            AppendLine("do");
            depth++;
            ConvertStatement(bs.Statement);
            depth--;
            AppendLine("end");

            AppendLine("do");
            depth++;
            AppendLine("::for_end::");
            for (int i = 0; i < bs.Incrementors.Count; i++)
            {
                Append(ExpressionToString(bs.Incrementors[i]));
                if (i < bs.Incrementors.Count - 2)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }
            depth--;
            AppendLine("end");

            depth--;
            AppendLine("end");
            depth--;
            AppendLine("end");

            Model.LeaveBlock();
        }

        void ConvertStatement(Metadata.DB_DoStatementSyntax bs)
        {
            AppendLine("repeat");
            ConvertStatement(bs.Statement);
            Append("until ");
            sb.Append("(");
            sb.Append(ExpressionToString(bs.Condition));
            sb.AppendLine(")._v;");
        }
        void ConvertStatement(Metadata.DB_WhileStatementSyntax bs)
        {
            Append("while");
            sb.Append(" (");
            sb.Append(ExpressionToString(bs.Condition));
            sb.AppendLine(")._v");
            AppendLine("do");
            ConvertStatement(bs.Statement);
            AppendLine("end");
        }

        void ConvertStatement(Metadata.DB_SwitchStatementSyntax bs)
        {
            Append("switch");
            sb.Append("(");
            sb.Append(ExpressionToString(bs.Expression));
            sb.AppendLine(")");
            AppendLine("{");
            depth++;
            for (int i = 0; i < bs.Sections.Count; i++)
            {
                ConvertSwitchSection(bs.Sections[i]);
            }
            depth--;
            AppendLine("}");
        }
        void ConvertSwitchSection(Metadata.DB_SwitchStatementSyntax.SwitchSectionSyntax bs)
        {
            for (int i = 0; i < bs.Labels.Count; i++)
            {
                AppendLine("case " + ExpressionToString(bs.Labels[i]) + ":");
            }

            for (int i = 0; i < bs.Statements.Count; i++)
            {
                ConvertStatement(bs.Statements[i]);
            }
        }


        void ConvertStatement(Metadata.DB_TryStatementSyntax ss)
        {
            AppendLine(@"local __ret_v = try(
        function()");
            depth++;
            ConvertStatement(ss.Block);
            depth--;
            AppendLine("end,");

            for (int i = 0; i < ss.Catches.Count; i++)
            {
                Model.AddLocal(ss.Catches[i].Identifier, Model.GetType(ss.Catches[i].Type));

                AppendLine("{");
                depth++;
                AppendLine(string.Format("type=\"{0}\",", GetCppTypeName(Model.GetType(ss.Catches[i].Type)), ss.Catches[i].Identifier));
                AppendLine(string.Format("func= function({0})",ss.Catches[i].Identifier));
                depth++;
                ConvertStatement(ss.Catches[i].Block);
                depth--;
                AppendLine("end");
                depth--;
                //AppendLine(@"end");
                AppendLine("}");
                if (i < ss.Catches.Count-1)
                    Append(",");
            }
            AppendLine(");");

            AppendLine("if __ret_v~= nil then return __ret_v end ");
        }

        void ConvertStatement(Metadata.DB_ThrowStatementSyntax ss)
        {
            Append("return ");
            sb.Append(ExpressionToString(ss.Expression));
            sb.AppendLine(";");
        }

        public string ExpressionToString(Metadata.Expression.Exp es, Metadata.Expression.Exp outer = null)
        {
            if (es is Metadata.Expression.ConstExp)
            {
                return ExpressionToString((Metadata.Expression.ConstExp)es, outer);
            }
            else if (es is Metadata.Expression.FieldExp)
            {
                return ExpressionToString((Metadata.Expression.FieldExp)es, outer);
            }
            else if (es is Metadata.Expression.MethodExp)
            {
                return ExpressionToString((Metadata.Expression.MethodExp)es, outer);
            }
            else if (es is Metadata.Expression.ThisExp)
            {
                return ExpressionToString((Metadata.Expression.ThisExp)es, outer);
            }
            else if (es is Metadata.Expression.ObjectCreateExp)
            {
                return ExpressionToString((Metadata.Expression.ObjectCreateExp)es, outer);
            }
            else if (es is Metadata.Expression.IndifierExp)
            {
                return ExpressionToString((Metadata.Expression.IndifierExp)es, outer);
            }
            else if (es is Metadata.Expression.BaseExp)
            {
                return ExpressionToString((Metadata.Expression.BaseExp)es, outer);
            }
            else if (es is Metadata.Expression.AssignmentExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.AssignmentExpressionSyntax)es, outer);
            }
            else if (es is Metadata.Expression.BinaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.BinaryExpressionSyntax)es, outer);
            }
            else if (es is Metadata.Expression.PrefixUnaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.PrefixUnaryExpressionSyntax)es, outer);
            }
            else if (es is Metadata.Expression.PostfixUnaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.PostfixUnaryExpressionSyntax)es, outer);
            }
            else if (es is Metadata.Expression.ParenthesizedExpressionSyntax)
            {
                return ExpressionToString(((Metadata.Expression.ParenthesizedExpressionSyntax)es), outer);
            }
            else if (es is Metadata.Expression.ElementAccessExp)
            {
                return ExpressionToString(((Metadata.Expression.ElementAccessExp)es), outer);
            }
            else
            {
                Console.Error.WriteLine("不支持的表达式 " + es.GetType().Name);
            }
            return "";
        }

        //static string ExpressionToString(Metadata.DB_InitializerExpressionSyntax es)
        //{
        //    StringBuilder ExpSB = new StringBuilder();
        //    if(es.Expressions.Count>0)
        //    {
        //        ExpSB.Append("(");
        //    }

        //    for(int i=0;i<es.Expressions.Count;i++)
        //    {
        //        ExpSB.Append(ExpressionToString(es.Expressions[i]));
        //        if (i < es.Expressions.Count - 2)
        //            ExpSB.Append(",");
        //    }

        //    if (es.Expressions.Count > 0)
        //    {
        //        ExpSB.Append(")");
        //    }

        //    return ExpSB.ToString();
        //}

        string GetExpConversion(Metadata.DB_Type left_type, Metadata.DB_Type right_type, Metadata.Expression.Exp right, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (left_type.GetRefType() != right_type.GetRefType() && !left_type.IsAssignableFrom(right_type, Model))
            {
                //查看是否有隐式转换的方法
                List<Metadata.DB_Type> args = new List<Metadata.DB_Type>();
                args.Add(right_type);
                Metadata.DB_Member operatorMethod = right_type.FindMethod(left_type.name, args, Model);
                if (operatorMethod != null && operatorMethod.method_is_conversion_operator)
                {
                    stringBuilder.Append(string.Format("{0}.{1}({2})", GetCppTypeName(right_type), GetMethodUniqueName(operatorMethod), ExpressionToString(right)));
                }
                else
                {
                    stringBuilder.Append(ExpressionToString(right));
                    Console.Error.WriteLine("类型不能转换 " + stringBuilder.ToString());
                }
            }
            else
                stringBuilder.Append(ExpressionToString(right, outer));





            return stringBuilder.ToString();
        }




        string ExpressionToString(Metadata.Expression.MethodExp es, Metadata.DB_Member method)
        {
            List<Metadata.DB_Type> args = new List<Metadata.DB_Type>();
            for (int i = 0; i < es.Args.Count; i++)
            {
                args.Add(Model.GetExpType(es.Args[i]));
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            if (es.Args != null)
            {
                //引用参数计数
                List<int> refArgs = new List<int>();

                for (int i = 0; i < es.Args.Count; i++)
                {
                    //实际参数类型
                    Metadata.DB_Type arg_type = args[i];
                    //实际参数是this
                    if (es.Args[i] is Metadata.Expression.ThisExp)
                    {
                        if (arg_type.is_value_type)
                        {
                            stringBuilder.Append("*");
                        }
                    }

                    if (method.method_args[i].is_out || method.method_args[i].is_ref)
                        refArgs.Add(i);

                    //形式参数类型
                    Metadata.DB_Type me_argType = Model.GetType(method.method_args[i].type);

                    string ArgString = GetExpConversion(me_argType, arg_type, es.Args[i], es);


                    if (me_argType.is_value_type)
                    {
                        stringBuilder.Append(string.Format("clone({0})", ArgString));
                    }
                    else
                    {
                        stringBuilder.Append(ArgString);
                    }

                    if (i < es.Args.Count - 1)
                        stringBuilder.Append(",");
                }

                if(refArgs.Count> 0)
                {
                    stringBuilder.AppendLine(",");
                    stringBuilder.Append("function(");

                    for(int i=0;i< refArgs.Count;i++)
                    {
                        stringBuilder.Append(method.method_args[refArgs[i]].name);
                        if (i < refArgs.Count - 1)
                            stringBuilder.Append(",");
                    }

                    stringBuilder.Append(")");

                    stringBuilder.AppendLine();


                    for (int i = 0; i < refArgs.Count; i++)
                    {
                        stringBuilder.AppendLine(string.Format("{0}={1}", ExpressionToString( es.Args[refArgs[i]]), method.method_args[refArgs[i]].name));
                    }

                    stringBuilder.AppendLine("end");
                }

            }
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }

        //void GetMethod(Metadata.Expression.MethodExp es,out Metadata.DB_Type caller,out Metadata.DB_Member method)
        //{
        //    List<Metadata.DB_Type> args = new List<Metadata.DB_Type>();
        //    for (int i = 0; i < es.Args.Count; i++)
        //    {
        //        args.Add(Model.GetExpType(es.Args[i]));
        //    }

        //    if (es.Caller is Metadata.Expression.IndifierExp)
        //    {
        //        //stringBuilder.Append(ExpressionToString(es.Caller));

        //        Metadata.Expression.IndifierExp ie = es.Caller as Metadata.Expression.IndifierExp;
        //        Metadata.Model.IndifierInfo ii = Model.GetIndifierInfo(ie.Name);
        //        caller = ii.type;
        //        if (ii.is_var)
        //        {
        //            method = caller.FindMethod("Invoke", args, Model);
        //            //stringBuilder.Append("->Invoke");
        //            return;
        //        }
        //        else
        //        {
        //            caller = Model.currentType;
        //            method = caller.FindMethod(ie.Name, args, Model);
        //            return;
        //        }
        //    }
        //    else if (es.Caller is Metadata.Expression.FieldExp)
        //    {
        //        Metadata.Expression.FieldExp fe = es.Caller as Metadata.Expression.FieldExp;
        //        caller = Model.GetExpType(fe.Caller, fe);
        //        method = caller.FindMethod(fe.Name, args, Model);

        //        return;
        //        //stringBuilder.Append(ExpressionToString(es.Caller));

        //        //stringBuilder.Append("::");
        //        //caller_type = Model.GetExpType(es.Caller);
        //    }
        //    else
        //    {
        //        caller = null;
        //        method = null;
        //        return;
        //    }
        //}

        string GetMethodUniqueName(Metadata.DB_Member method)
        {
            if(method.method_is_operator)
            {
                return GetOperatorFuncName(method.name, method.method_args.Length);
            }

            if(Model.GetType(method.declaring_type).is_generic_type)
            {
                return method.name;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(method.name);
            if(method.method_args.Length>0)
                sb.Append("_");
            for (int i=0;i<method.method_args.Length;i++)
            {
                string TypeName = GetCppTypeName(Model.GetType(method.method_args[i].type));
                TypeName = TypeName.Replace(".", "_");
                sb.Append(TypeName);
                //sb.Append(method.method_args[i].name);
                if(i< method.method_args.Length-1)
                    sb.Append("_");
            }
            return sb.ToString();
        }

        string ExpressionToString(Metadata.Expression.MethodExp es, Metadata.Expression.Exp outer)
        {
            ITypeConverter tc = Converter.GetTypeConverter(Model.currentType);
            if (tc != null)
            {
                string content;
                if (tc.ConvertMethodExp(Converter, Model.currentType, es, out content))
                {
                    return content;
                }
            }


            StringBuilder stringBuilder = new StringBuilder();


            Metadata.DB_Type caller_type = null;

            List<Metadata.DB_Type> args = new List<Metadata.DB_Type>();
            for (int i = 0; i < es.Args.Count; i++)
            {
                args.Add(Model.GetExpType(es.Args[i]));
            }
            Metadata.DB_Member method = null;

            if (es.Caller is Metadata.Expression.IndifierExp)
            { 
                Metadata.Expression.IndifierExp ie = es.Caller as Metadata.Expression.IndifierExp;
                Metadata.Model.IndifierInfo ii = Model.GetIndifierInfo(ie.Name);
                caller_type = ii.type;
                if (ii.is_var)
                {
                    method = caller_type.FindMethod("Invoke", args, Model);
                    stringBuilder.Append(ExpressionToString(es.Caller));
                    stringBuilder.Append(":Invoke");
                }
                else if (ii.is_event)
                {
                    method = caller_type.FindMethod("Invoke", args, Model);
                    stringBuilder.Append(ExpressionToString(es.Caller));
                    stringBuilder.Append(":Invoke");
                }
                else
                {
                    
                    caller_type = Model.currentType;
                    method = caller_type.FindMethod(ie.Name, args, Model);

                    if (ii.is_method || ii.is_field || ii.is_property)
                    {
                        if (method != null)
                        {
                            if (method.is_static)
                            {
                                stringBuilder.Append(GetCppTypeName(Model.GetType(method.declaring_type)));
                                stringBuilder.Append(".");
                            }
                            else
                            {
                                stringBuilder.Append("self");
                                stringBuilder.Append(":");
                            }
                        }

                    }

                    stringBuilder.Append(GetMethodUniqueName(method));
                }
            }
            else if (es.Caller is Metadata.Expression.FieldExp)
            {
                Metadata.Expression.FieldExp fe = es.Caller as Metadata.Expression.FieldExp;
                caller_type = Model.GetExpType(fe.Caller, fe);
                method = caller_type.FindMethod(fe.Name, args, Model);

                stringBuilder.Append(ExpressionToString(fe.Caller));
                if(method.is_static)
                {
                    stringBuilder.Append("."+ GetMethodUniqueName(method));
                }
                else
                {
                    stringBuilder.Append(":" + GetMethodUniqueName(method));
                }
            }

            stringBuilder.Append(ExpressionToString(es, method));
            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.ConstExp es, Metadata.Expression.Exp outer)
        {
            
            if (es.value == "null")
            {
                return "nil";
            }

            Metadata.DB_Type type = Model.GetExpType(es, outer);

            if(type!=null)
            {
                return string.Format("{0}.new({1})", GetCppTypeName(type),es.value);
            }

            //if (!string.IsNullOrEmpty(es.value))
            //{
            //    if (es.value.Length > 0 && es.value[0] == '"')
            //        return "Ref<System::String>(new System::String(_T(" + es.value + ")))";
            //    else if (es.value.StartsWith("'"))
            //        return "_T(" + es.value + ")";
            //}
            return es.value;
        }
        string ExpressionToString(Metadata.Expression.FieldExp es, Metadata.Expression.Exp right = null)
        {
            ITypeConverter tc = Converter.GetTypeConverter(Model.currentType);
            if (tc != null)
            {
                string content;
                if (tc.ConvertFieldExp(Converter, Model.currentType, es, out content))
                {
                    return content;
                }
            }

            //if (es.Caller == null)   //本地变量或者类变量，或者全局类
            //{
            //    return es.Name;
            //}
            //else
            {
                StringBuilder stringBuilder = new StringBuilder();
                

                Metadata.DB_Type caller_type = null;

                if (es.Caller is Metadata.Expression.IndifierExp)
                {
                    Metadata.Expression.IndifierExp ie = es.Caller as Metadata.Expression.IndifierExp;
                    Metadata.Model.IndifierInfo ii = Model.GetIndifierInfo(ie.Name);
                    caller_type = ii.type;

                    
                    stringBuilder.Append(ExpressionToString(es.Caller, es));

                    if (ii.is_namespace || ii.is_type)
                    {
                        stringBuilder.Append(".");

                    }
                    else
                    {
                        if (caller_type.is_class)
                        {
                            if(caller_type.FindProperty(es.Name,Model)!=null)   //属性，方法调用
                                stringBuilder.Append(":");
                            else
                                stringBuilder.Append(".");
                        }
                        else
                        {
                            stringBuilder.Append(".");
                        }
                    }

                }
                else
                {
                    stringBuilder.Append(ExpressionToString(es.Caller, es));

                    caller_type = Model.GetExpType(es.Caller);

                    if (caller_type != null)
                    {
                        if (caller_type.is_class)
                        {
                            stringBuilder.Append(".");
                        }
                        else
                        {
                            stringBuilder.Append(".");
                        }
                    }
                }

                stringBuilder.Append(ExportProperty(es.Name, caller_type, es, right));
                return stringBuilder.ToString();
            }
        }

        string ExportProperty(string Name, Metadata.DB_Type caller_type, Metadata.Expression.Exp This, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool property = false;
            if (caller_type != null)
            {
                Metadata.DB_Member member = caller_type.FindMember(Name, Model);
                if (member != null)
                {
                    if (member.member_type == (int)Metadata.MemberTypes.Property)
                    {
                        property = true;
                        bool lefgValue = false;
                        if (outer != null && outer is Metadata.Expression.AssignmentExpressionSyntax)
                        {
                            Metadata.Expression.AssignmentExpressionSyntax aes = outer as Metadata.Expression.AssignmentExpressionSyntax;
                            if (aes.Left == This)
                            {
                                lefgValue = true;
                            }
                        }

                        if (!lefgValue)
                            stringBuilder.Append(member.property_get + "()");
                        else
                            stringBuilder.Append(member.property_set + "(" + ExpressionToString(outer) + ")");

                    }

                }
            }
            if (!property)
            {
                stringBuilder.Append(Name);

            }

            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.ObjectCreateExp es, Metadata.Expression.Exp outer)
        {
            StringBuilder ExpSB = new StringBuilder();

            Metadata.DB_Type type = Model.GetType(es.Type);
            List<Metadata.DB_Type> args = new List<Metadata.DB_Type>();
            if (es.Args != null)
            {
                for (int i = 0; i < es.Args.Count; i++)
                {
                    args.Add(Model.GetExpType(es.Args[i]));
                }
            }
            Metadata.DB_Member constrctorMethod = type.FindMethod(type.name, args, Model);

            if (constrctorMethod != null)
            {
                ExpSB.Append("Construct(");
            }

            ExpSB.Append(GetCppTypeName(type));
            ExpSB.Append(".new()");

            if (constrctorMethod != null)
            {
                ExpSB.Append(",\""+GetMethodUniqueName(constrctorMethod)+"\"");
                if (es.Args != null && es.Args.Count > 0)
                {
                    ExpSB.Append(",");
                }

                if (es.Args != null)
                {
                    for (int i = 0; i < es.Args.Count; i++)
                    {
                        ExpSB.Append(ExpressionToString(es.Args[i], es));
                        if (i < es.Args.Count - 2)
                            ExpSB.Append(",");
                    }
                }

                ExpSB.Append(")");
            }

            return ExpSB.ToString();
        }
        string ExpressionToString(Metadata.Expression.BaseExp es, Metadata.Expression.Exp outer)
        {
            return GetCppTypeName(Model.GetType(Model.currentType.base_type));
        }

        //string ExpressionToString(Metadata.VariableDeclaratorSyntax es)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append(es.Identifier);
        //    if (es.Initializer != null)
        //    {
        //        stringBuilder.Append("=");
        //        stringBuilder.Append(ExpressionToString(es.Initializer));
        //    }

        //    return stringBuilder.ToString();
        //}

        string MakeDelegate(Metadata.DB_Member delegateMethod, Metadata.Expression.Exp exp)
        {
            if (exp is Metadata.Expression.IndifierExp)
            {
                Metadata.Expression.IndifierExp indifierExp = exp as Metadata.Expression.IndifierExp;
                Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(indifierExp.Name);
                if (!info.is_method)
                    return ExpressionToString(exp);
            }
            StringBuilder stringBuilder = new StringBuilder();
            Metadata.DB_Type type = Model.GetType(delegateMethod.declaring_type);
            Metadata.DB_Member right_method = GetExpDelegateMethod(delegateMethod, exp);
            Metadata.DB_Type caller = Model.GetType(right_method.declaring_type);

            if (right_method.is_static)
            {
                stringBuilder.Append(string.Format("{0}.new(nil,{1})", GetCppTypeName(type), ExpressionToString(exp)));
            }
            else
            {
                if (exp is Metadata.Expression.FieldExp)
                {
                    Metadata.Expression.FieldExp fe = exp as Metadata.Expression.FieldExp;
                    stringBuilder.Append(string.Format("{0}.new({2},{3}.{4})", GetCppTypeName(type), GetCppTypeName(caller), ExpressionToString(fe.Caller), GetCppTypeName(caller), ExpressionToString(exp)));
                }
                else if (exp is Metadata.Expression.IndifierExp)
                {
                    Metadata.Expression.IndifierExp fe = exp as Metadata.Expression.IndifierExp;
                    stringBuilder.Append(string.Format("{0}.new({2},{3}.{4})", GetCppTypeName(type), GetCppTypeName(caller), "self", GetCppTypeName(caller), ExpressionToString(exp) ));
                }
            }

            return stringBuilder.ToString();
        }

        Metadata.DB_Member GetExpDelegateMethod(Metadata.DB_Member delegateMethod, Metadata.Expression.Exp exp)
        {
            Metadata.DB_Member right_method = null;
            List<Metadata.DB_Member> methods = null;
            if (exp is Metadata.Expression.FieldExp)
            {
                Metadata.Expression.FieldExp fe = exp as Metadata.Expression.FieldExp;
                Metadata.DB_Type caller = Model.GetExpType(fe.Caller, fe);
                methods = caller.FindMethod(fe.Name, Model);

            }
            else if (exp is Metadata.Expression.IndifierExp)
            {
                Metadata.Expression.IndifierExp indifierExp = exp as Metadata.Expression.IndifierExp;
                Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(indifierExp.Name);
                if (info.is_method)
                {
                    methods = Model.currentType.FindMethod(indifierExp.Name, Model);
                }
            }


            foreach (var m in methods)
            {
                if (m.method_args.Length != delegateMethod.method_args.Length)
                    continue;
                bool martch = true;
                for (int arg_index = 0; arg_index < m.method_args.Length; arg_index++)
                {
                    if (m.method_args[arg_index].type != delegateMethod.method_args[arg_index].type
                        || m.method_args[arg_index].is_out != delegateMethod.method_args[arg_index].is_out
                        || m.method_args[arg_index].is_params != delegateMethod.method_args[arg_index].is_params
                        || m.method_args[arg_index].is_ref != delegateMethod.method_args[arg_index].is_ref
                        )
                    {
                        martch = false;
                        break;
                    }
                }

                if (!martch)
                {
                    continue;
                }

                right_method = m;
                break;

            }

            return right_method;
        }


        string ExpressionToString(Metadata.VariableDeclarationSyntax es, Metadata.Expression.Exp outer = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Metadata.DB_Type type = Model.GetType(es.Type);
            //if (type.is_class)
            //{
            //    if (type.is_delegate)
            //    {

            //    }
            //    else
            //    {
            //        Append("Ref<" + GetCppTypeName(type) + "> ");
            //    }
            //}
            //else
            //    Append(GetCppTypeName(type) + " ");
            Append("local ");
            //stringBuilder.Append(GetCppTypeName(Model.GetType(es.Type)));
            //stringBuilder.Append(" ");
            for (int i = 0; i < es.Variables.Count; i++)
            {
                if (!type.is_delegate)
                {
                    Metadata.VariableDeclaratorSyntax esVar = es.Variables[i];
                    stringBuilder.Append(esVar.Identifier);
                    if (esVar.Initializer != null)
                    {
                        //if (esVar.Initializer is Metadata.Expression.IndifierExp)
                        //{
                        //    Metadata.Expression.IndifierExp indifierExp = esVar.Initializer as Metadata.Expression.IndifierExp;
                        //    Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(indifierExp.Name);
                        //    if(info.is_method)
                        //    {

                        //    }
                        //}
                        //else
                        {
                            Metadata.DB_Type right_type = Model.GetExpType(esVar.Initializer);

                            stringBuilder.Append(" = ");
                            if (right_type != null)
                            {
                                string ArgString = GetExpConversion(type, right_type, esVar.Initializer, null);
                                stringBuilder.Append(ArgString);
                            }
                            else
                            {

                                stringBuilder.Append(ExpressionToString(esVar.Initializer));

                            }
                        }

                    }
                }
                else
                {
                    Metadata.VariableDeclaratorSyntax esVar = es.Variables[i];

                    Metadata.DB_Member delegateMethod = type.FindMethod("Invoke", Model)[0];

                    if (esVar.Initializer != null)
                    {
                        Metadata.DB_Member right_method = GetExpDelegateMethod(delegateMethod, esVar.Initializer);
                        Metadata.DB_Type caller = Model.GetType(right_method.declaring_type);

                        stringBuilder.Append(string.Format("{1} = ", GetCppTypeName(type), esVar.Identifier));
                        stringBuilder.Append(MakeDelegate(delegateMethod, esVar.Initializer));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("{1}", GetCppTypeName(type), esVar.Identifier));
                    }

                }
                if (i < es.Variables.Count - 1)
                    stringBuilder.Append(",");
            }
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.IndifierExp es, Metadata.Expression.Exp outer)
        {
            Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(es.Name);

            if (info.is_type)
            {
                ITypeConverter tc = Converter.GetTypeConverter(info.type);
                string content;
                if (tc != null && tc.GetCppTypeName(Converter, info.type,out content))
                {
                    return content;
                }

                return GetCppTypeName(info.type);
            }
            else if(info.is_field)
            {
                if(info.field.is_static)
                {
                    return GetCppTypeName(Model.currentType)+"." + es.Name;
                }
                else
                {
                    return "self."+es.Name;
                }
            }
            else if(info.is_property)
            {
                if (info.field.is_static)
                {
                    

                    return GetCppTypeName(Model.currentType) + "." + ExportProperty(es.Name, Model.currentType, es, outer);
                }
                else
                {
                    return "self:" + ExportProperty(es.Name, Model.currentType, es, outer);
                }
            }
            else if(info.is_method)
            {
                if (info.methods[0].is_static)
                {
                    return GetCppTypeName(Model.currentType) + "." + GetMethodUniqueName(info.methods[0]);
                }
                else
                {
                    return "self:" + info.methods[0].name;
                }
            }

            return es.Name;
        }

        string ExpressionToString(Metadata.Expression.ThisExp exp, Metadata.Expression.Exp outer)
        {
            return "self";
        }
        string ExpressionToString(Metadata.Expression.AssignmentExpressionSyntax exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exp.OperatorToken == "=")
            {
                Metadata.DB_Type left_type = Model.GetExpType(exp.Left);
                Metadata.DB_Type right_type = Model.GetExpType(exp.Right);

                //if(exp.Left is Metadata.Expression.FieldExp)
                //{
                //    stringBuilder.Append( ExpressionToString(exp.Left as Metadata.Expression.FieldExp, exp.Right));
                //}
                //else
                {
                    stringBuilder.Append(ExpressionToString(exp.Left, exp));
                    stringBuilder.Append(" = ");
                    string ArgString = GetExpConversion(left_type, right_type, exp.Right, exp);
                    stringBuilder.Append(ArgString);
                }


            }
            else
            {
                if (exp.OperatorToken == "+=" || exp.OperatorToken == "-=")
                {
                    Metadata.DB_Member eventMember = null;
                    if (exp.Left is Metadata.Expression.IndifierExp)
                    {
                        Metadata.Expression.IndifierExp indifierExp = exp.Left as Metadata.Expression.IndifierExp;
                        Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(indifierExp.Name);
                        if (info.is_property)
                        {
                            //eventMember = Model.currentType.FindEvent(indifierExp.Name, Model);
                            if (eventMember == null)
                            {
                                eventMember = Model.currentType.FindProperty(indifierExp.Name, Model);
                                if (eventMember != null && !eventMember.IsEventProperty(Model))
                                {
                                    eventMember = null;
                                }
                            }
                        }
                    }
                    else if (exp.Left is Metadata.Expression.FieldExp)
                    {
                        Metadata.Expression.FieldExp fieldExp = exp.Left as Metadata.Expression.FieldExp;
                        eventMember = Model.GetExpType(fieldExp.Caller).FindEvent(fieldExp.Name, Model);
                        if (eventMember == null)
                        {
                            eventMember = Model.GetExpType(fieldExp.Caller).FindProperty(fieldExp.Name, Model);
                            if (eventMember != null && !eventMember.IsEventProperty(Model))
                            {
                                eventMember = null;
                            }
                        }
                    }

                    //左边是事件
                    if (eventMember != null)
                    {
                        Metadata.DB_Type declareType = Model.GetType(eventMember.declaring_type);
                        Metadata.DB_Type delegateType = Model.GetType(eventMember.type);
                        Metadata.DB_Member delegateMethod = delegateType.members.First().Value;
                        if (exp.OperatorToken == "+=")
                        {
                            stringBuilder.AppendFormat("{0}.{1}({2})", GetCppTypeName(declareType),GetMethodUniqueName( Model.currentType.FindMethod(  eventMember.property_add,Model)[0]), MakeDelegate(delegateMethod, exp.Right));
                            return stringBuilder.ToString();
                        }
                        else if (exp.OperatorToken == "-=")
                        {
                            stringBuilder.AppendFormat("{0}.{1}({2})", GetCppTypeName(declareType), GetMethodUniqueName(Model.currentType.FindMethod(eventMember.property_remove,Model)[0]), MakeDelegate(delegateMethod, exp.Right));
                            return stringBuilder.ToString();
                        }
                    }
                }


                string token = exp.OperatorToken.Replace("=", "");
                Metadata.Expression.BinaryExpressionSyntax binaryExpressionSyntax = new Metadata.Expression.BinaryExpressionSyntax();
                binaryExpressionSyntax.Left = exp.Left;
                binaryExpressionSyntax.Right = exp.Right;
                binaryExpressionSyntax.OperatorToken = token;

                //if (exp.Left is Metadata.Expression.FieldExp)
                //{
                //    stringBuilder.Append(ExpressionToString(exp.Left as Metadata.Expression.FieldExp, binaryExpressionSyntax));
                //}
                //else
                {
                    stringBuilder.Append(ExpressionToString(exp.Left, exp));
                    stringBuilder.Append(" = ");
                    stringBuilder.Append(ExpressionToString(binaryExpressionSyntax));
                }



                //Console.Error.WriteLine("无法解析的操作符 " + exp.OperatorToken);
            }

            //Console.WriteLine(stringBuilder.ToString());

            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.BinaryExpressionSyntax exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Metadata.DB_Type left_type = Model.GetExpType(exp.Left);
            Metadata.DB_Type right_type = Model.GetExpType(exp.Right);
            List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
            argTypes.Add(left_type);
            argTypes.Add(right_type);

            if (exp.OperatorToken == "&&" || exp.OperatorToken == "||")
            {
                stringBuilder.Append("(");
                stringBuilder.Append(ExpressionToString(exp.Left, exp));
                stringBuilder.Append(")?");
                if (exp.OperatorToken == "&&")
                {
                    stringBuilder.Append("(");
                    stringBuilder.Append(ExpressionToString(exp.Right, exp));
                    stringBuilder.Append("):false");
                }
                else
                {
                    stringBuilder.Append("true");
                    stringBuilder.Append(":(");
                    stringBuilder.Append(ExpressionToString(exp.Right, exp));
                    stringBuilder.Append(")");
                }
                return stringBuilder.ToString();
            }


            Metadata.DB_Member method = left_type.FindMethod(exp.OperatorToken, argTypes, Model);
            if (method != null)
            {
                stringBuilder.Append(string.Format("{0}.{1}(", GetCppTypeName(left_type), GetOperatorFuncName(exp.OperatorToken, argTypes.Count)));
            }
            else
            {
                method = right_type.FindMethod(exp.OperatorToken, argTypes, Model);
                if (method == null)
                {
                    Console.Error.WriteLine("操作符没有重载的方法 " + exp.ToString());
                    return stringBuilder.ToString();
                }
                stringBuilder.Append(string.Format("{0}.{1}(", GetCppTypeName(left_type), GetOperatorFuncName(exp.OperatorToken, argTypes.Count)));
            }

            if (left_type.is_value_type)
            {
                stringBuilder.Append(string.Format("clone({0})", ExpressionToString(exp.Left, exp)));
            }
            else
            {
                stringBuilder.Append(ExpressionToString(exp.Left, exp));
            }
            stringBuilder.Append(",");

            if (right_type.is_value_type)
            {
                stringBuilder.Append(string.Format("clone({0})", ExpressionToString(exp.Right, exp)));
            }
            else
            {
                stringBuilder.Append(ExpressionToString(exp.Right, exp));
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.PostfixUnaryExpressionSyntax exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exp.Operand is Metadata.Expression.ConstExp)
            {
                stringBuilder.Append(ExpressionToString(exp.Operand, exp));
                stringBuilder.Append(exp.OperatorToken);
            }
            else
            {
                string funcName = GetOperatorFuncName(exp.OperatorToken, 1);
                Metadata.DB_Type caller = Model.GetExpType(exp.Operand);
                Metadata.DB_Member func = caller.FindMethod(exp.OperatorToken, new List<Metadata.DB_Type>() { caller }, Model);

                if (exp.OperatorToken == "++" || exp.OperatorToken == "--")
                    stringBuilder.Append(string.Format("PostfixUnaryHelper.{0}({1},function(v) {1} = v end)", funcName, ExpressionToString(exp.Operand, exp)));
                else
                    stringBuilder.Append(string.Format("{0}.{1}({2})", GetCppTypeName(caller), funcName, ExpressionToString(exp.Operand, exp)));

            }

            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.PrefixUnaryExpressionSyntax exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (exp.Operand is Metadata.Expression.ConstExp)
            {
                stringBuilder.Append(exp.OperatorToken);
                stringBuilder.Append(ExpressionToString(exp.Operand, exp));
            }
            else
            {
                string funcName = GetOperatorFuncName(exp.OperatorToken, 1);
                Metadata.DB_Type caller = Model.GetExpType(exp.Operand);
                Metadata.DB_Member func = caller.FindMethod(exp.OperatorToken, new List<Metadata.DB_Type>() { caller }, Model);

                if (exp.OperatorToken == "++" || exp.OperatorToken == "--")
                    stringBuilder.Append(string.Format("PrefixUnaryHelper.{0}({1},function(v) {1} = v end)", funcName,  ExpressionToString(exp.Operand, exp)));
                else
                    stringBuilder.Append(string.Format("{0}.{1}({2})", GetCppTypeName(caller), funcName, ExpressionToString(exp.Operand, exp)));

            }

            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.ParenthesizedExpressionSyntax exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append(ExpressionToString(exp.exp, exp));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.ElementAccessExp exp, Metadata.Expression.Exp outer)
        {
            StringBuilder stringBuilder = new StringBuilder();


            stringBuilder.Append(ExpressionToString(exp.exp, exp));

            Metadata.DB_Type callerType = Model.GetExpType(exp.exp, exp);
            if (callerType.is_class)
            {
                stringBuilder.Append(".");
            }
            else
            {
                stringBuilder.Append(".");
            }

            bool leftValue = false;
            if (outer != null && outer is Metadata.Expression.AssignmentExpressionSyntax)
            {
                Metadata.Expression.AssignmentExpressionSyntax ae = outer as Metadata.Expression.AssignmentExpressionSyntax;
                if (ae.Left == exp)
                {
                    leftValue = true;

                }
            }

            if (leftValue)
                stringBuilder.Append("set_Index(");
            else
                stringBuilder.Append("get_Index(");

            for (int i = 0; i < exp.args.Count; i++)
            {
                stringBuilder.Append(ExpressionToString(exp.args[i], exp));
                if (i < exp.args.Count - 1)
                    stringBuilder.Append(",");
            }
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }

    }
}
