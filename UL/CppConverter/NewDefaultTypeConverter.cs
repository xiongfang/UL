using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metadata;
using Metadata.Expression;

namespace CppConverter
{
    //class CppVisitor : Metadata.ITypeVisitor, Metadata.IMemberVisitor, Metadata.IMethodVisitor
    //{
    //    int depth;
    //    StringBuilder sb = new StringBuilder();
    //    IConverter Converter;
    //    bool make_header;
    //    Metadata.Model Model;

    //    void AppendDepth()
    //    {
    //        for (int i = 0; i < depth; i++)
    //        {
    //            sb.Append("\t");
    //        }
    //    }

    //    void AppendLine(string msg)
    //    {
    //        AppendDepth();
    //        sb.AppendLine(msg);
    //    }

    //    void Append(string msg)
    //    {
    //        AppendDepth();
    //        sb.Append(msg);
    //    }
        

    //    public string GetTypeHeader(Metadata.DB_Type type)
    //    {
    //        TypeConfig tc = Converter.GetTypeConfig(type);
    //        if (tc != null)
    //        {
    //            if (!string.IsNullOrEmpty(tc.header_path))
    //                return tc.header_path;
    //        }


    //        string[] ns_list = type._namespace.Split('.');
    //        string path = System.IO.Path.Combine(ns_list);
    //        if (type.is_generic_type_definition)
    //        {
    //            return System.IO.Path.Combine(path, "t_" + type.name + ".h");
    //        }

    //        return System.IO.Path.Combine(path, type.name + ".h");
    //    }

    //    public string GetTypeCppFileName(Metadata.DB_Type type)
    //    {
    //        string[] ns_list = type._namespace.Split('.');
    //        string path = System.IO.Path.Combine(ns_list);
    //        if (type.is_generic_type_definition)
    //        {
    //            return System.IO.Path.Combine(path, "t_" + type.name + ".cpp");
    //        }

    //        return System.IO.Path.Combine(path, type.name + ".cpp");
    //    }

    //    string GetCppTypeName(Metadata.DB_Type type)
    //    {
    //        ITypeConverter tc = Converter.GetTypeConverter(type);
    //        if (tc != null)
    //        {
    //            string name;
    //            if (tc.GetCppTypeName(out name))
    //            {
    //                return name;
    //            }
    //        }
    //        if (type.is_generic_paramter)
    //            return type.name;
    //        if (type.is_generic_type)
    //        {

    //            StringBuilder sb = new StringBuilder();
    //            sb.Append(type._namespace.Replace(".", "::"));
    //            sb.Append("::");
    //            sb.Append(type.name);
    //            sb.Append("<");
    //            for (int i = 0; i < type.generic_parameters.Count; i++)
    //            {
    //                sb.Append(GetCppTypeName(Model.GetType(type.generic_parameters[i])));
    //                if (i < type.generic_parameters.Count - 1)
    //                    sb.Append(",");
    //            }
    //            sb.Append(">");
    //            return sb.ToString();
    //        }
    //        if (type.is_interface)
    //            return type._namespace.Replace(".", "::") + "::" + type.name;
    //        if (type.is_class)
    //            return type._namespace.Replace(".", "::") + "::" + type.name;
    //        if (type.is_value_type)
    //            return type._namespace.Replace(".", "::") + "::" + type.name;
    //        if (type.is_enum)
    //            return type._namespace.Replace(".", "::") + "::" + type.name;

    //        return type.static_full_name;
    //    }

    //    string GetCppTypeWrapName(Metadata.DB_Type type)
    //    {
    //        if (type.GetRefType().IsVoid)
    //            return "void";
    //        if (type.is_value_type)
    //        {
    //            return GetCppTypeName(type);
    //        }
    //        else
    //        {
    //            return string.Format("Ref<{0}>", GetCppTypeName(type));
    //        }

    //    }

    //    string GetModifierString(int modifier)
    //    {
    //        switch ((Metadata.Modifier)modifier)
    //        {
    //            case Metadata.Modifier.Private:
    //                return "private";
    //            case Metadata.Modifier.Protected:
    //                return "protected";
    //            case Metadata.Modifier.Public:
    //                return "public";
    //        }

    //        return "";
    //    }

