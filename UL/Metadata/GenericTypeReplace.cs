using Metadata.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata
{
    class GenericTypeReplace:ITypeVisitor,IMemberVisitor,IMethodVisitor
    {
        Model model;

        public GenericTypeReplace(Model model)
        {
            this.model = model;

        }
        //public HashSet<Expression.TypeSyntax> typeRef = new HashSet<Expression.TypeSyntax>();

        TypeSyntax ReplaceType(DB_Type type, TypeSyntax oldTs)
        {
             if(model.GetType(oldTs).is_generic_paramter)
            {
                for (int i = 0; i < type.generic_parameter_definitions.Count; i++)
                {
                    if (type.generic_parameter_definitions[i].type_name == oldTs.Name)
                        return type.generic_parameters[i];
                }
            }

            if(oldTs is GenericNameSyntax)
            {
                GenericNameSyntax genericNameSyntax = oldTs as GenericNameSyntax;
                for(int i=0;i< genericNameSyntax.Arguments.Count;i++)
                {
                    genericNameSyntax.Arguments[i] = ReplaceType(type, oldTs);
                }
            }

            return oldTs;
        }

        public IMethodVisitor GetMethodVisitor() { return this; }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer)
        {
            //throw new NotImplementedException();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer)
        {
            model.EnterBlock();
            foreach (var s in statement.List)
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

            statement.Declaration.Type = ReplaceType(type, statement.Declaration.Type);

            model.EnterBlock();
            DB_ForStatementSyntax ss = statement as DB_ForStatementSyntax;
            if (ss.Declaration != null)
            {
                VisitDeclareVairable(type, member, statement, ss.Declaration);
            }
            model.VisitExp(this, type, member, statement, ss.Condition, null);
            foreach (var inc in ss.Incrementors)
                model.VisitExp(this, type, member, statement, inc, null);

            model.VisitStatement(this, type, member, ss.Statement, statement);

            model.LeaveBlock();
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Condition, null);
            model.VisitStatement(this, type, member, statement.Statement, statement);
            if (statement.Else != null)
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
                model.AddLocal(e.Identifier, model.Finder.FindType(Declaration.Type));
                model.VisitExp(this, type, method, statement, e.Initializer, null);
            }
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer)
        {
            if (statement.Expression != null)
                model.VisitExp(this, type, member, statement, statement.Expression, null);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Expression, null);
            foreach (var sec in statement.Sections)
            {
                foreach (var l in sec.Labels)
                {
                    model.VisitExp(this, type, member, statement, l, null);
                }
                foreach (var s in sec.Statements)
                {
                    model.VisitStatement(this, type, member, s, statement);
                }
            }
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Expression, null);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer)
        {
            foreach (var c in statement.Catches)
            {
                c.Type = ReplaceType(type, c.Type);
            }
            foreach (var c in statement.Catches)
            {
                model.VisitStatement(this, type, member, c.Block, statement);
            }
            if (statement.Finally != null)
                model.VisitStatement(this, type, member, statement.Finally.Block, statement);
        }

        public void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer)
        {
            model.VisitExp(this, type, member, statement, statement.Condition, null);
            model.VisitStatement(this, type, member, statement.Statement, statement);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, AssignmentExpressionSyntax exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.Left, exp);
            model.VisitExp(this, type, member, statement, exp.Right, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BaseExp exp, Exp outer)
        {
            //typeRef.Add(type.base_type);
            type.base_type = ReplaceType(type, type.base_type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, BinaryExpressionSyntax exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.Left, exp);
            model.VisitExp(this, type, member, statement, exp.Right, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ConstExp exp, Exp outer)
        {
            //typeRef.Add(model.GetExpType(exp).GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, FieldExp exp, Exp outer)
        {
            //DB_Type caller = model.GetExpType(exp.Caller);
            //typeRef.Add(caller.GetRefType());


            //if (caller.members.ContainsKey(exp.Name))
            //{
            //    typeRef.Add(caller.members[exp.Name].typeName);
            //}
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
            foreach (var a in exp.Args)
            {
                model.VisitExp(this, type, member, statement, a, exp);
            }
            if (exp.Caller is IndifierExp)
            {

            }
            else
            {
                model.VisitExp(this, type, member, statement, exp.Caller, exp);
            }

            //DB_Type returnType = model.GetExpType(exp);
            //if (returnType != null)
            //    typeRef.Add(returnType.GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ObjectCreateExp exp, Exp outer)
        {
            //typeRef.Add(exp.Type);
            exp.Type = ReplaceType(type, exp.Type);
            foreach (var a in exp.Args)
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
            //typeRef.Add(type.GetRefType());
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ThrowExp exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.exp, exp);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, IndifierExp exp, Exp outer)
        {

            Model.IndifierInfo info = model.GetIndifierInfo(exp.Name);
            if (info.is_class_type_parameter)
            {
                Console.Error.Write("未处理的类型参数标识符 "+exp.Name);
            }
            //if (info.type != null)
            //    typeRef.Add(info.type.GetRefType());
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
            foreach (var m in type.members.Values)
                model.AcceptMemberVisitor(this, type, m);
        }

        public void VisitMember(DB_Type type, DB_Member member)
        {
            if (member.member_type == (int)Metadata.MemberTypes.Method)
            {
                if (member.method_body != null)
                {
                    model.VisitStatement(this, type, member, member.method_body, null);
                }

                if (member.method_args != null)
                {
                    foreach(var t in member.method_args)
                    {
                        t.type = ReplaceType(type, t.type);
                    }
                }
            }

            if (!member.type.IsVoid)
                member.type = ReplaceType(type, member.type);
        }

        public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, ElementAccessExp exp, Exp outer)
        {
            model.VisitExp(this, type, member, statement, exp.exp, exp);
            foreach (var e in exp.args)
            {
                model.VisitExp(this, type, member, statement, e, exp);
            }

        }
    }
}
