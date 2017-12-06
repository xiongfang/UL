using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata
{

    public interface ITypeVisitor
    {
        void VisitType(DB_Type type);
        //IMemberVisitor GetMemberVisitor();
    }

    public interface IMemberVisitor
    {
        void VisitMember(DB_Type type, DB_Member member);
        //IMethodVisitor GetMethodVisitor();
    }

    public interface IMethodVisitor
    {
        void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_DoStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_ExpressionStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_ForStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_LocalDeclarationStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer);
        void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer);

        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.AssignmentExpressionSyntax exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.BaseExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.BinaryExpressionSyntax exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ConstExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.FieldExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.MethodExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ObjectCreateExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ParenthesizedExpressionSyntax exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.PostfixUnaryExpressionSyntax exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.PrefixUnaryExpressionSyntax exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ThisExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ThrowExp exp, Expression.Exp outer);
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.IndifierExp exp, Expression.Exp outer);
    }


    public partial class Model
    {
        public void AcceptTypeVisitor(ITypeVisitor visitor,DB_Type type)
        {
            VisitType(type,visitor);
        }

        void VisitType(DB_Type type, ITypeVisitor visitor)
        {
            StartUsing(type.usingNamespace);
            EnterNamespace(type._namespace);
            EnterType(type);

            visitor.VisitType(type);

            LeaveType();
            LeaveNamespace();
            ClearUsing();
        }




        //public IMemberVisitor GetMemberVisitor() { return this; }

        public void AcceptMemberVisitor(IMemberVisitor visitor, DB_Type type, DB_Member member)
        {
            IMemberVisitor memberVisitor = visitor;

            if (memberVisitor != null)
            {
                if (member.member_type == (int)MemberTypes.Method)
                {
                    EnterMethod(member);
                    memberVisitor.VisitMember(type, member);
                    LeaveMethod();
                }
                else
                {
                    memberVisitor.VisitMember(type, member);
                }
            }
        }

        //public IMethodVisitor GetMethodVisitor() { return this; }

        //public void VisitStatement(DB_Type type, DB_Member member, DB_BreakStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_BlockSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    EnterBlock();
        //    foreach (var s in statement.List)
        //    {
        //        VisitStatement(type, member,s, statement);
        //    }
        //    LeaveBlock();
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_DoStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    DB_DoStatementSyntax ss = statement as DB_DoStatementSyntax;
        //    VisitExp(type, member, statement, ss.Condition,null);
        //    VisitStatement(type, member, ss.Statement,ss);
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_ExpressionStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    VisitExp(type, member, statement, statement.Exp, null);
        //}


        //void VisitDeclareVairable(DB_Type type, DB_Member method, DB_StatementSyntax statement, VariableDeclarationSyntax Declaration)
        //{
        //    foreach (var e in Declaration.Variables)
        //    {
        //        AddLocal(e.Identifier, Finder.FindType(Declaration.Type));
        //        VisitExp(type, method, statement, e.Initializer,null);
        //    }
        //}

        //public void VisitStatement(DB_Type type, DB_Member member, DB_ForStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    EnterBlock();
        //    DB_ForStatementSyntax ss = statement as DB_ForStatementSyntax;
        //    if (ss.Declaration != null)
        //    {
        //        VisitDeclareVairable(type, member, statement, ss.Declaration);
        //    }
        //    VisitExp(type, member, statement, ss.Condition,null);
        //    foreach (var inc in ss.Incrementors)
        //        VisitExp(type, member, statement, inc,null);

        //    VisitStatement(type, member, ss.Statement,statement);

        //    LeaveBlock();
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_IfStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    DB_IfStatementSyntax ss = statement as DB_IfStatementSyntax;
        //    VisitExp(type, member, statement, ss.Condition,null);
        //    VisitStatement(type, member, ss.Statement,ss);
        //    if (ss.Else != null)
        //    {
        //        VisitStatement(type, member, ss.Else,ss);
        //    }
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_LocalDeclarationStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    DB_LocalDeclarationStatementSyntax ss = statement as DB_LocalDeclarationStatementSyntax;
        //    VisitDeclareVairable(type, member, statement, ss.Declaration);
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_ReturnStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    DB_ReturnStatementSyntax ss = statement as DB_ReturnStatementSyntax;
        //    if (ss.Expression != null)
        //    {
        //        VisitExp(type, member, ss, ss.Expression,null);
        //    }
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_SwitchStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    VisitExp(type, member, statement, statement.Expression,null);
        //    EnterBlock();
        //    foreach (var sec in statement.Sections)
        //    {
        //        foreach (var l in sec.Labels)
        //        {
        //            VisitExp(type, member, statement, l,null);
        //        }
        //        foreach (var s in sec.Statements)
        //        {
        //            VisitStatement(type, member, s, statement);
        //        }
        //    }
        //    LeaveBlock();
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_ThrowStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    VisitExp(type, member, statement, statement.Expression,null);
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_TryStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    DB_TryStatementSyntax ss = statement as DB_TryStatementSyntax;
        //    VisitStatement(type, member, ss.Block,statement);
        //    foreach (var c in ss.Catches)
        //    {
        //        VisitStatement(type, member, c.Block, statement);
        //    }
        //    if (ss.Finally != null)
        //        VisitStatement(type, member, ss.Finally.Block, statement);
        //}
        //public void VisitStatement(DB_Type type, DB_Member member, DB_WhileStatementSyntax statement, DB_StatementSyntax outer)
        //{
        //    methodVisitor.VisitStatement(type, member, statement, outer);
        //    VisitExp(type, member, statement, statement.Condition,null);
        //    VisitStatement(type, member, statement.Statement, statement);
        //}

        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.AssignmentExpressionSyntax exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.BaseExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.BinaryExpressionSyntax exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ConstExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.FieldExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.MethodExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);

        //    VisitExp(type, member, statement, exp.Caller,exp);
        //    List<DB_Type> argTypes = new List<DB_Type>();
        //    foreach (var a in exp.Args)
        //    {
        //        VisitExp(type, member, statement, a,null);
        //    }
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ObjectCreateExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);

        //    foreach (var a in exp.Args)
        //    {
        //        VisitExp(type, member, statement, a, exp);
        //    }

        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ParenthesizedExpressionSyntax exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.PostfixUnaryExpressionSyntax exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.PrefixUnaryExpressionSyntax exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ThisExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}
        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ThrowExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}

        //public void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.IndifierExp exp, Expression.Exp outer)
        //{
        //    methodVisitor.VisitExp(type, member, statement, exp, outer);
        //}

        public void VisitStatement(IMethodVisitor visitor,DB_Type type, DB_Member method, DB_StatementSyntax statement, DB_StatementSyntax outer)
        {
            if (statement is DB_BlockSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_BlockSyntax, outer);
            }
            else if (statement is DB_IfStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_IfStatementSyntax, outer);
            }
            else if (statement is DB_ForStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_ForStatementSyntax, outer);
            }
            else if (statement is DB_LocalDeclarationStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_LocalDeclarationStatementSyntax, outer);
            }
            else if (statement is DB_DoStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_DoStatementSyntax, outer);
            }
            else if (statement is DB_ExpressionStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_ExpressionStatementSyntax, outer);
            }
            else if (statement is DB_SwitchStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_SwitchStatementSyntax, outer);
            }
            else if (statement is DB_WhileStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_WhileStatementSyntax, outer);
            }
            else if (statement is DB_TryStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_TryStatementSyntax, outer);
            }
            else if (statement is DB_ThrowStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_ThrowStatementSyntax, outer);
            }
            else if (statement is DB_ReturnStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_ReturnStatementSyntax, outer);
            }
            else
            {
                throw new NotSupportedException("不支持的语句 " + statement.JsonType);
            }
        }

        public void VisitExp(IMethodVisitor visitor, DB_Type type, DB_Member method, DB_StatementSyntax statement, Expression.Exp exp, Expression.Exp outer)
        {
            if (exp is Expression.IndifierExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.IndifierExp, outer);
            }
            else if (exp is Expression.FieldExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.FieldExp, outer);
            }
            else if (exp is Expression.ObjectCreateExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ObjectCreateExp, outer);
            }
            else if (exp is Expression.ConstExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ConstExp, outer);
            }
            else if (exp is Expression.MethodExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.MethodExp, outer);
            }
            else if (exp is Expression.ParenthesizedExpressionSyntax)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ParenthesizedExpressionSyntax, outer);
            }
            else if(exp is Expression.ThisExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ThisExp, outer);
            }
            else if (exp is Expression.BaseExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.BaseExp, outer);
            }
            else if (exp is Expression.AssignmentExpressionSyntax)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.AssignmentExpressionSyntax, outer);
            }
            else if (exp is Expression.BinaryExpressionSyntax)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.BinaryExpressionSyntax, outer);
            }
            else if (exp is Expression.PostfixUnaryExpressionSyntax)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.PostfixUnaryExpressionSyntax, outer);
            }
            else if (exp is Expression.PrefixUnaryExpressionSyntax)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.PrefixUnaryExpressionSyntax, outer);
            }
            else if (exp is Expression.ThrowExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ThrowExp, outer);
            }
            else
            {
                throw new NotSupportedException("不支持的表达式 " + exp.JsonType);
            }

        }

        //public void VisitTypeStart(DB_Type type)
        //{
        //    if(typeVisitor!=null)
        //        typeVisitor.VisitTypeStart(type);
        //}

        //public void VisitTypeEnd(DB_Type type)
        //{
        //    if (typeVisitor != null)
        //        typeVisitor.VisitTypeEnd(type);
        //}

        //public void VisitMethodStart(DB_Type type, DB_Member member)
        //{
        //    if (methodVisitor != null)
        //        methodVisitor.VisitMethodStart(type, member);
        //}

        //public void VisitMethodEnd(DB_Type type, DB_Member member)
        //{
        //    if (methodVisitor != null)
        //        methodVisitor.VisitMethodEnd(type, member);
        //}
    }
}