    //    string GetOperatorFuncName(string token, int arg_count = 1)
    //    {
    //        switch (token)
    //        {
    //            case "+":
    //                return arg_count == 2 ? "op_Addition" : "op_UnaryPlus";
    //            case "-":
    //                return arg_count == 2 ? "op_Substraction" : "op_UnaryNegation";
    //            case "*":
    //                return "op_Multiply";
    //            case "/":
    //                return "op_Division";
    //            case "%":
    //                return "op_Modulus";
    //            case "&":
    //                return "op_BitwiseAnd";
    //            case "|":
    //                return "op_BitwiseOr";
    //            case "~":
    //                return "op_OnesComplement";
    //            case "<<":
    //                return "op_LeftShift";
    //            case ">>":
    //                return "op_RightShift";
    //            case "==":
    //                return "op_Equality";
    //            case "!=":
    //                return "op_Inequality";
    //            case ">":
    //                return "op_GreaterThen";
    //            case "<":
    //                return "op_LessThen";
    //            case "++":
    //                return "op_Increment";
    //            case "--":
    //                return "op_Decrement";
    //            case "!":
    //                return "op_LogicNot";
    //            default:
    //                Console.Error.WriteLine("未知的操作符 " + token);
    //                return token;
    //        }
    //    }

    //    void ConvertMemberHeader(Metadata.DB_Member member)
    //    {
    //        Metadata.DB_Type member_type = Model.GetType(member.typeName);
    //        if (member.member_type == (int)Metadata.MemberTypes.Field)
    //        {
    //            AppendLine(GetModifierString(member.modifier) + ":");
    //            //属性
    //            //AppendLine(ConvertCppAttribute(member.attributes));
    //            if (member.is_static)
    //                Append("static ");
    //            else
    //                Append("");


    //            //if (member_type.is_class)
    //            //    AppendLine(string.Format("{0}* {1};", GetCppTypeWrapName(Model.GetType(member.field_type)), member.name));
    //            //else
    //            AppendLine(string.Format("{0} {1};", GetCppTypeWrapName(Model.GetType(member.type)), member.name));
    //        }
    //        else if (member.member_type == (int)Metadata.MemberTypes.Method)
    //        {
    //            Model.EnterMethod(member);

    //            AppendLine(GetModifierString(member.modifier) + ":");


    //            //属性
    //            //AppendLine(ConvertCppAttribute(member.attributes));

    //            if (member.is_static)
    //                Append("static ");
    //            else
    //                Append("");

    //            if (member.method_abstract)
    //            {
    //                sb.Append("abstract ");
    //            }
    //            if (member.method_virtual)
    //            {
    //                sb.Append("virtual ");
    //            }

    //            if (!member.method_is_constructor)
    //            {
    //                string method_name = member.name;
    //                if (member.method_is_operator)
    //                {
    //                    method_name = GetOperatorFuncName(member.name, member.method_args.Length);
    //                }
    //                sb.Append(string.Format("{1} {2}", "", member.type.IsVoid ? "void" : GetCppTypeWrapName(Model.GetType(member.type)), method_name));
    //            }
    //            else
    //            {
    //                sb.Append(string.Format("{0}", member.name));
    //            }

    //            sb.Append("(");
    //            if (member.method_args != null)
    //            {
    //                for (int i = 0; i < member.method_args.Length; i++)
    //                {
    //                    Metadata.DB_Type arg_Type = Model.GetType(member.method_args[i].type);
    //                    //string typeName = GetCppTypeName(arg_Type);
    //                    sb.Append(string.Format("{0} {1} {2}", GetCppTypeWrapName(arg_Type), (member.method_args[i].is_ref || member.method_args[i].is_out) ? "&" : "", member.method_args[i].name));
    //                    if (i < member.method_args.Length - 1)
    //                        sb.Append(",");
    //                }
    //            }

    //            Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);

    //            if (declare_type.is_generic_type_definition)
    //            {
    //                sb.AppendLine(")");
    //                ConvertStatement(member.method_body);
    //            }
    //            else
    //            {
    //                sb.AppendLine(");");
    //            }

    //            Model.LeaveMethod();
    //        }
    //    }

    //    void ConvertMemberCpp(Metadata.DB_Member member)
    //    {
    //        Metadata.DB_Type member_type = Model.GetType(member.typeName);
    //        if (member.member_type == (int)Metadata.MemberTypes.Field)
    //        {
    //            if (member.is_static)
    //            {
    //                if (member_type.is_class)
    //                    AppendLine("Ref<" + GetCppTypeName(Model.GetType(member.type)) + "> " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name + ";");
    //                else if (member_type.is_value_type)
    //                {
    //                    Append(GetCppTypeName(Model.GetType(member.type)) + " " + GetCppTypeName(Model.GetType(member.declaring_type)) + "::" + member.name);
    //                    if (member.field_initializer != null)
    //                    {
    //                        sb.Append("=");
    //                        sb.Append(ExpressionToString(member.field_initializer));
    //                    }
    //                    sb.AppendLine(";");
    //                }


