using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Odbc;


namespace CSharpCompiler
{
    class Program
    {

        static OdbcConnection _con;
        static OdbcTransaction _trans;


        static SemanticModel model;

        static void Main(string[] args)
        {
            string code = System.IO.File.ReadAllText(args[0]);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);


            var compilation = CSharpCompilation.Create("HelloWorld")
                                               .AddReferences(
                                                    MetadataReference.CreateFromFile(
                                                        typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(tree);

            //获取所有类
            {
                model = compilation.GetSemanticModel(tree);
            }


            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
            {
                con.Open();
                _con = con;

                OdbcTransaction trans = con.BeginTransaction();
                _trans = trans;

                var root = (CompilationUnitSyntax)tree.GetRoot();
                IEnumerable<SyntaxNode> nodes = root.DescendantNodes();

                //导出所有类
                var classNodes = nodes.OfType<ClassDeclarationSyntax>();
                foreach (var c in classNodes)
                {
                    ExportClass(c);
                }

                Console.WriteLine("Commit...");
                trans.Commit();
            }
        }

        static bool ContainModifier(SyntaxTokenList Modifiers,string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }

        static void ExportClass(ClassDeclarationSyntax c)
        {
            
            Metadata.DB_Type type = new Metadata.DB_Type();

            

            bool isPublic = ContainModifier(c.Modifiers,"public");
            bool isProtected = ContainModifier(c.Modifiers, "protected");
            bool isPrivate = !isPublic && !isProtected;
            type.is_abstract = ContainModifier(c.Modifiers, "abstract");

            Console.WriteLine("Identifier:" + c.Identifier);
            Console.WriteLine("Modifiers:" + c.Modifiers);
            type.modifier = Metadata.DB.MakeModifier(isPublic, isPrivate, isProtected);
            type.is_interface = false;
            type.is_value_type = false;

            NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
            if(namespaceDeclarationSyntax!=null)
            {
                type.full_name = namespaceDeclarationSyntax.Name.ToString() + "." + c.Identifier.Text;
            }
            else
            {
                type.full_name = c.Identifier.Text;
            }
            Metadata.DB.SaveDBType(type, _con, _trans);

            //导出所有变量
            var virableNodes = c.DescendantNodes().OfType<VariableDeclarationSyntax>();
            foreach (var v in virableNodes)
            {
                ExportVariable(v, type);
            }

            //导出所有方法
            var funcNodes = c.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var f in funcNodes)
            {
                ExportMethod(f, type);
            }
            Console.WriteLine();
        }

        static void ExportVariable(VariableDeclarationSyntax v, Metadata.DB_Type type)
        {
            TypeInfo ti = model.GetTypeInfo(v.Type);
            foreach(var ve in v.Variables)
            {
                Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                dB_Member.name = ve.Identifier.Text;
                dB_Member.is_static = ti.Type.IsStatic;
                dB_Member.declaring_type = type.full_name; 
                dB_Member.member_type = (int)Metadata.MemberTypes.Field;
                dB_Member.modifier = 0;
                Metadata.MemberVaiable mv = new Metadata.MemberVaiable();

                mv.type_fullname = GetTypeFullName(ti);

                dB_Member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mv);

                Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
            }
            
        }

        static string GetTypeFullName(TypeInfo ti)
        {
            return GetTypeFullName(ti.Type);
        }

        static string GetTypeFullName(ITypeSymbol Type)
        {
            if (Type.TypeKind == TypeKind.Array)
            {
                IArrayTypeSymbol arrayType = (IArrayTypeSymbol)Type;
                return GetTypeFullName(arrayType.ElementType)+"[]";
            }

            if (Type.ContainingNamespace != null)
                return Type.ContainingNamespace.Name + "." + Type.Name;
            else if (Type.ContainingType != null)
                return Type.ContainingType.Name + "." + Type.Name;
            return Type.Name;
        }

        static string GetTypeFullName(TypeSyntax ts)
        {
            TypeInfo ti = model.GetTypeInfo(ts);
            return GetTypeFullName(ti);
        }

