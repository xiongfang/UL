using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.CompileNode;

namespace Model
{
    public class CSToUL
    {
        public static CompileNode_Globle Convert(string code)
        {
            CompileNode_Globle globleNode = new CompileNode_Globle();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
            //导出所有类
            var classNodes = nodes.OfType<MemberDeclarationSyntax>();


            if (root.Usings != null)
            {
                foreach (var u in root.Usings)
                {
                    globleNode.usingList.Add(u.Name.ToString());
                }
            }

            foreach (var c in classNodes)
            {
                if (c is ClassDeclarationSyntax)
                {
                    var classNode = new CompileNode_Class();
                    classNode.Parent = globleNode;
                    classNode.Compile(c as ClassDeclarationSyntax);
                }
                
            }

            return globleNode;
        }
    }
}
