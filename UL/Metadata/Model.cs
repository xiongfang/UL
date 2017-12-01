﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metadata.Expression;

namespace Metadata
{
    public interface IModelTypeFinder
    {
        //查找一个数据库类型
        DB_Type FindType(string full_name);
        //查找一个类型，如果是动态类型，构造一个
        DB_Type FindType(Expression.TypeSyntax refType);
        //Dictionary<string, Metadata.DB_Type> FindNamespace(string ns);
    }




    public partial class Model
    {
        public IModelTypeFinder Finder;

        //当前类的命名空间
        public HashSet<string> usingNamespace = new HashSet<string>();
        public string outNamespace;
        //当前处理的类型
        public Metadata.DB_Type currentType;
        Metadata.DB_Member currentMethod;
        //当前函数的本地变量和参数
        Stack<Dictionary<string, Metadata.DB_Type>> stackLocalVariables = new Stack<Dictionary<string, Metadata.DB_Type>>();


        public Model(IModelTypeFinder finder) { Finder = finder; }

        public void StartUsing(List<string> usingList)
        {
            foreach (var ns in usingList)
                usingNamespace.Add(ns);
        }

        public void ClearUsing()
        {
            usingNamespace.Clear();
        }

        public void EnterNamespace(string ns)
        {
            outNamespace = ns;
        }

        public void LeaveNamespace()
        {
            outNamespace = null;
        }

        public void EnterType(Metadata.DB_Type type)
        {
            if(string.IsNullOrEmpty(outNamespace))
            {
                Console.Error.WriteLine("未指定外部命名空间");
            }
            currentType = type;
        }
        public void LeaveType()
        {
            currentType = null;
        }

        public void EnterMethod(Metadata.DB_Member member)
        {
            currentMethod = member;
            EnterBlock();
            foreach(var arg in member.method_args)
            {
                AddLocal(arg.name, GetType(arg.type));
            }
        }
        public void LeaveMethod()
        {
            LeaveBlock();
            currentMethod = null;
        }

        public void EnterBlock()
        {
            stackLocalVariables.Push(new Dictionary<string, Metadata.DB_Type>());
        }

        public void LeaveBlock()
        {
            stackLocalVariables.Pop();
        }

        public void AddLocal(string name, Metadata.DB_Type type)
        {
            stackLocalVariables.Peek().Add(name, type);
        }


        public Metadata.DB_Type GetType(string full_name)
        {
            return Finder.FindType(full_name);
        }

        public Metadata.DB_Type GetType(Metadata.Expression.TypeSyntax typeRef)
        {
            return Finder.FindType(typeRef);
        }