        static void ExportMethod(MethodDeclarationSyntax f, Metadata.DB_Type type)
        {
            Console.WriteLine("\tIdentifier:" + f.Identifier);
            Console.WriteLine("\tModifiers:" + f.Modifiers);
            Console.WriteLine("\tReturnType:" + f.ReturnType);
            TypeInfo ti = model.GetTypeInfo(f.ReturnType);

            Metadata.DB_Member dB_Member = new Metadata.DB_Member();
            dB_Member.name = f.Identifier.Text;
            dB_Member.is_static = ti.Type.IsStatic;
            dB_Member.declaring_type = type.full_name;
            dB_Member.member_type = (int)Metadata.MemberTypes.Field;
            dB_Member.modifier = 0;

            Metadata.MemberFunc mf = new Metadata.MemberFunc();
            mf.args = new Metadata.MemberFunc.Args[f.ParameterList.Parameters.Count];
            for (int i=0;i< f.ParameterList.Parameters.Count;i++)
            {
                mf.args[i] = new Metadata.MemberFunc.Args();
                mf.args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                mf.args[i].type_fullname = GetTypeFullName(f.ParameterList.Parameters[i].Type);
                mf.args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                mf.args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
            }

            mf.ret_type = GetTypeFullName(f.ReturnType);

            
            ExportBody(f.Body, mf);

            dB_Member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mf);
            Metadata.DB.SaveDBMember(dB_Member, _con, _trans);

