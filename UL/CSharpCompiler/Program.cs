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

            
            using (OdbcConnection con = new OdbcConnection("Dsn=MySql"))
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

        static void ExportClass(ClassDeclarationSyntax c)
        {
            
            Metadata.DB_Type type = new Metadata.DB_Type();

            type.name = c.Identifier.Text;

            bool isPublic = c.Modifiers.Count>0 && c.Modifiers.Count((a) => { return a.Text == "public"; }) >0;
            bool isProtected = c.Modifiers.Count > 0 && c.Modifiers.Count((a) => { return a.Text == "protected"; }) > 0;
            bool isPrivate = !isPublic && !isProtected;
            type.is_abstract = c.Modifiers.Count > 0 && c.Modifiers.Count((a) => { return a.Text == "abstract"; }) > 0;

            Console.WriteLine("Identifier:" + c.Identifier);
            Console.WriteLine("Modifiers:" + c.Modifiers);
            type.modifier = Metadata.DB.MakeModifier(isPublic, isPrivate, isProtected);
            type.is_interface = false;
            type.is_value_type = false;

            NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
            if(namespaceDeclarationSyntax!=null)
            {
                type._namespace = namespaceDeclarationSyntax.Name.ToString();
            }
            else
            {
                type._namespace = "";
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
                mv.type_fullname = ti.Type.Name;

                dB_Member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mv);

                Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
            }
            
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
            }

            Metadata.DB.SaveDBMember(dB_Member,_con,_trans);

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