        public DB_Type GetExpType(Expression.Exp exp, Expression.Exp outer=null)
        {
            if (exp is Metadata.Expression.ConstExp)
            {
                Metadata.Expression.ConstExp e = exp as Metadata.Expression.ConstExp;

                int int_v;
                if (int.TryParse(e.value, out int_v))
                {
                    return GetType("System.Int32");
                }

                long long_v;
                if (long.TryParse(e.value, out long_v))
                {
                    return GetType("System.Int64");
                }

                bool b_v;
                if (bool.TryParse(e.value, out b_v))
                {
                    return GetType("System.Boolean");
                }

                float single_v;
                if (float.TryParse(e.value, out single_v))
                {
                    return GetType("System.Single");
                }

                double double_v;
                if (double.TryParse(e.value, out double_v))
                {
                    return GetType("System.Double");
                }

                if (e.value == "null")
                {
                    return GetType("System.Object");
                }

                if(e.value.StartsWith("'"))
                {
                    return GetType("System.Char");
                }

                return GetType("System.String");
            }

            else if (exp is Metadata.Expression.FieldExp)
            {
                Metadata.Expression.FieldExp e = exp as Metadata.Expression.FieldExp;
                Metadata.DB_Type caller_type = GetExpType(e.Caller);

                if(outer is MethodExp)
                {
                    MethodExp methodExp = outer as MethodExp;
                    List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                    foreach (var t in methodExp.Args)
                    {
                        argTypes.Add(GetExpType(t));
                    }
                    Metadata.DB_Member member = caller_type.FindMethod(e.Name, argTypes, this);
                    return GetType(member.typeName);
                }
                else
                {
                    DB_Member field = caller_type.FindField(e.Name, this);
                    if (field == null)
                        field = caller_type.FindProperty(e.Name, this);
                    return GetType(field.typeName);
                }
            }

            else if (exp is Metadata.Expression.IndifierExp)
            {
                Metadata.Expression.IndifierExp e = exp as Metadata.Expression.IndifierExp;
                return GetIndifierInfo(e.Name).type;
            }

            else if (exp is Metadata.Expression.MethodExp)
            {
                Metadata.Expression.MethodExp me = exp as Metadata.Expression.MethodExp;
                //Metadata.Expression.FieldExp e = me.Caller as Metadata.Expression.FieldExp;
                Metadata.DB_Type caller_type = GetExpType(me.Expression,me);
                return caller_type;
            }

            else if (exp is Metadata.Expression.ObjectCreateExp)
            {
                Metadata.Expression.ObjectCreateExp e = exp as Metadata.Expression.ObjectCreateExp;
                return GetType(e.Type);
            }

            else if (exp is Metadata.Expression.BaseExp)
            {
                return GetType(currentType.base_type);
            }

            else if (exp is Metadata.Expression.ThisExp)
            {
                return currentType;
            }

            else if (exp is Metadata.Expression.BinaryExpressionSyntax)
            {
                Metadata.Expression.BinaryExpressionSyntax me = exp as Metadata.Expression.BinaryExpressionSyntax;
                Metadata.DB_Type caller_type = GetExpType(me.Left);
                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                argTypes.Add(GetExpType(me.Right));
                
                Metadata.DB_Member member = caller_type.FindMethod(me.OperatorToken, argTypes, this);
                return GetType(member.typeName);
            }

            else if (exp is Metadata.Expression.PostfixUnaryExpressionSyntax)
            {
                Metadata.Expression.PostfixUnaryExpressionSyntax me = exp as Metadata.Expression.PostfixUnaryExpressionSyntax;
                Metadata.DB_Type caller_type = GetExpType(me.Operand);
                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                argTypes.Add(caller_type);

                Metadata.DB_Member member = caller_type.FindMethod(me.OperatorToken, argTypes, this);
                return GetType(member.typeName);
            }

            else if (exp is Metadata.Expression.PrefixUnaryExpressionSyntax)
            {
                Metadata.Expression.PrefixUnaryExpressionSyntax me = exp as Metadata.Expression.PrefixUnaryExpressionSyntax;
                Metadata.DB_Type caller_type = GetExpType(me.Operand);
                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                argTypes.Add(caller_type);

                Metadata.DB_Member member = caller_type.FindMethod(me.OperatorToken, argTypes, this);
                return GetType(member.typeName);
            }

            else if (exp is Metadata.Expression.ParenthesizedExpressionSyntax)
            {
                Metadata.Expression.ParenthesizedExpressionSyntax pes = exp as Metadata.Expression.ParenthesizedExpressionSyntax;
                return GetExpType(pes.exp);
            }
            else
            {
                Console.Error.WriteLine("无法确定表达式类型 " + exp.JsonType);
            }
            return null;
        }

        public class IndifierInfo
        {
            public bool is_type;
            public bool is_var;
            public bool is_namespace;
            public bool is_member;
            public bool is_class_type_parameter;
            public bool is_method_type_parameter;
            public Metadata.DB_Type type;
        }



        public DB_Type FindTypeInNamespace(string name,string ns)
        {
            do
            {
                DB_Type type = Finder.FindType(ns + "." + name);
                if (type != null)
                    return type;
                if (ns.Contains("."))
                {
                    ns = ns.Substring(0, ns.LastIndexOf('.'));
                }
                else
                    break;
            }
            while (ns != null);

            return null;
        }

        public IndifierInfo GetIndifierInfo(string name,string name_space="")
        {
            IndifierInfo info = new IndifierInfo();

            //在指定的命名空间查找标示符
            if(!string.IsNullOrEmpty(name_space))
            {
                DB_Type type = Finder.FindType(name_space + "." + name);
                if (type != null)
                {
                    info.is_type = true;
                    info.type = type;
                    return info;
                }
                return null;
            }

            //查找本地变量
            foreach (var v in stackLocalVariables)
            {
                if (v.ContainsKey(name))
                {
                    info.is_var = true;
                    info.type = v[name];
                    return info;
                }

            }

            //泛型参数
            if (currentMethod != null)
            {
                foreach (var gd in currentMethod.method_generic_parameter_definitions)
                {
                    if (gd.type_name == name)
                    {
                        info.is_method_type_parameter = true;
                        info.type = Metadata.DB_Type.MakeGenericParameterType(currentType, gd);
                        return info;
                    }
                }
            }

            //查找成员变量
            if (currentType != null)
            {
                if (currentType.FindField(name,this)!=null)
                {
                    info.is_member = true;
                    info.type = GetType(currentType.FindField(name, this).typeName);
                    return info;
                }
                if (currentType.FindProperty(name, this) != null)
                {
                    info.is_member = true;
                    info.type = GetType(currentType.FindProperty(name, this).typeName);
                    return info;
                }
                //查找泛型
                if (currentType.is_generic_type_definition)
                {
                    foreach (var gd in currentType.generic_parameter_definitions)
                    {
                        if (gd.type_name == name)
                        {
                            info.is_class_type_parameter = true;
                            info.type = Metadata.DB_Type.MakeGenericParameterType(currentType, gd);
                            return info;
                        }
                    }
                }
                //当前命名空间查找
                foreach (var nsName in currentType.usingNamespace)
                {
                    DB_Type type = FindTypeInNamespace(name, nsName);
                    if (type != null)
                    {
                        info.is_type = true;
                        info.type = type;
                        return info;
                    }
                }
            }

            //当前命名空间查找
            if (!string.IsNullOrEmpty(outNamespace))
            {
                DB_Type type = FindTypeInNamespace(name, outNamespace);
                if (type != null)
                {
                    info.is_type = true;
                    info.type = type;
                    return info;
                }
            }
            foreach (var nsName in usingNamespace)
            {
                DB_Type type = FindTypeInNamespace(name, nsName);
                if (type != null)
                {
                    info.is_type = true;
                    info.type = type;
                    return info;
                }
            }


            return null;
        }
    }