            Console.WriteLine();
        }

        static void ExportBody(BlockSyntax bs, Metadata.MemberFunc mf)
        {
            mf.body = ExportStatement(bs) as Metadata.DB_BlockSyntax;
        }



        static Metadata.DB_StatementSyntax ExportStatement(StatementSyntax node)
        {
            if (node is IfStatementSyntax)
            {
                return ExportStatement(node as IfStatementSyntax);
            }
            else if (node is ExpressionStatementSyntax)
            {
                return ExportStatement(node as ExpressionStatementSyntax);
            }
            else if(node is BlockSyntax)
            {
                return ExportStatement(node as BlockSyntax);
            }
            else if(node is LocalDeclarationStatementSyntax)
            {
                return ExportStatement(node as LocalDeclarationStatementSyntax);
            }
            else
            {
                Console.Error.WriteLine("error:Unsopproted StatementSyntax" + node);
            }
            return null;
        }
        static Metadata.DB_StatementSyntax ExportStatement(BlockSyntax bs)
        {
            Metadata.DB_BlockSyntax db_bs = new Metadata.DB_BlockSyntax();

            foreach (var node in bs.ChildNodes())
            {
                if (node is StatementSyntax)
                {
                    Metadata.DB_StatementSyntax ss = ExportStatement(node as StatementSyntax);
                    if (ss != null)
                        db_bs.List.Add(ss);
                }
            }

            return db_bs;
        }

        static Metadata.DB_StatementSyntax ExportStatement(IfStatementSyntax ss)
        {
            Metadata.DB_IfStatementSyntax db_ss = new Metadata.DB_IfStatementSyntax();

            db_ss.Condition = ExportExp(ss.Condition);
            db_ss.Statement = ExportStatement(ss.Statement);
            if(ss.Else!=null)
                db_ss.Else = ExportStatement(ss.Else.Statement);
            return db_ss;
        }

        static Metadata.DB_ExpressionStatementSyntax ExportStatement(ExpressionStatementSyntax ss)
        {
            Metadata.DB_ExpressionStatementSyntax db_ss = new Metadata.DB_ExpressionStatementSyntax();
            db_ss.Exp = ExportExp(ss.Expression);
            return db_ss;
        }

        static Metadata.DB_StatementSyntax ExportStatement(LocalDeclarationStatementSyntax ss)
        {
            Metadata.DB_LocalDeclarationStatementSyntax db_ss = new Metadata.DB_LocalDeclarationStatementSyntax();
            db_ss.Type = GetTypeFullName(ss.Declaration.Type);
            foreach(var v in ss.Declaration.Variables)
            {
                db_ss.Variables.Add(ExportExp(v));
            }
            return db_ss;
        }



        static Metadata.DB_ExpressionSyntax ExportExp(ExpressionSyntax es)
        {
            if(es is LiteralExpressionSyntax)
            {
                return ExportExp(es as LiteralExpressionSyntax);
            }
            else if(es is InitializerExpressionSyntax)
            {
                return ExportExp(es as InitializerExpressionSyntax);
            }
            else if(es is ObjectCreationExpressionSyntax)
            {
                return ExportExp(es as ObjectCreationExpressionSyntax);
            }
            else if(es is InvocationExpressionSyntax)
            {
                return ExportExp(es as InvocationExpressionSyntax);
            }
            else if(es is MemberAccessExpressionSyntax)
            {
                return ExportExp(es as MemberAccessExpressionSyntax);
            }
            else if(es is IdentifierNameSyntax)
            {
                return ExportExp(es as IdentifierNameSyntax);
            }
            else
            {
                Console.Error.WriteLine("error:Unsopproted Expression" + es);
            }
            return null;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(IdentifierNameSyntax es)
        {
            Metadata.DB_IdentifierNameSyntax db_les = new Metadata.DB_IdentifierNameSyntax();
            db_les.Name = es.Identifier.Text;
            return db_les;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(LiteralExpressionSyntax es)
        {
            Metadata.DB_LiteralExpressionSyntax db_les = new Metadata.DB_LiteralExpressionSyntax();
            db_les.token = es.Token.Text;
            return db_les;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(MemberAccessExpressionSyntax es)
        {
            Metadata.DB_MemberAccessExpressionSyntax db_les = new Metadata.DB_MemberAccessExpressionSyntax();
            db_les.Exp = ExportExp(es.Expression);
            db_les.name = es.Name.Identifier.Text;
            return db_les;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(InvocationExpressionSyntax es)
        {
            Metadata.DB_InvocationExpressionSyntax db_les = new Metadata.DB_InvocationExpressionSyntax();
            db_les.Exp = ExportExp(es.Expression);
            foreach(var a in es.ArgumentList.Arguments)
            {
                db_les.Arguments.Add(ExportExp(a) as Metadata.DB_ArgumentSyntax);
            }
            return db_les;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(ArgumentSyntax es)
        {
            Metadata.DB_ArgumentSyntax db_les = new Metadata.DB_ArgumentSyntax();
            db_les.Expression = ExportExp(es.Expression);
            return db_les;
        }
        static Metadata.DB_ExpressionSyntax ExportExp(InitializerExpressionSyntax es)
        {
            Metadata.DB_InitializerExpressionSyntax db_les = new Metadata.DB_InitializerExpressionSyntax();
            if(es.Expressions!=null)
            {
                foreach(var e in es.Expressions)
                {
                    db_les.Expressions.Add(ExportExp(e));
                }
            }
            return db_les;
        }

        static Metadata.DB_ExpressionSyntax ExportExp(ObjectCreationExpressionSyntax es)
        {
            Metadata.DB_ObjectCreationExpressionSyntax db_les = new Metadata.DB_ObjectCreationExpressionSyntax();
            if(es.Initializer!=null)
                db_les.Initializer = ExportExp(es.Initializer) as Metadata.DB_InitializerExpressionSyntax;

            if(es.ArgumentList!=null)
            {
                foreach (var a in es.ArgumentList.Arguments)
                {
                    db_les.Arguments.Add(ExportExp(a) as Metadata.DB_ArgumentSyntax);
                }
            }

            db_les.Type = GetTypeFullName(es.Type);
            return db_les;
        }
        static Metadata.VariableDeclaratorSyntax ExportExp(VariableDeclaratorSyntax es)
        {
            Metadata.VariableDeclaratorSyntax db_les = new Metadata.VariableDeclaratorSyntax();
            db_les.Identifier = es.Identifier.Text;
            db_les.Initializer = ExportExp(es.Initializer.Value) as Metadata.DB_InitializerExpressionSyntax;
            return db_les;
        }
    }
}