    //            }
    //        }
    //        else if (member.member_type == (int)Metadata.MemberTypes.Method)
    //        {
    //            Model.EnterMethod(member);
    //            Metadata.DB_Type declare_type = Model.GetType(member.declaring_type);
    //            if (!declare_type.is_generic_type_definition && member.method_body != null)
    //            {
    //                if (!member.method_is_constructor)
    //                {
    //                    string method_name = member.name;
    //                    if (member.method_is_operator)
    //                    {
    //                        method_name = GetOperatorFuncName(member.name, member.method_args.Length);
    //                    }
    //                    sb.Append(string.Format("{0} {1}::{2}", member.type.IsVoid ? "void" : GetCppTypeWrapName(Model.GetType(member.type)), GetCppTypeName(Model.GetType(member.declaring_type)), method_name));
    //                }
    //                else
    //                    sb.Append(string.Format("{1}::{2}", "", GetCppTypeName(Model.GetType(member.declaring_type)), member.name));
    //                sb.Append("(");
    //                if (member.method_args != null)
    //                {
    //                    for (int i = 0; i < member.method_args.Length; i++)
    //                    {
    //                        sb.Append(string.Format("{0} {1} {2}", GetCppTypeWrapName(Model.GetType(member.method_args[i].type)), (member.method_args[i].is_ref || member.method_args[i].is_out) ? "&" : "", member.method_args[i].name));
    //                        if (i < member.method_args.Length - 1)
    //                            sb.Append(",");
    //                    }
    //                }
    //                sb.AppendLine(")");

    //                ConvertStatement(member.method_body);
    //            }
    //            Model.LeaveMethod();
    //        }
    //    }

    //    void ConvertType(DB_Type type)
    //    {
    //        string outputDir = Converter.GetProject().export_dir;
    //        ITypeConverter tc = Converter.GetTypeConverter(type);
    //        if (tc != null)
    //        {
    //            sb.Clear();
    //            string content;
    //            if (tc.ConvertTypeHeader(Converter, type, out content))
    //            {
    //                sb.Append(content);

    //                WriteFile(System.IO.Path.Combine(outputDir, GetTypeHeader(type)), type, sb.ToString());
    //            }

    //            sb.Clear();
    //            if (tc.ConvertTypeCpp(Converter, type, out content))
    //            {
    //                sb.Append(content);
    //                WriteFile(System.IO.Path.Combine(outputDir, GetTypeCppFileName(type)), type, sb.ToString());
    //            }
    //        }
    //        else
    //        {
    //            make_header = true;
    //            Model.Accept(type, this);
    //            make_header = false;
    //            Model.Accept(type, this);
    //        }
    //    }

    //    void WriteFile(string path, Metadata.DB_Type type, string content)
    //    {
    //        string dir = System.IO.Path.GetDirectoryName(path);
    //        if (!System.IO.Directory.Exists(dir))
    //        {
    //            System.IO.Directory.CreateDirectory(dir);
    //        }

    //        if (System.IO.File.Exists(path))
    //        {
    //            if (System.IO.File.ReadAllText(path, Encoding.UTF8) == content)
    //                return;
    //        }

    //        System.IO.File.WriteAllText(path, content, Encoding.UTF8);

    //    }