    public class MyCppHeaderTypeFinder : ITypeVisitor,IMemberVisitor
    {
        Model model;
        public MyCppHeaderTypeFinder(Model model)
        {
            this.model = model;
        }
        public HashSet<Expression.TypeSyntax> result = new HashSet<Expression.TypeSyntax>();
        public void VisitType(DB_Type type)
        {
            if (!type.base_type.IsVoid)
                result.Add(type.base_type);
            foreach(var i in type.interfaces)
            {
                result.Add(i);
            }
        }
        public IMemberVisitor GetMemberVisitor() { return this; }
        public void VisitMember(DB_Type type, DB_Member m)
        {
            if (m.member_type == (int)Metadata.MemberTypes.Field)
            {
                result.Add(m.type);
            }
            else if (m.member_type == (int)Metadata.MemberTypes.Method)
            {
                if (!m.type.IsVoid)
                    result.Add(m.type);
                foreach (var a in m.method_args)
                {
                    result.Add(a.type);
                }
            }
            else if (m.member_type == (int)Metadata.MemberTypes.Property)
            {
                result.Add(m.type);
            }
        }

        public IMethodVisitor GetMethodVisitor() { return null; }
    }

    public class MyCppMethodBodyTypeFinder : ITypeVisitor, IMemberVisitor,IMethodVisitor
    {
        Model model;
        public MyCppMethodBodyTypeFinder(Model model)
        {
            this.model = model;
        }
        public HashSet<Expression.TypeSyntax> typeRef = new HashSet<Expression.TypeSyntax>();
        public void VisitType(DB_Type type)
        {

        }
        public void VisitMember(DB_Type type, DB_Member member)
        {

        }
        public IMemberVisitor GetMemberVisitor() { return this; }
        public IMethodVisitor GetMethodVisitor() { return this; }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer)
        {
           // throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_DoStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ExpressionStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ForStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
            typeRef.Add(statement.Declaration.Type);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_LocalDeclarationStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, AssignmentExpressionSyntax exp, Exp outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BaseExp exp, Exp outer)
        {
            typeRef.Add(type.base_type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BinaryExpressionSyntax exp, Exp outer)
        {
            throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ConstExp exp, Exp outer)
        {
            typeRef.Add(model.GetExpType(exp).GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, FieldExp exp, Exp outer)
        {
            DB_Type caller = model.GetExpType(exp.Caller);
            typeRef.Add(caller.GetRefType());
            if(outer is MethodExp)
            {
                MethodExp methodExp = outer as MethodExp;
                List<DB_Type> argTypes = new List<DB_Type>();
                foreach (var a in methodExp.Args)
                {
                    argTypes.Add(model.GetExpType(a));
                }
                DB_Member method = caller.FindMethod(exp.Name, argTypes, model);
                typeRef.Add(method.type);
            }
            else
            {
                typeRef.Add(caller.members[exp.Name].typeName);
            }
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, MethodExp exp, Exp outer)
        {
            //Expression.MethodExp e = exp as Expression.MethodExp;
            //DB_Type caller = model.GetExpType(e.Expression);
            //typeRef.Add(caller.GetRefType());
            //List<DB_Type> argTypes = new List<DB_Type>();
            //foreach (var a in e.Args)
            //{
            //    argTypes.Add(model.GetExpType(a));
            //}
            //typeRef.Add(caller.FindMethod(e.Name, argTypes, this.model).type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ObjectCreateExp exp, Exp outer)
        {
            typeRef.Add(exp.Type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ParenthesizedExpressionSyntax exp, Exp outer)
        {
            throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PostfixUnaryExpressionSyntax exp, Exp outer)
        {
            throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PrefixUnaryExpressionSyntax exp, Exp outer)
        {
            throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThisExp exp, Exp outer)
        {
            typeRef.Add(type.GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThrowExp exp, Exp outer)
        {
            throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, IndifierExp exp, Exp outer)
        {
            //throw new NotImplementedException();
            DB_Type vt = model.GetIndifierInfo(exp.Name).type;
            typeRef.Add(vt.GetRefType());
        }
    }
}
