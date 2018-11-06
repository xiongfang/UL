using System;
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
        DB_Type FindType(Expression.TypeSyntax refType,Model model);
        //Dictionary<string, Metadata.DB_Type> FindNamespace(string ns);
    }




    public partial class Model
    {
        public IModelTypeFinder Finder;

        //当前类的命名空间
        //public HashSet<string> usingNamespace = new HashSet<string>();
        public Stack<string> outNamespace = new Stack<string>();
        //当前处理的类型
        public Metadata.DB_Type currentType;
        Metadata.DB_Member currentMethod;
        //当前函数的本地变量和参数
        Stack<Dictionary<string, Metadata.DB_Type>> stackLocalVariables = new Stack<Dictionary<string, Metadata.DB_Type>>();


        public Model(IModelTypeFinder finder) { Finder = finder; }

        public string CurrentNameSpace
        {
            get
            {
                string ns = "";
                for(int i= outNamespace.Count-1; i>=0;i--)
                {
                    ns += outNamespace.ElementAt(i);
                    if (i >0)
                        ns += ".";
                }

                return ns;
            }
        }

        //public void StartUsing(List<string> usingList)
        //{
        //    foreach (var ns in usingList)
        //        usingNamespace.Add(ns);
        //}

        //public void ClearUsing()
        //{
        //    usingNamespace.Clear();
        //}

        public void EnterNamespace(string ns)
        {
            outNamespace.Push(ns);
        }

        public void LeaveNamespace()
        {
            outNamespace.Pop();
        }

        public void EnterType(Metadata.DB_Type type)
        {
            if(outNamespace.Count==0)
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
            return Finder.FindType(typeRef, this);
        }

        public DB_Type GetExpType(Expression.Exp exp, Expression.Exp outer=null)
        {
            if (exp is Metadata.Expression.ConstExp)
            {
                Metadata.Expression.ConstExp e = exp as Metadata.Expression.ConstExp;

                int int_v;
                if (int.TryParse(e.value, out int_v))
                {
                    return GetType("ul.System.Int32");
                }

                long long_v;
                if (long.TryParse(e.value, out long_v))
                {
                    return GetType("ul.System.Int64");
                }

                bool b_v;
                if (bool.TryParse(e.value, out b_v))
                {
                    return GetType("ul.System.Boolean");
                }

                float single_v;
                if (float.TryParse(e.value, out single_v))
                {
                    return GetType("ul.System.Single");
                }

                double double_v;
                if (double.TryParse(e.value, out double_v))
                {
                    return GetType("ul.System.Double");
                }

                if (e.value == "null")
                {
                    return GetType("ul.System.Object");
                }

                if(e.value.StartsWith("'"))
                {
                    return GetType("ul.System.Char");
                }

                return GetType("ul.System.String");
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

                    if (field == null)
                        return null;
                    return GetType(field.typeName);
                }
            }

            else if (exp is Metadata.Expression.IndifierExp)
            {
                Metadata.Expression.IndifierExp e = exp as Metadata.Expression.IndifierExp;
                IndifierInfo info = GetIndifierInfo(e.Name);
                //if(outer is MethodExp)
                //{
                //    MethodExp methodExp = outer as MethodExp;
                //    List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                //    foreach (var t in methodExp.Args)
                //    {
                //        argTypes.Add(GetExpType(t));
                //    }
                //    foreach(var m in info.methods)
                //    {
                //        if(m.MatchingParameter(argTypes,this))
                //        {
                //            return GetType(m.typeName);
                //        }
                //    }

                //}
                //else
                {
                    return info.type;
                }
            }

            else if (exp is Metadata.Expression.MethodExp)
            {
                Metadata.Expression.MethodExp me = exp as Metadata.Expression.MethodExp;

                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                foreach (var t in me.Args)
                {
                    argTypes.Add(GetExpType(t));
                }

                if (me.Caller is FieldExp)
                {
                    FieldExp fe = me.Caller as FieldExp;
                    Metadata.DB_Type caller_type = GetExpType(fe.Caller, fe);
                    return GetType(caller_type.FindMethod(fe.Name, argTypes, this).typeName);
                }
                else if(me.Caller is IndifierExp)
                {
                    IndifierExp ie = me.Caller as IndifierExp;
                    IndifierInfo info =  GetIndifierInfo(ie.Name);

                    if(info.is_method)
                    {
                        foreach(var m in info.methods)
                        {
                            if(m.MatchingParameter(argTypes,this))
                            {
                                return GetType(m.typeName);
                            }
                        }
                    }
                    else
                    {
                        return info.type;
                    }
                }
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
            else if(exp is Metadata.Expression.ElementAccessExp)
            {
                Metadata.Expression.ElementAccessExp eae = exp as Metadata.Expression.ElementAccessExp;
                Metadata.DB_Type caller_type = GetExpType(eae.exp);
                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                foreach(var a in eae.args)
                    argTypes.Add(GetExpType(a));


                return GetType(caller_type.FindProperty("Index", this).type);

                //string methodName = "";

                //if(outer is Metadata.Expression.AssignmentExpressionSyntax && ((Metadata.Expression.AssignmentExpressionSyntax)outer).Left == exp)
                //{
                //    methodName = "set_Index";
                //}
                //else
                //{
                //    methodName = "get_Index";
                //}

                //Metadata.DB_Member member = caller_type.FindMethod(methodName, argTypes, this);
                //return GetType(member.typeName);
            }
            else
            {
                Console.Error.WriteLine("无法确定表达式类型 " + exp.GetType().Name);
            }
            return null;
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


        public class IndifierInfo
        {
            public bool is_type;
            public bool is_var;
            public bool is_namespace;
            public bool is_field;
            public bool is_property;
            public bool is_event;
            public bool is_class_type_parameter;
            public bool is_method_type_parameter;
            public bool is_method;
            public List<DB_Member> methods;
            public Metadata.DB_Type type;
            public DB_Member field;
        }

        public enum EIndifierFlag
        {
            IF_Local = 1,
            IF_Type =1<<1,
            IF_Method = 1<<2,

            IF_All = IF_Local| IF_Type| IF_Method
        }

        public virtual IndifierInfo GetIndifierInfo(string name,EIndifierFlag flag = EIndifierFlag.IF_All)
        {
            IndifierInfo info = new IndifierInfo();

            //在指定的命名空间查找标示符
            if (name.Contains('.') && (flag & EIndifierFlag.IF_Type) != 0)
            {
                DB_Type type = Finder.FindType(name);
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
                        Metadata.DB_Type constraintType = Finder.FindType(gd.constraint,this);
                        info.type = Metadata.DB_Type.MakeGenericParameterType(constraintType, name);
                        return info;
                    }
                }
            }

            //查找成员变量
            if (currentType != null)
            {
                if ((flag & EIndifierFlag.IF_Local)!=0)
                {
                    if (currentType.FindField(name, this) != null)
                    {
                        info.is_field = true;
                        info.field = currentType.FindField(name, this);
                        info.type = GetType(info.field.typeName);
                        return info;
                    }
                    if (currentType.FindProperty(name, this) != null)
                    {
                        info.is_property = true;
                        info.field = currentType.FindProperty(name, this);
                        info.type = GetType(info.field.typeName);
                        return info;
                    }
                    if (currentType.FindEvent(name, this) != null)
                    {
                        info.is_event = true;
                        info.field = currentType.FindEvent(name, this);
                        info.type = GetType(info.field.typeName);
                        return info;
                    }
                }
                if ((flag & EIndifierFlag.IF_Method)!=0)
                {

                    List<DB_Member> methods = currentType.FindMethod(name, this);
                    if (methods.Count > 0)
                    {
                        info.is_method = true;
                        info.methods = methods;
                        return info;
                    }
                }
                if ((flag & EIndifierFlag.IF_Type) != 0)
                {
                    //查找泛型
                    if (currentType.is_generic_type_definition)
                    {
                        foreach (var gd in currentType.generic_parameter_definitions)
                        {
                            if (gd.type_name == name)
                            {
                                info.is_class_type_parameter = true;
                                Metadata.DB_Type constraintType = Finder.FindType(gd.constraint, this);
                                info.type = Metadata.DB_Type.MakeGenericParameterType(constraintType, name);
                                return info;
                            }
                        }
                    }
                    else if(currentType.is_generic_type)
                    {
                        //泛型实例，已经被替换了参数，所以无需查找
                    }
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

        public void VisitType(DB_Type type)
        {
            if (!type.base_type.IsVoid)
                result.Add(type.base_type);
            foreach (var i in type.interfaces)
            {
                result.Add(i);
            }
            foreach (var m in type.members.Values)
                model.AcceptMemberVisitor(this, type, m);
        }
    }

    public class MyCppMethodBodyTypeFinder : ITypeVisitor,IMemberVisitor, IMethodVisitor
    {
        Model model;
        public MyCppMethodBodyTypeFinder(Model model)
        {
            this.model = model;
        }
        public HashSet<Expression.TypeSyntax> typeRef = new HashSet<Expression.TypeSyntax>();


        public IMethodVisitor GetMethodVisitor() { return this; }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer)
        {
            model.EnterBlock();
            foreach(var s in statement.List)
            {
                model.VisitStatement(this, type, member, s, statement);
            }
            model.LeaveBlock();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_DoStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Condition, null);
            model.VisitStatement(this, type, member, statement.Statement, statement);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ExpressionStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Exp, null);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ForStatementSyntax statement, DB_StatementSyntax outer)
        {
            typeRef.Add(statement.Declaration.Type);

            model.EnterBlock();
            DB_ForStatementSyntax ss = statement as DB_ForStatementSyntax;
            if (ss.Declaration != null)
            {
                VisitDeclareVairable(type, member, statement, ss.Declaration);
            }
            model.VisitExp(this,type, member, statement, ss.Condition, null);
            foreach (var inc in ss.Incrementors)
                model.VisitExp(this,type, member, statement, inc, null);

            model.VisitStatement(this, type, member, ss.Statement, statement);

            model.LeaveBlock();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Condition, null);
            model.VisitStatement(this, type, member, statement.Statement, statement);
            if(statement.Else!=null)
                model.VisitStatement(this, type, member, statement.Else, statement);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_LocalDeclarationStatementSyntax statement, DB_StatementSyntax outer)
        {
            VisitDeclareVairable(type, member, statement, statement.Declaration);
        }

        void VisitDeclareVairable(DB_Type type, DB_Member method, DB_StatementSyntax statement, VariableDeclarationSyntax Declaration)
        {
            foreach (var e in Declaration.Variables)
            {
                model.AddLocal(e.Identifier, model.Finder.FindType(Declaration.Type, model));
                model.VisitExp(this,type, method, statement, e.Initializer, null);
            }
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer)
        {
            if(statement.Expression!=null)
                model.VisitExp(this, type, member, statement, statement.Expression, null);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Expression, null);
            foreach(var sec in statement.Sections)
            {
                foreach (var l in sec.Labels)
                {
                    model.VisitExp(this,type, member, statement, l, null);
                }
                foreach (var s in sec.Statements)
                {
                    model.VisitStatement(this, type, member, s, statement);
                }
            }
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this,type, member, statement, statement.Expression, null);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer)
        {
            foreach(var c in statement.Catches)
            {
                typeRef.Add(c.Type);
            }
            foreach (var c in statement.Catches)
            {
                model.VisitStatement(this,type, member, c.Block, statement);
            }
            if (statement.Finally != null)
                model.VisitStatement(this, type, member, statement.Finally.Block, statement);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this,type, member, statement, statement.Condition,null);
            model.VisitStatement(this, type, member, statement.Statement, statement);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, AssignmentExpressionSyntax exp, Exp outer)
        {
            model.VisitExp(this,type, member, statement, exp.Left, exp);
            model.VisitExp(this, type, member, statement, exp.Right, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BaseExp exp, Exp outer)
        {
            typeRef.Add(type.base_type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BinaryExpressionSyntax exp, Exp outer)
        {
            model.VisitExp(this,type, member, statement, exp.Left, exp);
            model.VisitExp(this, type, member, statement, exp.Right, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ConstExp exp, Exp outer)
        {
            typeRef.Add(model.GetExpType(exp).GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, FieldExp exp, Exp outer)
        {
            DB_Type caller = model.GetExpType(exp.Caller);
            typeRef.Add(caller.GetRefType());

            
            if(caller.members.ContainsKey(exp.Name))
            {
                typeRef.Add(caller.members[exp.Name].typeName);
            }
            //else
            //{
            //    List<DB_Member> methods = caller.FindMethod(exp.Name, model);
            //    if(methods.Count>0)
            //    {
            //        //typeRef.Add(caller.members[exp.Name].typeName);
            //    }
            //}


            model.VisitExp(this, type, member, statement, exp.Caller, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, MethodExp exp, Exp outer)
        {
            foreach(var a in exp.Args)
            {
                model.VisitExp(this, type, member, statement, a, exp);
            }
            if(exp.Caller is IndifierExp)
            {

            }
            else
            {
                model.VisitExp(this, type, member, statement, exp.Caller, exp);
            }

            DB_Type returnType = model.GetExpType(exp);
            if(returnType!=null)
                typeRef.Add(returnType.GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ObjectCreateExp exp, Exp outer)
        {
            typeRef.Add(exp.Type);

            foreach(var a in exp.Args)
                model.VisitExp(this, type, member, statement, a, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ParenthesizedExpressionSyntax exp, Exp outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PostfixUnaryExpressionSyntax exp, Exp outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, PrefixUnaryExpressionSyntax exp, Exp outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThisExp exp, Exp outer)
        {
            typeRef.Add(type.GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThrowExp exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.exp, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, IndifierExp exp, Exp outer)
        {

            Model.IndifierInfo info = model.GetIndifierInfo(exp.Name);
            if(info.type!=null)
                typeRef.Add(info.type.GetRefType());
        }

        //public void VisitMethodStart(DB_Type type, DB_Member member)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void VisitMethodEnd(DB_Type type, DB_Member member)
        //{
        //    //throw new NotImplementedException();
        //}

        public void VisitType(DB_Type type)
        {
            foreach(var m in type.members.Values)
                model.AcceptMemberVisitor(this,type,m);
        }

        public void VisitMember(DB_Type type, DB_Member member)
        {
            if(member.member_type == (int)Metadata.MemberTypes.Method)
            {
                if(member.method_body!=null)
                {
                    model.VisitStatement(this, type, member, member.method_body, null);
                }
            }
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ElementAccessExp exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.exp, exp);
            foreach(var e in exp.args)
            {
                model.VisitExp(this, type, member, statement, e, exp);
            }
            
        }
    }
}
