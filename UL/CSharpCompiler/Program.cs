using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = System.IO.File.ReadAllText(args[0]);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);

            var root = (CompilationUnitSyntax)tree.GetRoot();

            //导出所有类
            var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var c in classNodes)
            {
                ExportClass(c);
            }
        }

        static void ExportClass(ClassDeclarationSyntax c)
        {
            Console.WriteLine("Identifier:" + c.Identifier);
            Console.WriteLine("Modifiers:" + c.Modifiers);

            //导出所有变量
            var virableNodes = c.DescendantNodes().OfType<VariableDeclarationSyntax>();
            foreach (var v in virableNodes)
            {
                ExportVariable(v);
            }

            //导出所有方法
            var funcNodes = c.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var f in funcNodes)
            {
                ExportMethod(f);
            }
            Console.WriteLine();
        }

        static void ExportVariable(VariableDeclarationSyntax v)
        {
            
        }


        static void ExportMethod(MethodDeclarationSyntax f)
        {
            Console.WriteLine("\tIdentifier:" + f.Identifier);
            Console.WriteLine("\tModifiers:" + f.Modifiers);
            Console.WriteLine("\tReturnType:" + f.ReturnType);

            ExportBody(f.Body);


            Console.WriteLine();
        }

        static void ExportBody(BlockSyntax bs)
        {
            foreach (var node in bs.ChildNodes())
            {
                if (node is StatementSyntax)
                {
                    Console.WriteLine("\t\t" + ((StatementSyntax)node));
                    if (node as ExpressionStatementSyntax != null)
                    {
                        ExpressionStatementSyntax ss = node as ExpressionStatementSyntax;
                        ExpressionSyntax es = ss.Expression;
                    }

                    if (node is IfStatementSyntax)
                    {
                        
                        IfStatementSyntax ss = node as IfStatementSyntax;
                    }
                }
                else
                {
                    Console.Error.WriteLine("error:" + node);
                }
            }
        }
    }
}