    //    public CppVisitor(Metadata.Model model, bool header) { this.Model = model; this.make_header = header; }
    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, AssignmentExpressionSyntax exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BaseExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BinaryExpressionSyntax exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ConstExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, FieldExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, MethodExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ObjectCreateExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ParenthesizedExpressionSyntax exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PostfixUnaryExpressionSyntax exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PrefixUnaryExpressionSyntax exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThisExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThrowExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, IndifierExp exp, Exp outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_DoStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_ExpressionStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_ForStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_LocalDeclarationStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer)
    //    {
    //        throw new NotImplementedException();
    //    }



    //    public IMemberVisitor GetMemberVisitor()
    //    {
    //        return this;
    //    }

    //    public void VisitMember(DB_Type type, DB_Member member)
    //    {
    //        Metadata.DB_Type member_type = Model.GetType(member.typeName);
    //        if (member.member_type == (int)Metadata.MemberTypes.Field)
    //        {
    //            AppendLine(GetModifierString(member.modifier) + ":");
    //            //属性
    //            //AppendLine(ConvertCppAttribute(member.attributes));
    //            if (member.is_static)
    //                Append("static ");
    //            else
    //                Append("");


    //            //if (member_type.is_class)
    //            //    AppendLine(string.Format("{0}* {1};", GetCppTypeWrapName(Model.GetType(member.field_type)), member.name));
    //            //else
    //            AppendLine(string.Format("{0} {1};", GetCppTypeWrapName(Model.GetType(member.type)), member.name));
    //        }
    //    }

    //    public IMethodVisitor GetMethodVisitor()
    //    {
    //        if (make_header)
    //            return null;
    //        return this;
    //    }

    //    public void VisitTypeStart(DB_Type type)
    //    {
    //        if (make_header)
    //        {
    //            sb.Clear();
    //            sb.AppendLine("#pragma once");

    //            //包含头文件
    //            HashSet<string> depTypes = Converter.GetTypeDependences(type);

    //            HashSet<string> NoDeclareTypes = Converter.GetTypeDependencesNoDeclareType(type);
    //            foreach (var t in depTypes)
    //            {
    //                Metadata.DB_Type depType = Model.GetType(t);
    //                if (!depType.is_generic_paramter && t != type.static_full_name)
    //                {
    //                    if (NoDeclareTypes.Contains(t))
    //                    {
    //                        sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
    //                    }
    //                    else
    //                    {
    //                        //前向声明
    //                        List<string> nsList = new List<string>();
    //                        nsList.AddRange(depType._namespace.Split('.'));
    //                        for (int i = 0; i < nsList.Count; i++)
    //                        {
    //                            AppendLine("namespace " + nsList[i] + "{");
    //                            depth++;
    //                        }
    //                        //sb.AppendLine("namespace " + depType._namespace);
    //                        //sb.AppendLine("{");
    //                        AppendDepth();
    //                        if (depType.is_generic_type_definition)
    //                        {
    //                            sb.Append("template");
    //                            sb.Append("<");
    //                            for (int i = 0; i < depType.generic_parameter_definitions.Count; i++)
    //                            {
    //                                sb.Append(depType.generic_parameter_definitions[i].type_name);
    //                                if (i < depType.generic_parameter_definitions.Count - 1)
    //                                    sb.Append(",");
    //                            }
    //                            sb.AppendLine(">");
    //                            if (depType.is_value_type)
    //                                sb.AppendLine("struct " + depType.name + ";");
    //                            else
    //                                sb.AppendLine("class " + depType.name + ";");
    //                        }
    //                        else
    //                        {
    //                            if (depType.is_value_type)
    //                            {
    //                                if (depType.is_enum)
    //                                {
    //                                    sb.AppendLine("struct " + depType.name + ";");
    //                                }
    //                                else
    //                                {
    //                                    sb.AppendLine("struct " + depType.name + ";");
    //                                }
    //                            }
    //                            else
    //                                sb.AppendLine("class " + depType.name + ";");
    //                        }

    //                        for (int i = 0; i < nsList.Count; i++)
    //                        {
    //                            depth--;
    //                            AppendLine("}");
    //                        }
    //                        //sb.AppendLine("}");
    //                    }
    //                }
    //            }


    //            //if (HasCppAttribute(type.attributes))
    //            //{
    //            //    //包含虚幻生成的头文件
    //            //    AppendLine(string.Format("#include \"{0}.generated.h\"", type.name));

    //            //    //属性
    //            //    AppendLine(ConvertCppAttribute(type.attributes));
    //            //}


    //            //sb.AppendLine(string.Format("namespace {0}{{", type._namespace));
    //            {
    //                List<string> nsList = new List<string>();
    //                nsList.AddRange(type._namespace.Split('.'));
    //                for (int i = 0; i < nsList.Count; i++)
    //                {
    //                    AppendLine("namespace " + nsList[i] + "{");
    //                    depth++;
    //                }

    //                depth++;
    //                //if (type.is_enum)
    //                //{
    //                //    Append(string.Format("enum {0}", type.name));
    //                //}
    //                //else
    //                //{
    //                if (type.is_generic_type_definition)
    //                {
    //                    Append("template<");
    //                    for (int i = 0; i < type.generic_parameter_definitions.Count; i++)
    //                    {
    //                        sb.Append("class " + type.generic_parameter_definitions[i].type_name);
    //                        if (i < type.generic_parameter_definitions.Count - 1)
    //                            sb.Append(",");
    //                    }
    //                    sb.AppendLine(">");
    //                }

    //                if (type.is_value_type)
    //                {
    //                    Append(string.Format("struct {0}", type.name));
    //                }
    //                else
    //                {
    //                    Append(string.Format("class {0}", type.name));
    //                }
    //                if (!type.base_type.IsVoid /*&& !type.is_value_type*/ || type.interfaces.Count > 0)
    //                {
    //                    sb.Append(":");
    //                    if (!type.base_type.IsVoid /*&& !type.is_value_type*/)
    //                    {
    //                        sb.Append("public " + GetCppTypeName(Model.GetType(type.base_type)));
    //                        if (type.interfaces.Count > 0)
    //                            sb.Append(",");
    //                    }
    //                    for (int i = 0; i < type.interfaces.Count; i++)
    //                    {
    //                        sb.Append("public " + GetCppTypeName(Model.GetType(type.interfaces[i])));
    //                        if (i < type.interfaces.Count - 1)
    //                            sb.Append(",");
    //                    }
    //                    sb.AppendLine();
    //                }
    //                //}

    //                AppendLine("{");
                    
    //                depth++;


                    
    //            }
    //        }
    //        else
    //        {
    //            sb.Clear();
    //            Project cfg = Converter.GetProject();
    //            if (!type.is_enum && !type.is_generic_type_definition)
    //            {
    //                //Model.EnterNamespace(type._namespace);
    //                //Model.EnterType(type);

    //                if (!string.IsNullOrEmpty(cfg.precompile_header))
    //                {
    //                    sb.AppendLine(string.Format("#include \"{0}\"", cfg.precompile_header));
    //                }
    //                sb.AppendLine("#include \"" + GetTypeHeader(type) + "\"");
    //                //sb.AppendLine(string.Format("namespace {0}{{", type._namespace));

    //                //包含依赖的头文件
    //                HashSet<string> depTypes = Converter.GetMethodBodyDependences(type);
    //                HashSet<string> headDepTypes = Converter.GetTypeDependences(type);
    //                foreach (var t in headDepTypes)
    //                {
    //                    Metadata.DB_Type depType = Model.GetType(t);
    //                    if (!depType.is_generic_paramter && t != type.static_full_name)
    //                        sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
    //                }
    //                foreach (var t in depTypes)
    //                {
    //                    if (!headDepTypes.Contains(t))
    //                    {
    //                        Metadata.DB_Type depType = Model.GetType(t);
    //                        if (!depType.is_generic_paramter && t != type.static_full_name)
    //                            sb.AppendLine("#include \"" + GetTypeHeader(depType) + "\"");
    //                    }
    //                }


    //                foreach (var us in type.usingNamespace)
    //                {
    //                    sb.AppendLine("using namespace " + us.Replace(".", "::") + ";");
    //                }

    //                TypeConfig tc = Converter.GetTypeConfig(type);

    //                if (tc != null)
    //                {
    //                    if (!string.IsNullOrEmpty(tc.ext_cpp))
    //                    {
    //                        AppendLine("#include \"" + tc.ext_cpp + "\"");
    //                    }
    //                }

    //                //foreach (var m in type.members.Values)
    //                //{
    //                //    ConvertMemberCpp(m);
    //                //}
    //            }
    //        }
    //    }

    //    public void VisitTypeEnd(DB_Type type)
    //    {
    //        string outputDir = Converter.GetProject().export_dir;
    //        if (make_header)
    //        {
    //            List<string> nsList = new List<string>();
    //            nsList.AddRange(type._namespace.Split('.'));

    //            depth--;


    //            TypeConfig tc = Converter.GetTypeConfig(type);

    //            if (tc != null)
    //            {
    //                if (!string.IsNullOrEmpty(tc.ext_header))
    //                {
    //                    AppendLine("#include \"" + tc.ext_header + "\"");
    //                }
    //            }

    //            AppendLine("};");
    //            depth--;

    //            for (int i = 0; i < nsList.Count; i++)
    //            {
    //                depth--;
    //                AppendLine("}");
    //            }

    //            WriteFile(System.IO.Path.Combine(outputDir, GetTypeHeader(type)), type, sb.ToString());
    //        }
    //        else
    //        {
    //            if (!type.is_enum && !type.is_generic_type_definition)
    //                WriteFile(System.IO.Path.Combine(outputDir, GetTypeCppFileName(type)), type, sb.ToString());
    //        }

    //    }

    //    public void VisitMethodStart(DB_Type type, DB_Member member)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void VisitMethodEnd(DB_Type type, DB_Member member)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //class NewDefaultTypeConverter : IDefaultTypeConverter
    //{
    //    int depth;
    //    StringBuilder sb = new StringBuilder();
    //    IConverter Converter;
    //    bool make_header;

    //    public NewDefaultTypeConverter(IConverter cv) { this.Converter = cv; }

    //    Metadata.Model Model
    //    {
    //        get { return Converter.GetModel(); }
    //    }

    //    public int priority
    //    {
    //        get { return 0; }
    //    }



    //    public void VisitType(DB_Type type)
    //    {

    //    }
        





    //}
}
