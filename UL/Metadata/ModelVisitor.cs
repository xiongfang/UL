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
    }

    public interface IMemberVisitor
    {
        void VisitMember(DB_Type type, DB_Member member);
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
        void VisitExp(DB_Type type, DB_Member member, DB_StatementSyntax statement, Expression.ElementAccessExp exp, Expression.Exp outer);

        void VisitTryCatchClauseSyntax(IMethodVisitor visitor, DB_Type type, DB_Member method, CatchClauseSyntax catchClauseSyntax, DB_TryStatementSyntax outer);
    }


    public partial class Model
    {
        public void AcceptTypeVisitor(ITypeVisitor visitor,DB_Type type)
        {
            VisitType(type,visitor);
        }

        void VisitType(DB_Type type, ITypeVisitor visitor)
        {
            //StartUsing(type.usingNamespace);
            EnterNamespace(type._namespace);
            EnterType(type);

            visitor.VisitType(type);

            LeaveType();
            LeaveNamespace();
            //ClearUsing();
        }

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
        

        public void VisitStatement(IMethodVisitor visitor,DB_Type type, DB_Member method, DB_StatementSyntax statement, DB_StatementSyntax outer)
        {
            if (statement is DB_BlockSyntax)
            {
                EnterBlock();
                visitor.VisitStatement(type, method, statement as DB_BlockSyntax, outer);
                LeaveBlock();
            }
            else if (statement is DB_IfStatementSyntax)
            {
                visitor.VisitStatement(type, method, statement as DB_IfStatementSyntax, outer);
            }
            else if (statement is DB_ForStatementSyntax)
            {
                EnterBlock();
                VariableDeclarationSyntax Declaration = ((DB_ForStatementSyntax)statement).Declaration;
                foreach (var e in Declaration.Variables)
                {
                    AddLocal(e.Identifier, Finder.FindType(Declaration.Type, this));
                }
                visitor.VisitStatement(type, method, statement as DB_ForStatementSyntax, outer);
                LeaveBlock();
            }
            else if (statement is DB_LocalDeclarationStatementSyntax)
            {
                DB_LocalDeclarationStatementSyntax ss = statement as DB_LocalDeclarationStatementSyntax;
                VariableDeclarationSyntax Declaration = ss.Declaration;
                foreach (var e in Declaration.Variables)
                {
                    AddLocal(e.Identifier,Finder.FindType(Declaration.Type, this));
                }
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
                throw new NotSupportedException("不支持的语句 " + statement.GetType().Name);
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
            else if(exp is Expression.ElementAccessExp)
            {
                visitor.VisitExp(type, method, statement, exp as Expression.ElementAccessExp, outer);
            }
            else
            {
                throw new NotSupportedException("不支持的表达式 " + exp.GetType().ToString());
            }

        }

        public void VisitTryCatchClauseSyntax(IMethodVisitor visitor, DB_Type type, DB_Member method, CatchClauseSyntax catchClauseSyntax, DB_TryStatementSyntax outer)
        {
            EnterBlock();
            AddLocal(catchClauseSyntax.Identifier, Finder.FindType(catchClauseSyntax.Type, this));
            visitor.VisitTryCatchClauseSyntax(visitor, type, method, catchClauseSyntax, outer);
            LeaveBlock();
        }


    }
}
