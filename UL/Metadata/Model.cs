using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    public interface ITypeVisitor
    {
        void VisitType(DB_Type type);
        bool VisitMember(DB_Type type, DB_Member member);
        //返回是否访问子语句以及表达式
        void VisitStatement(DB_Type type, DB_Member member, DB_StatementSyntax statement);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.Exp exp);
    }

    public class Model
    {
        public IModelTypeFinder Finder;

        //当前类的命名空间
        public HashSet<string> usingNamespace = new HashSet<string>();
        public string outNamespace;
        //当前处理的类型
        Metadata.DB_Type currentType;
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











        ////查找指定的命名空间的所有类型
        //public static Dictionary<string, Metadata.DB_Type> FindNamespace(string ns)
        //{
        //    Dictionary<string, Metadata.DB_Type> rt = new Dictionary<string, Metadata.DB_Type>();


        //    foreach (var v in types.Select(a => { if (a.Value._namespace == ns) return a.Value; return null; }))
        //    {
        //        if (v != null)
        //            rt.Add(v.unique_name, v);
        //    }
        //    return rt;
        //}

        public Metadata.DB_Type GetType(string full_name)
        {
            return Finder.FindType(full_name);
        }

        public Metadata.DB_Type GetType(Metadata.Expression.TypeSyntax typeRef)
        {
            return Finder.FindType(typeRef);
        }

        public DB_Type GetExpType(Expression.Exp exp)
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

                //bool b_v;
                //if(bool.TryParse(e.value,out b_v))
                //{
                //    return GetType("System.Boolean");
                //}

                return GetType("System.String");
            }

            if (exp is Metadata.Expression.FieldExp)
            {
                Metadata.Expression.FieldExp e = exp as Metadata.Expression.FieldExp;
                Metadata.DB_Type caller_type = GetExpType(e.Caller);
                return GetType(caller_type.FindField(e.Name,this).typeName);
            }

            if (exp is Metadata.Expression.IndifierExp)
            {
                Metadata.Expression.IndifierExp e = exp as Metadata.Expression.IndifierExp;
                return GetIndifierInfo(e.Name).type;
            }

            if (exp is Metadata.Expression.MethodExp)
            {
                Metadata.Expression.MethodExp me = exp as Metadata.Expression.MethodExp;
                //Metadata.Expression.FieldExp e = me.Caller as Metadata.Expression.FieldExp;
                Metadata.DB_Type caller_type = GetExpType(me.Caller);
                List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                foreach (var t in me.Args)
                {
                    argTypes.Add(GetExpType(t));
                }
                Metadata.DB_Member member = caller_type.FindMethod(me.Name, argTypes,this);
                return GetType(member.typeName);
            }

            if (exp is Metadata.Expression.ObjectCreateExp)
            {
                Metadata.Expression.ObjectCreateExp e = exp as Metadata.Expression.ObjectCreateExp;
                return GetType(e.Type);
            }

            if(exp is Metadata.Expression.BaseExp)
            {
                return GetType(currentType.base_type);
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

        public IndifierInfo GetIndifierInfo(string name)
        {
            IndifierInfo info = new IndifierInfo();

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
                    DB_Type type = Finder.FindType(nsName+"."+name);
                    if (type != null)
                    {
                        info.is_type = true;
                        info.type = type;
                        return info;
                    }
                }
            }

            return null;
        }

        //public static Metadata.DB_Type FindVariable(string name)
        //{
        //    //查找本地变量
        //    foreach (var v in stackLocalVariables)
        //    {
        //        if (v.ContainsKey(name))
        //            return v[name];
        //    }

        //    //查找成员变量
        //    if (currentType != null)
        //    {
        //        if (currentType.members.ContainsKey(name))
        //            return Finder.FindType(currentType.members[name].typeName);
        //        //查找泛型
        //        if (currentType.is_generic_type_definition)
        //        {
        //            foreach (var gd in currentType.generic_parameter_definitions)
        //            {
        //                if (gd.type_name == name)
        //                {
        //                    return Metadata.DB_Type.MakeGenericParameterType(currentType, gd);
        //                }
        //            }
        //        }
        //    }

        //    //当前命名空间查找
        //    if(!string.IsNullOrEmpty(outNamespace))
        //    {
        //        Dictionary<string, Metadata.DB_Type> nsTypes = Finder.FindNamespace(outNamespace);
        //        if (nsTypes != null && nsTypes.ContainsKey(name))
        //            return nsTypes[name];
        //    }

        //    //当前命名空间查找
        //    foreach (var nsName in usingNamespace)
        //    {
        //        Dictionary<string, Metadata.DB_Type> nsTypes = Finder.FindNamespace(nsName);
        //        if (nsTypes != null && nsTypes.ContainsKey(name))
        //            return nsTypes[name];
        //    }

        //    return null;
        //}

        public void Visit(DB_Type type,ITypeVisitor visitor)
        {
            StartUsing(type.usingNamespace);
            EnterNamespace(type._namespace);
            EnterType(type);
            visitor.VisitType(type);
            foreach(var m in type.members)
            {
                if(m.Value.member_type == (int)MemberTypes.Method || m.Value.member_type == (int)MemberTypes.Constructor)
                {
                    EnterMethod(m.Value);
                }
                
                if(visitor.VisitMember(type, m.Value))
                {
                    if (m.Value.method_body != null)
                    {
                        VisitStatement(type, visitor, m.Value, m.Value.method_body);
                    }
                }
                if (m.Value.member_type == (int)MemberTypes.Method || m.Value.member_type == (int)MemberTypes.Constructor)
                {
                    LeaveMethod();
                }
            }
            LeaveType();
            LeaveNamespace();
            ClearUsing();
        }

        void VisitStatement(DB_Type type, ITypeVisitor visitor,DB_Member method,DB_StatementSyntax statement)
        {
            visitor.VisitStatement(type, method, statement);
            if (statement is DB_BlockSyntax)
            {
                EnterBlock();
                DB_BlockSyntax bs = statement as DB_BlockSyntax;
                foreach(var s in bs.List)
                {
                    VisitStatement(type, visitor, method, s);
                }
                LeaveBlock();
            }
            else if(statement is DB_IfStatementSyntax)
            {
                DB_IfStatementSyntax ss = statement as DB_IfStatementSyntax;
                VisitExp(type, visitor, method, statement, ss.Condition);
                VisitStatement(type, visitor, method, ss.Statement);
                if(ss.Else!=null)
                {
                    VisitStatement(type, visitor, method, ss.Else);
                }
            }
            else if(statement is DB_ForStatementSyntax)
            {
                EnterBlock();
                DB_ForStatementSyntax ss = statement as DB_ForStatementSyntax;
                if(ss.Declaration!=null)
                {
                    VisitDeclareVairable(type, visitor, method, statement, ss.Declaration);
                }
                VisitExp(type, visitor, method, statement, ss.Condition);
                foreach(var inc in ss.Incrementors)
                    VisitExp(type, visitor, method, statement, inc);

                VisitStatement(type, visitor, method, ss.Statement);

                LeaveBlock();
            }
            else if(statement is DB_LocalDeclarationStatementSyntax)
            {
                DB_LocalDeclarationStatementSyntax ss = statement as DB_LocalDeclarationStatementSyntax;
                VisitDeclareVairable(type, visitor, method, statement, ss.Declaration);
            }
            else if(statement is DB_DoStatementSyntax)
            {
                DB_DoStatementSyntax ss = statement as DB_DoStatementSyntax;
                VisitExp(type, visitor, method, statement, ss.Condition);
                VisitStatement(type, visitor, method, ss.Statement);
            }
            else if(statement is DB_ExpressionStatementSyntax)
            {
                DB_ExpressionStatementSyntax ss = statement as DB_ExpressionStatementSyntax;
                VisitExp(type, visitor, method, statement, ss.Exp);
            }
            else if(statement is DB_SwitchStatementSyntax)
            {
                DB_SwitchStatementSyntax ss = statement as DB_SwitchStatementSyntax;
                VisitExp(type, visitor, method, statement, ss.Expression);
                EnterBlock();
                foreach(var sec in ss.Sections)
                {
                    foreach(var l in sec.Labels)
                    {
                        VisitExp(type, visitor, method, statement, l);
                    }
                    foreach (var s in sec.Statements)
                    {
                        VisitStatement(type, visitor, method, s);
                    }
                }
                LeaveBlock();
            }
            else if(statement is DB_WhileStatementSyntax)
            {
                DB_WhileStatementSyntax ss = statement as DB_WhileStatementSyntax;
                VisitExp(type, visitor, method, statement, ss.Condition);
                VisitStatement(type, visitor, method, ss.Statement);
            }
        }

        void VisitExp(DB_Type type, ITypeVisitor visitor, DB_Member method, DB_StatementSyntax statement,Expression.Exp exp)
        {
            visitor.VisitExp(type, method, statement, exp);
            if (exp is Expression.IndifierExp)
            {
            }
            else if (exp is Expression.FieldExp)
            {
                Expression.FieldExp e = exp as Expression.FieldExp;
                VisitExp(type, visitor, method, statement, e.Caller);
            }
            else if (exp is Expression.ObjectCreateExp)
            {
                Expression.ObjectCreateExp e = exp as Expression.ObjectCreateExp;
                foreach (var a in e.Args)
                {
                    VisitExp(type, visitor, method, statement, a);
                }
                
            }
            else if (exp is Expression.ConstExp)
            {
                Expression.ConstExp e = exp as Expression.ConstExp;
                
            }
            else if (exp is Expression.MethodExp)
            {
                Expression.MethodExp e = exp as Expression.MethodExp;
                VisitExp(type, visitor, method, statement, e.Caller);
                List<DB_Type> argTypes = new List<DB_Type>();
                foreach (var a in e.Args)
                {
                    VisitExp(type, visitor, method, statement, a);
                }
            }

        }

        void VisitDeclareVairable(DB_Type type, ITypeVisitor visitor, DB_Member method, DB_StatementSyntax statement,VariableDeclarationSyntax Declaration)
        {
            foreach (var e in Declaration.Variables)
            {
                AddLocal(e.Identifier,Finder.FindType( Declaration.Type));
                VisitExp(type, visitor, method, statement, e.Initializer);
            }
        }

    }

    public class MyCppHeaderTypeFinder : ITypeVisitor
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
            foreach (var m in type.members.Values)
            {
                if (m.member_type == (int)Metadata.MemberTypes.Field)
                {
                    result.Add(m.field_type);
                }
                else if (m.member_type == (int)Metadata.MemberTypes.Method || m.member_type == (int)Metadata.MemberTypes.Constructor)
                {
                    if (!m.method_ret_type.IsVoid)
                        result.Add(m.method_ret_type);
                    foreach (var a in m.method_args)
                    {
                        result.Add(a.type);
                    }
                }
            }
        }
        public bool VisitMember(DB_Type type, DB_Member member)
        {
            return false;
        }
        public void VisitStatement(DB_Type type, DB_Member member, DB_StatementSyntax statement)
        {

        }
        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.Exp exp)
        {

        }
    }

    public class MyCppMethodBodyTypeFinder:ITypeVisitor
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
        public bool VisitMember(DB_Type type, DB_Member member)
        {
            return true;
        }
        //返回是否访问子语句以及表达式
        public void VisitStatement(DB_Type type, DB_Member member, DB_StatementSyntax statement)
        {

        }
        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.Exp exp)
        {
            if(exp is Expression.IndifierExp)
            {
                Expression.IndifierExp e = exp as Expression.IndifierExp;
                DB_Type vt = model.GetIndifierInfo(e.Name).type;
                typeRef.Add(vt.GetRefType());
            }
            else if(exp is Expression.FieldExp)
            {
                Expression.FieldExp e = exp as Expression.FieldExp;
                DB_Type caller = model.GetExpType(e.Caller);
                typeRef.Add(caller.GetRefType());
                typeRef.Add(caller.members[e.Name].typeName);
            }
            else if(exp is Expression.ObjectCreateExp)
            {
                Expression.ObjectCreateExp e = exp as Expression.ObjectCreateExp;
                typeRef.Add(e.Type);
            }
            else if(exp is Expression.ConstExp)
            {
                Expression.ConstExp e = exp as Expression.ConstExp;
                typeRef.Add(model.GetExpType(e).GetRefType());
            }
            else if(exp is Expression.MethodExp)
            {
                Expression.MethodExp e = exp as Expression.MethodExp;
                DB_Type caller = model.GetExpType(e.Caller);
                typeRef.Add(caller.GetRefType());
                List<DB_Type> argTypes = new List<DB_Type>();
                foreach(var a in e.Args)
                {
                    argTypes.Add(model.GetExpType(a));
                }
                typeRef.Add(caller.FindMethod(e.Name, argTypes,this.model).method_ret_type);
            }
        }
    }
}
