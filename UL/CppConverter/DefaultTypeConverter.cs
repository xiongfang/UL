using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    class DefaultTypeConverter:IDefaultTypeConverter
    {
        int depth;
        StringBuilder sb = new StringBuilder();
        IConverter Converter;

        public DefaultTypeConverter(IConverter cv) { this.Converter = cv; }

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

        void Append(string msg)
        {
            AppendDepth();
            sb.Append(msg);
        }


        public void ConvertTypeHeader(Metadata.DB_Type type)
        {
            Model.EnterNamespace(type._namespace);
            Model.EnterType(type);
            //头文件
            {
                sb.Clear();
                sb.AppendLine("#pragma once");

                //包含头文件
                HashSet<string> depTypes = Converter.GetTypeDependences(type);

                HashSet<string> NoDeclareTypes = Converter.GetTypeDependencesNoDeclareType(type);
                foreach (var t in depTypes)
                {
                    Metadata.DB_Type depType = Model.GetType(t);
                    if (!depType.is_generic_paramter && t != type.static_full_name)
                    {
                        if (NoDeclareTypes.Contains(t))
                        {
                            sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
                        }
                        else
                        {
                            //前向声明
                            sb.AppendLine("namespace " + depType._namespace);
                            sb.AppendLine("{");
                            if (depType.is_generic_type_definition)
                            {
                                sb.Append("template");
                                sb.Append("<");
                                for (int i = 0; i < depType.generic_parameter_definitions.Count; i++)
                                {
                                    sb.Append(depType.generic_parameter_definitions[i].type_name);
                                    if (i < depType.generic_parameter_definitions.Count - 1)
                                        sb.Append(",");
                                }
                                sb.AppendLine(">");
                                if (depType.is_value_type)
                                    sb.AppendLine("struct " + depType.name + ";");
                                else
                                    sb.AppendLine("class " + depType.name + ";");
                            }
                            else
                            {
                                if (depType.is_value_type)
                                    sb.AppendLine("struct " + depType.name + ";");
                                else
                                    sb.AppendLine("class " + depType.name + ";");
                            }
                            sb.AppendLine("}");
                        }
                    }
                }


                //if (HasCppAttribute(type.attributes))
                //{
                //    //包含虚幻生成的头文件
                //    AppendLine(string.Format("#include \"{0}.generated.h\"", type.name));

                //    //属性
                //    AppendLine(ConvertCppAttribute(type.attributes));
                //}


                sb.AppendLine(string.Format("namespace {0}{{", type._namespace));
                {
                    depth++;
                    if (type.is_enum)
                    {
                        Append(string.Format("enum {0}", type.name));
                    }
                    else
                    {
                        if (type.is_generic_type_definition)
                        {
                            Append("template<");
                            for (int i = 0; i < type.generic_parameter_definitions.Count; i++)
                            {
                                sb.Append("class " + type.generic_parameter_definitions[i].type_name);
                                if (i < type.generic_parameter_definitions.Count - 1)
                                    sb.Append(",");
                            }
                            sb.AppendLine(">");
                        }

                        if (type.is_value_type)
                        {
                            Append(string.Format("struct {0}", type.name));
                        }
                        else
                        {
                            Append(string.Format("class {0}", type.name));
                        }
                        if (!type.base_type.IsVoid /*&& !type.is_value_type*/ || type.interfaces.Count > 0)
                        {
                            sb.Append(":");
                            if (!type.base_type.IsVoid /*&& !type.is_value_type*/)
                            {
                                sb.Append("public " + GetCppTypeName(Model.GetType(type.base_type)));
                                if (type.interfaces.Count > 0)
                                    sb.Append(",");
                            }
                            for (int i = 0; i < type.interfaces.Count; i++)
                            {
                                sb.Append("public " + GetCppTypeName(Model.GetType(type.interfaces[i])));
                                if (i < type.interfaces.Count - 1)
                                    sb.Append(",");
                            }
                            sb.AppendLine();
                        }
                    }

                    AppendLine("{");
                    {
                        depth++;

                        if (type.is_enum)
                        {
                            List<Metadata.DB_Member> members = type.members.Values.ToList();
                            members.Sort((a, b) => { return a.order <= b.order ? -1 : 1; });
                            for (int i = 0; i < members.Count; i++)
                            {
                                Append(members[i].name);
                                if (i < members.Count - 1)
                                    sb.Append(",");
                                sb.AppendLine();
                            }
                        }
                        else
                        {
                            //if (HasCppAttribute(type.attributes))
                            //{
                            //    AppendLine("GENERATED_BODY()");
                            //}

                            foreach (var m in type.members.Values)
                            {
                                ConvertMemberHeader(m);
                            }
                        }

                        depth--;
                    }

                    TypeConfig tc = Converter.GetTypeConfig(type);

                    if (tc != null)
                    {
                        if (!string.IsNullOrEmpty(tc.ext_header))
                        {
                            AppendLine("#include \"" + tc.ext_header + "\"");
                        }
                    }

                    AppendLine("};");
                    depth--;
                }

                sb.AppendLine("}");

                //System.IO.File.WriteAllText(System.IO.Path.Combine(outputDir, GetTypeHeader(type)), sb.ToString());
                Model.LeaveType();
                Model.LeaveNamespace();
                //return sb.ToString();
            }

        }
        public bool ConvertTypeCpp(Metadata.DB_Type type)
        {
            //cpp文件
            {
                sb.Clear();
                Project cfg = Converter.GetProject();
                if (!type.is_enum && !type.is_generic_type_definition)
                {
                    Model.EnterNamespace(type._namespace);
                    Model.EnterType(type);

                    if (!string.IsNullOrEmpty(cfg.precompile_header))
                    {
                        sb.AppendLine(string.Format("#include \"{0}\"", cfg.precompile_header));
                    }
                    sb.AppendLine("#include \"" + GetTypeHeader(type) + "\"");
                    //sb.AppendLine(string.Format("namespace {0}{{", type._namespace));

                    //包含依赖的头文件
                    HashSet<string> depTypes = Converter.GetMethodBodyDependences(type);
                    HashSet<string> headDepTypes = Converter.GetTypeDependences(type);
                    foreach (var t in headDepTypes)
                    {
                        Metadata.DB_Type depType = Model.GetType(t);
                        if (!depType.is_generic_paramter && t != type.static_full_name)
                            sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
                    }
                    foreach (var t in depTypes)
                    {
                        if (!headDepTypes.Contains(t))
                        {
                            Metadata.DB_Type depType = Model.GetType(t);
                            if (!depType.is_generic_paramter && t != type.static_full_name)
                                sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
                        }
                    }


                    foreach (var us in type.usingNamespace)
                    {
                        sb.AppendLine("using namespace " + us + ";");
                    }

                    TypeConfig tc = Converter.GetTypeConfig(type);

                    if (tc != null)
                    {
                        if (!string.IsNullOrEmpty(tc.ext_cpp))
                        {
                            AppendLine("#include \"" + tc.ext_cpp + "\"");
                        }
                    }

                    foreach (var m in type.members.Values)
                    {
                        ConvertMemberCpp(m);
                    }

                    Model.LeaveType();
                    Model.LeaveNamespace();
                    return true;
                }
                
                
            }

            return false;
        }

        public string GetTypeHeader(Metadata.DB_Type type)
        {
            TypeConfig tc = Converter.GetTypeConfig(type);
            if (tc != null)
            {
                if (!string.IsNullOrEmpty(tc.header_path))
                    return tc.header_path;
            }

            return type.name + ".h";
        }

        public void ConvertType(Metadata.DB_Type type)
        {
            string outputDir = Converter.GetProject().export_dir;
            ITypeConverter tc = Converter.GetTypeConverter(type);
            if (tc != null)
            {
                sb.Clear();
                string content;
                if (tc.ConvertTypeHeader(Converter, type, out content))
                {
                    sb.Append(content);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(outputDir, GetTypeHeader(type)), sb.ToString(),Encoding.UTF8);
                }

                sb.Clear();
                if (tc.ConvertTypeCpp(Converter, type, out content))
                {
                    sb.Append(content);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(outputDir, type.name + ".cpp"), sb.ToString(), Encoding.UTF8);
                }
            }
            else
            {
                sb.Clear();
                ConvertTypeHeader(type);
                //sb.Append(ConvertTypeHeader(type));
                System.IO.File.WriteAllText(System.IO.Path.Combine(outputDir, GetTypeHeader(type)), sb.ToString(), Encoding.UTF8);


                sb.Clear();
                if(ConvertTypeCpp(type))
                {
                    System.IO.File.WriteAllText(System.IO.Path.Combine(outputDir, type.name + ".cpp"), sb.ToString(), Encoding.UTF8);
                }
            }
        }

        string GetCppTypeName(Metadata.DB_Type type)
        {
            ITypeConverter tc = Converter.GetTypeConverter(type);
            if (tc != null)
            {
                string name;
                if (tc.GetCppTypeName(out name))
                {
                    return name;
                }
            }
            if (type.is_generic_paramter)
                return type.name;
            if (type.is_generic_type)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(type._namespace);
                sb.Append("::");
                sb.Append(type.name);
                sb.Append("<");
                for (int i = 0; i < type.generic_parameters.Count; i++)
                {
                    sb.Append(GetCppTypeName(Model.GetType(type.generic_parameters[i])));
                    if (i < type.generic_parameters.Count - 1)
                        sb.Append(",");
                }
                sb.Append(">");
                return sb.ToString();
            }
            if (type.is_interface)
                return type._namespace + "::" + type.name;
            if (type.is_class)
                return type._namespace + "::" + type.name;
            if (type.is_value_type)
                return type._namespace + "::" + type.name;
            if (type.is_enum)
                return type._namespace + "::" + type.name;

            return type.static_full_name;
        }

        string GetCppTypeWrapName(Metadata.DB_Type type)
        {
            if (type.GetRefType().IsVoid)
                return "void";
            if (type.is_value_type)
            {
                return GetCppTypeName(type);
            }
            else
            {
                return string.Format("Ref<{0}>", GetCppTypeName(type));
            }

        }

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

        string GetOperatorFuncName(string token)
        {
            switch(token)
            {
                case "+":
                    return "op_Addition";
                case "-":
                    return "op_Substraction";
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
                default:
                    Console.Error.WriteLine("未知的操作符 " + token);
                    return token;
            }
        }

        void ConvertMemberHeader(Metadata.DB_Member member)
        {
            Metadata.DB_Type member_type = Model.GetType(member.typeName);
            if (member.member_type == (int)Metadata.MemberTypes.Field)
            {
                AppendLine(GetModifierString(member.modifier) + ":");
                //属性
                //AppendLine(ConvertCppAttribute(member.attributes));
                if (member.is_static)
                    Append("static ");
                else
                    Append("");


                //if (member_type.is_class)
                //    AppendLine(string.Format("{0}* {1};", GetCppTypeWrapName(Model.GetType(member.field_type)), member.name));
                //else
                    AppendLine(string.Format("{0} {1};", GetCppTypeWrapName(Model.GetType(member.field_type)), member.name));
            }
            else if (member.member_type == (int)Metadata.MemberTypes.Method)
            {
                Model.EnterMethod(member);

                AppendLine(GetModifierString(member.modifier) + ":");


                //属性
                //AppendLine(ConvertCppAttribute(member.attributes));

                if (member.is_static)
                    Append("static ");
                else
                    Append("");

                if (member.method_abstract)
                {
                    sb.Append("abstract ");
                }
                if (member.method_virtual)
                {
                    sb.Append("virtual ");
                }

                if (!member.method_is_constructor)
                {
                    string method_name = member.name;
                    if (member.method_is_operator)
                    {
                        method_name = GetOperatorFuncName(member.name);
                    }
                    sb.Append(string.Format("{1} {2}", "", member.method_ret_type.IsVoid ? "void" : GetCppTypeWrapName(Model.GetType(member.method_ret_type)), method_name));
                }
                else
                {
                    sb.Append(string.Format("{0}", member.name));
                }
                    
                sb.Append("(");
                if (member.method_args != null)
                {
                    for (int i = 0; i < member.method_args.Length; i++)
                    {
                        Metadata.DB_Type arg_Type = Model.GetType(member.method_args[i].type);
                        //string typeName = GetCppTypeName(arg_Type);
                        sb.Append(string.Format("{0} {1} {2}", GetCppTypeWrapName(arg_Type), (member.method_args[i].is_ref || member.method_args[i].is_out) ? "&" : "", member.method_args[i].name));
                        if (i < member.method_args.Length - 1)
                            sb.Append(",");
                    }
                }

                Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);

                if (declare_type.is_generic_type_definition)
                {
                    sb.AppendLine(")");
                    ConvertStatement(member.method_body);
                }
                else
                {
                    sb.AppendLine(");");
                }

                Model.LeaveMethod();
            }
        }

        void ConvertMemberCpp(Metadata.DB_Member member)
        {
            Metadata.DB_Type member_type = Model.GetType(member.typeName);
            if (member.member_type == (int)Metadata.MemberTypes.Field)
            {
                if (member.is_static)
                {
                    if (member_type.is_class)
                        AppendLine("Ref<" + GetCppTypeName(Model.GetType(member.field_type)) + "> " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name + ";");
                    else if (member_type.is_value_type)
                    {
                        Append(GetCppTypeName(Model.GetType(member.field_type)) + " " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name);
                        if (member.field_initializer != null)
                        {
                            sb.Append("=");
                            sb.Append(ExpressionToString(member.field_initializer));
                        }
                        sb.AppendLine(";");
                    }


                }
            }
            else if (member.member_type == (int)Metadata.MemberTypes.Method)
            {
                Model.EnterMethod(member);
                Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);
                if (!declare_type.is_generic_type_definition && member.method_body != null)
                {
                    if (!member.method_is_constructor)
                    {
                        string method_name = member.name;
                        if (member.method_is_operator)
                        {
                            method_name = GetOperatorFuncName(member.name);
                        }
                        sb.Append(string.Format("{0} {1}::{2}", member.method_ret_type.IsVoid ? "void" : GetCppTypeWrapName(Model.GetType(member.method_ret_type)), GetCppTypeName(Model.GetType(member.declaring_type)), method_name));
                    }
                    else
                        sb.Append(string.Format("{1}::{2}", "", GetCppTypeName(Model.GetType(member.declaring_type)), member.name));
                    sb.Append("(");
                    if (member.method_args != null)
                    {
                        for (int i = 0; i < member.method_args.Length; i++)
                        {
                            sb.Append(string.Format("{0} {1} {2}", GetCppTypeWrapName(Model.GetType(member.method_args[i].type)), (member.method_args[i].is_ref || member.method_args[i].is_out) ? "&" : "", member.method_args[i].name));
                            if (i < member.method_args.Length - 1)
                                sb.Append(",");
                        }
                    }
                    sb.AppendLine(")");

                    ConvertStatement(member.method_body);
                }
                Model.LeaveMethod();
            }
        }

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
                AppendLine("return " + ExpressionToString(((Metadata.DB_ReturnStatementSyntax)ss).Expression) + ";");
            }
            else if(ss is Metadata.DB_TryStatementSyntax)
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
            AppendLine("{");
            depth++;
            Model.EnterBlock();
            foreach (var s in bs.List)
            {
                ConvertStatement(s);
            }
            depth--;
            Model.LeaveBlock();
            AppendLine("}");
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

        void ConvertStatement(Metadata.DB_IfStatementSyntax bs)
        {
            AppendLine("if(" + ExpressionToString(bs.Condition) + ")");
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
        }

        void ConvertStatement(Metadata.DB_ExpressionStatementSyntax bs)
        {
            AppendLine(ExpressionToString(bs.Exp) + ";");
        }

        void ConvertStatement(Metadata.DB_LocalDeclarationStatementSyntax bs)
        {
            //Metadata.DB_Type type = Model.GetType(bs.Declaration.Type);
            //if (type.is_class)
            //    Append("Ref<" + GetCppTypeName(type) + "> ");
            //else
            //    Append(GetCppTypeName(type) + " ");

            sb.Append(ExpressionToString(bs.Declaration));
            //for (int i = 0; i < bs.Declaration.Variables.Count; i++)
            //{
            //    sb.Append(ExpressionToString(bs.Declaration.Variables[i]));
            //    if (i < bs.Declaration.Variables.Count - 2)
            //    {
            //        sb.Append(",");
            //    }
            //    Model.AddLocal(bs.Declaration.Variables[i].Identifier, Model.GetType(bs.Declaration.Type));
            //}
            sb.AppendLine(";");
        }
        void ConvertStatement(Metadata.DB_ForStatementSyntax bs)
        {
            Model.EnterBlock();
            Append("for(");
            sb.Append(ExpressionToString(bs.Declaration));
            sb.Append(";");
            sb.Append(ExpressionToString(bs.Condition));
            sb.Append(";");

            for (int i = 0; i < bs.Incrementors.Count; i++)
            {
                sb.Append(ExpressionToString(bs.Incrementors[i]));
                if (i < bs.Incrementors.Count - 2)
                {
                    sb.Append(",");
                }
            }
            sb.AppendLine(")");
            ConvertStatement(bs.Statement);
            Model.LeaveBlock();
        }

        void ConvertStatement(Metadata.DB_DoStatementSyntax bs)
        {
            AppendLine("do");
            ConvertStatement(bs.Statement);
            Append("while");
            sb.Append("(");
            sb.Append(ExpressionToString(bs.Condition));
            sb.AppendLine(");");
        }
        void ConvertStatement(Metadata.DB_WhileStatementSyntax bs)
        {
            Append("while");
            sb.Append("(");
            sb.Append(ExpressionToString(bs.Condition));
            sb.AppendLine(")");
            ConvertStatement(bs.Statement);
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
            AppendLine("try");
            ConvertStatement(ss.Block);

            for (int i = 0; i < ss.Catches.Count; i++)
            {
                AppendLine(string.Format("catch({0} {1})",GetCppTypeName(Model.GetType( ss.Catches[i].Type)),ss.Catches[i].Identifier));

                ConvertStatement(ss.Catches[i].Block);
            }
        }

        void ConvertStatement(Metadata.DB_ThrowStatementSyntax ss)
        {
            Append("throw ");
            sb.Append(ExpressionToString(ss.Expression));
            sb.AppendLine(";");
        }

        public string ExpressionToString(Metadata.Expression.Exp es)
        {
            if (es is Metadata.Expression.ConstExp)
            {
                return ExpressionToString((Metadata.Expression.ConstExp)es);
            }
            else if (es is Metadata.Expression.FieldExp)
            {
                return ExpressionToString((Metadata.Expression.FieldExp)es);
            }
            else if (es is Metadata.Expression.MethodExp)
            {
                return ExpressionToString((Metadata.Expression.MethodExp)es);
            }
            else if (es is Metadata.Expression.ThisExp)
            {
                return ExpressionToString((Metadata.Expression.ThisExp)es);
            }
            else if (es is Metadata.Expression.ObjectCreateExp)
            {
                return ExpressionToString((Metadata.Expression.ObjectCreateExp)es);
            }
            else if (es is Metadata.Expression.IndifierExp)
            {
                return ExpressionToString((Metadata.Expression.IndifierExp)es);
            }
            else if (es is Metadata.Expression.BaseExp)
            {
                return ExpressionToString((Metadata.Expression.BaseExp)es);
            }
            else if(es is Metadata.Expression.AssignmentExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.AssignmentExpressionSyntax)es);
            }
            else if (es is Metadata.Expression.BinaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.BinaryExpressionSyntax)es);
            }
            else if (es is Metadata.Expression.PrefixUnaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.PrefixUnaryExpressionSyntax)es);
            }
            else if (es is Metadata.Expression.PostfixUnaryExpressionSyntax)
            {
                return ExpressionToString((Metadata.Expression.PostfixUnaryExpressionSyntax)es);
            }
            else if(es is Metadata.Expression.ParenthesizedExpressionSyntax)
            {
                return ExpressionToString(((Metadata.Expression.ParenthesizedExpressionSyntax)es));
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

        string GetExpConversion(Metadata.DB_Type left_type,Metadata.DB_Type right_type,Metadata.Expression.Exp right)
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
                    stringBuilder.Append(string.Format("{0}::{1}({2})", GetCppTypeName(right_type), operatorMethod.name, ExpressionToString(right)));
                }
                else
                {
                    stringBuilder.Append(ExpressionToString(right));
                    Console.Error.WriteLine("类型不能转换 " + stringBuilder.ToString());
                }
            }
            else
                stringBuilder.Append(ExpressionToString(right));

            return stringBuilder.ToString();
        }


        string ExpressionToString(Metadata.Expression.MethodExp es)
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

            if (es.Caller is Metadata.Expression.IndifierExp)
            {
                stringBuilder.Append(ExpressionToString(es.Caller));

                Metadata.Expression.IndifierExp ie = es.Caller as Metadata.Expression.IndifierExp;
                Metadata.Model.IndifierInfo ii = Model.GetIndifierInfo(ie.Name);
                caller_type = ii.type;
                if (ii.is_namespace || ii.is_type)
                {
                    stringBuilder.Append("::");
                }
                else if (caller_type.is_class)
                {
                    stringBuilder.Append("->");
                }
                else
                {
                    stringBuilder.Append(".");
                }
            }
            else if (es.Caller is Metadata.Expression.BaseExp)
            {
                stringBuilder.Append(ExpressionToString(es.Caller));
                stringBuilder.Append("::");
                caller_type = Model.GetExpType(es.Caller);
            }
            else if (es.Caller is Metadata.Expression.ThisExp)
            {
                caller_type = Model.GetExpType(es.Caller);
                if(caller_type.FindMethod(es.Name, args, Model).is_static)
                {
                    //stringBuilder.Append("::");
                }
                else
                {
                    stringBuilder.Append("->");

                }
            }
            else
            {
                stringBuilder.Append(ExpressionToString(es.Caller));
                caller_type = Model.GetExpType(es.Caller);
                if (caller_type.is_class)
                {
                    stringBuilder.Append("->");
                }
                else
                {
                    stringBuilder.Append(".");
                }
            }

           

            Metadata.DB_Member method = caller_type.FindMethod(es.Name, args, Model);

            stringBuilder.Append(es.Name);
            stringBuilder.Append("(");
            if (es.Args != null)
            {
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

                    
                    //形式参数类型
                    Metadata.DB_Type me_argType = Model.GetType(method.method_args[i].type);

                    string ArgString = GetExpConversion(me_argType, arg_type, es.Args[i]);


                    if (me_argType.is_class && arg_type.is_class &&  arg_type.GetRefType() != me_argType.GetRefType())
                    {
                        stringBuilder.Append(string.Format("Ref<{1}>({0}.Get())", ArgString, GetCppTypeName(me_argType)));
                    }
                    else
                    {
                        stringBuilder.Append(ArgString);
                    }

                    if (i < es.Args.Count - 2)
                        stringBuilder.Append(",");
                }
            }
            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.ConstExp es)
        {
            if(es.value == "null")
            {
                return "nullptr";
            }
            if (!string.IsNullOrEmpty(es.value))
            {
                if (es.value.Length > 0 && es.value[0] == '"')
                    return "Ref<System::String>(new System::String(_T(" + es.value + ")))";
            }
            return es.value;
        }
        string ExpressionToString(Metadata.Expression.FieldExp es)
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

            if (es.Caller == null)   //本地变量或者类变量，或者全局类
            {
                return es.Name;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(ExpressionToString(es.Caller));

                Metadata.DB_Type caller_type = null;

                if (es.Caller is Metadata.Expression.IndifierExp)
                {
                    Metadata.Expression.IndifierExp ie = es.Caller as Metadata.Expression.IndifierExp;
                    Metadata.Model.IndifierInfo ii = Model.GetIndifierInfo(ie.Name);
                    if (ii.is_namespace || ii.is_type)
                    {
                        stringBuilder.Append("::");
                        caller_type = null;
                    }
                    else
                    {
                        caller_type = ii.type;
                    }
                }
                else
                {
                    caller_type = Model.GetExpType(es.Caller);
                }

                if (caller_type != null)
                {
                    if (caller_type.is_class)
                    {
                        //Metadata.DB_Member member = caller_type.members[es.Name];
                        //if (member.is_static)
                        stringBuilder.Append("->");
                        //else if (member.member_type == (int)Metadata.MemberTypes.Method)
                        //{

                        //}
                    }
                    else
                    {
                        stringBuilder.Append(".");
                    }
                }

                stringBuilder.Append(es.Name);
                return stringBuilder.ToString();
            }
        }
        string ExpressionToString(Metadata.Expression.ObjectCreateExp es)
        {
            StringBuilder ExpSB = new StringBuilder();
            ExpSB.Append("new ");
            ExpSB.Append(GetCppTypeName(Model.GetType(es.Type)));
            ExpSB.Append("(");
            if (es.Args != null)
            {
                for (int i = 0; i < es.Args.Count; i++)
                {
                    ExpSB.Append(ExpressionToString(es.Args[i]));
                    if (i < es.Args.Count - 2)
                        ExpSB.Append(",");
                }
            }
            ExpSB.Append(")");
            return ExpSB.ToString();
        }
        string ExpressionToString(Metadata.Expression.BaseExp es)
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

        string ExpressionToString(Metadata.VariableDeclarationSyntax es)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Metadata.DB_Type type = Model.GetType(es.Type);
            if (type.is_class)
                Append("Ref<" + GetCppTypeName(type) + "> ");
            else
                Append(GetCppTypeName(type) + " ");

            //stringBuilder.Append(GetCppTypeName(Model.GetType(es.Type)));
            //stringBuilder.Append(" ");
            for (int i = 0; i < es.Variables.Count; i++)
            {
                Model.AddLocal(es.Variables[i].Identifier, Model.GetType(es.Type));
                {
                    Metadata.VariableDeclaratorSyntax esVar = es.Variables[i];
                    stringBuilder.Append(esVar.Identifier);
                    if (esVar.Initializer != null)
                    {
                        Metadata.DB_Type right_type = Model.GetExpType(esVar.Initializer);

                        stringBuilder.Append(" = ");
                        string ArgString = GetExpConversion(type, right_type, esVar.Initializer);
                        stringBuilder.Append(ArgString);
                    }
                }
                if (i < es.Variables.Count - 1)
                    stringBuilder.Append(",");
            }
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.IndifierExp es)
        {
            Metadata.Model.IndifierInfo info = Model.GetIndifierInfo(es.Name);
            if (info.is_type)
            {
                ITypeConverter tc = Converter.GetTypeConverter(info.type);
                string content;
                //if(tc.ConvertIdentifierExp(Converter, info.type,  es, out content))
                //{
                //    return content;
                //}
                if (tc!= null && tc.GetCppTypeName(out content))
                {
                    return content;
                }

                return GetCppTypeName(info.type);
            }
            return es.Name;
        }

        string ExpressionToString(Metadata.Expression.ThisExp exp)
        {
            return "this";
        }
        string ExpressionToString(Metadata.Expression.AssignmentExpressionSyntax exp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            if(exp.OperatorToken=="=")
            {
                Metadata.DB_Type left_type = Model.GetExpType(exp.Left);
                Metadata.DB_Type right_type = Model.GetExpType(exp.Right);

                stringBuilder.Append(ExpressionToString(exp.Left));
                stringBuilder.Append(" = ");

                string ArgString = GetExpConversion(left_type, right_type, exp.Right);
                stringBuilder.Append(ArgString);
            }
            else
            {
                string token = exp.OperatorToken.Replace("=", "");
                Metadata.Expression.BinaryExpressionSyntax binaryExpressionSyntax = new Metadata.Expression.BinaryExpressionSyntax();
                binaryExpressionSyntax.Left = exp.Left;
                binaryExpressionSyntax.Right = exp.Right;
                binaryExpressionSyntax.OperatorToken = token;
                stringBuilder.Append(ExpressionToString(exp.Left));
                stringBuilder.Append(" = ");
                stringBuilder.Append(ExpressionToString(binaryExpressionSyntax));

                //Console.Error.WriteLine("无法解析的操作符 " + exp.OperatorToken);
            }

            Console.WriteLine(stringBuilder.ToString());

            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.BinaryExpressionSyntax exp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            Metadata.DB_Type left_type = Model.GetExpType(exp.Left);
            Metadata.DB_Type right_type = Model.GetExpType(exp.Right);
            List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
            argTypes.Add(left_type);
            argTypes.Add(right_type);
            Metadata.DB_Member method = left_type.FindMethod(exp.OperatorToken, argTypes, Model);
            if(method != null)
            {
                stringBuilder.Append(string.Format("{0}::{1}(", GetCppTypeName(left_type), GetOperatorFuncName(exp.OperatorToken)));
            }
            else
            {
                method = right_type.FindMethod(exp.OperatorToken, argTypes, Model);
                if(method == null)
                {
                    Console.Error.WriteLine("操作符没有重载的方法 " + exp.ToString());
                    return stringBuilder.ToString();
                }
                stringBuilder.Append(string.Format("{0}::{1}(", GetCppTypeName(left_type), GetOperatorFuncName(exp.OperatorToken)));
            }
            
            if(left_type.is_class && method.method_args[0].type != left_type.GetRefType())
            {
                stringBuilder.Append(string.Format("Ref<{0}>({1})", GetCppTypeName(Model.GetType(method.method_args[0].type)), ExpressionToString(exp.Left)));
            }
            else
            {
                stringBuilder.Append(ExpressionToString(exp.Left));
            }
            stringBuilder.Append(",");
            if (right_type.is_class && method.method_args[1].type != right_type.GetRefType())
            {
                stringBuilder.Append(string.Format("Ref<{0}>({1})", GetCppTypeName(Model.GetType(method.method_args[1].type)), ExpressionToString(exp.Right)));
            }
            else
            {
                stringBuilder.Append(ExpressionToString(exp.Right));
            }
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.PostfixUnaryExpressionSyntax exp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(ExpressionToString(exp.Operand));
            stringBuilder.Append(exp.OperatorToken);
            return stringBuilder.ToString();
        }
        string ExpressionToString(Metadata.Expression.PrefixUnaryExpressionSyntax exp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(exp.OperatorToken);
            stringBuilder.Append(ExpressionToString(exp.Operand));
            return stringBuilder.ToString();
        }

        string ExpressionToString(Metadata.Expression.ParenthesizedExpressionSyntax exp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            stringBuilder.Append(ExpressionToString(exp.exp));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}
