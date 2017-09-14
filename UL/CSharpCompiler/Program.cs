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

        static Dictionary<string, Dictionary<string, Metadata.DB_Type>> refTypes = new Dictionary<string, Dictionary<string, Metadata.DB_Type>>();
        static Dictionary<string, Dictionary<string, Metadata.DB_Type>> compilerTypes = new Dictionary<string, Dictionary<string, Metadata.DB_Type>>();

        static Dictionary<string, Dictionary<string, Metadata.DB_Member>> dicMembers = new Dictionary<string, Dictionary<string, Metadata.DB_Member>>();

        enum ECompilerStet
        {
            ScanType,
            ScanMember,
            Compile
        }
        static ECompilerStet step;
        static List<string> usingNamespace;
        //static SemanticModel model;

        static void AddCompilerType(Metadata.DB_Type type)
        {
            Dictionary<string, Metadata.DB_Type> rt = null;
            if (!compilerTypes.TryGetValue(type._namespace, out rt))
            {
                rt = new Dictionary<string, Metadata.DB_Type>();
                compilerTypes[type._namespace] = rt;
            }
            rt.Add(type.name, type);
        }
        static void AddMember(string full_name,Metadata.DB_Member member)
        {
            Dictionary<string, Metadata.DB_Member> members = null;
            if(!dicMembers.TryGetValue(full_name,out members))
            {
                members = new Dictionary<string, Metadata.DB_Member>();
                dicMembers.Add(full_name, members);
            }
            members.Add(member.identifier, member);
        }
        static Dictionary<string,Metadata.DB_Type> FindNamespace(string ns)
        {
            Dictionary<string, Metadata.DB_Type> rt = null;
            if (compilerTypes.TryGetValue(ns, out rt))
                return rt;
            if (refTypes.TryGetValue(ns, out rt))
                return rt;

            return null;
        }

        static Dictionary<string, Metadata.DB_Member> FindMembers(string full_name)
        {
            Dictionary<string, Metadata.DB_Member> members = null;
            if (!dicMembers.TryGetValue(full_name, out members))
                return null;
            return members;
        }

        static Metadata.DB_Type FindType(string nameOrFullname)
        {
            string name = Metadata.DB_Type.GetName(nameOrFullname);
            if (nameOrFullname.Contains("."))
            {
                Dictionary<string, Metadata.DB_Type> ns = FindNamespace(Metadata.DB_Type.GetNamespace(nameOrFullname));
                if(ns!=null && ns.ContainsKey(Metadata.DB_Type.GetName(nameOrFullname)))
                {
                    return ns[Metadata.DB_Type.GetName(nameOrFullname)];
                }
                else
                {
                    return null;
                }
            }

            //当前命名空间查找
            foreach(var nsName in usingNamespace)
            {
                Dictionary<string, Metadata.DB_Type> ns = FindNamespace(nsName);
                if (ns.ContainsKey(name))
                {
                    return ns[name];
                }
            }

            return null;
        }

        /*
         *  第一步：从数据库加载引用的类
         *  第二步：扫描所有类型
         *  第三步：扫描所有成员
         *  第四步：编译方法
         *  第五步：存储数据库
         * */
        static void Main(string[] args)
        {
            List<SyntaxTree> treeList = new List<SyntaxTree>();
            foreach (var file in args)
            {
                string code = System.IO.File.ReadAllText(file);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code);


                treeList.Add(tree);
            }

            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
            {
                con.Open();
                _con = con;

                OdbcTransaction trans = con.BeginTransaction();
                _trans = trans;

                foreach(var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();
                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    //导出所有类
                    var classNodes = nodes.OfType<ClassDeclarationSyntax>();
                    step = ECompilerStet.ScanType;
                    foreach (var c in classNodes)
                    {
                        ExportClass(c);
                    }


                }

                foreach (var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();
                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    var classNodes = nodes.OfType<ClassDeclarationSyntax>();

                    step = ECompilerStet.ScanMember;
                    foreach (var c in classNodes)
                    {
                        ExportClass(c);
                    }
                }

                foreach (var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();
                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    var classNodes = nodes.OfType<ClassDeclarationSyntax>();

                    step = ECompilerStet.Compile;
                    foreach (var c in classNodes)
                    {
                        ExportClass(c);
                    }
                }

                //存储数据库
                foreach (var v in compilerTypes)
                {
                    foreach (var c in v.Value)
                    {
                        Metadata.DB.SaveDBType(c.Value, _con, _trans);
                        //存储成员
                        Dictionary<string, Metadata.DB_Member> members = FindMembers(c.Value.full_name);
                        if (members != null)
                        {
                            foreach (var m in members.Values)
                            {
                                Metadata.DB.SaveDBMember(m, _con, _trans);
                            }
                        }
                    }
                }

                Console.WriteLine("Commit...");
                trans.Commit();
            }
        }


        static bool ContainModifier(SyntaxTokenList Modifiers,string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }

        static int GetModifier(SyntaxTokenList Modifiers)
        {
            bool isPublic = ContainModifier(Modifiers, "public");
            bool isProtected = ContainModifier(Modifiers, "protected");
            bool isPrivate = !isPublic && !isProtected;

            return Metadata.DB.MakeModifier(isPublic, isPrivate, isProtected); ;
        }

        //static void LoadTypesIfNotLoaded(string ns)
        //{
        //    if(FindNamespace(ns)==null)
        //    {
        //        Dictionary<string,Metadata.DB_Type> dictionary = Metadata.DB.Load(ns, _con);
        //        refTypes.Add(ns, dictionary);
        //    }
        //}



        static void ExportClass(ClassDeclarationSyntax c)
        {
            if(step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                //bool isPublic = ContainModifier(c.Modifiers, "public");
                //bool isProtected = ContainModifier(c.Modifiers, "protected");
                //bool isPrivate = !isPublic && !isProtected;
                type.is_abstract = ContainModifier(c.Modifiers, "abstract");

                Console.WriteLine("Identifier:" + c.Identifier);
                Console.WriteLine("Modifiers:" + c.Modifiers);
                type.modifier = GetModifier(c.Modifiers);
                type.is_interface = false;
                type.is_value_type = false;

                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    type.full_name = namespaceDeclarationSyntax.Name.ToString() + "." + c.Identifier.Text;
                    //foreach (var ns in namespaceDeclarationSyntax.Usings)
                    //{
                    //    LoadTypesIfNotLoaded(ns.Name.ToString());
                    //}
                }
                else
                {
                    type.full_name = c.Identifier.Text;
                }
                //Metadata.DB.SaveDBType(type, _con, _trans);
                AddCompilerType(type);
            }
            else if(step == ECompilerStet.ScanMember)
            {
                usingNamespace = new List<string>();
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                    foreach(var ns in namespaceDeclarationSyntax.Usings)
                    {
                        usingNamespace.Add(ns.Name.ToString());
                    }
                }
                Metadata.DB_Type type = FindType(c.Identifier.Text);

                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<FieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<MethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
                Console.WriteLine();
            }
            else if(step == ECompilerStet.Compile)
            {
                Metadata.DB_Type type = FindType(c.Identifier.Text);
                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<MethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
            }
        }

        static string GetKeywordTypeName(string kw)
        {
            switch(kw)
            {
                case "int":
                    return "System.Int32";
                case "string":
                    return "System.String";
                default:
                    return "void";
            }
        }


        static Metadata.DB_Type GetType(TypeSyntax typeSyntax)
        {
            
            if (typeSyntax is PredefinedTypeSyntax)
            {
                PredefinedTypeSyntax predefinedTypeSyntax = typeSyntax as PredefinedTypeSyntax;
                string typeName = GetKeywordTypeName(predefinedTypeSyntax.Keyword.Text);
                return FindType(typeName);
            }
            //else if(typeSyntax is ArrayTypeSyntax)
            //{
            //    ArrayTypeSyntax ts = typeSyntax as ArrayTypeSyntax;
            //    Metadata.DB_Type elementType = GetType(ts.ElementType);

            //}
            else if(typeSyntax is IdentifierNameSyntax)
            {
                IdentifierNameSyntax ts = typeSyntax as IdentifierNameSyntax;
                return FindType(ts.Identifier.Text);
            }

            return null;
        }

        static void ExportVariable(FieldDeclarationSyntax v, Metadata.DB_Type type)
        {
            Metadata.DB_Type v_type = GetType(v.Declaration.Type);

            if (v_type == null)
            {
                Console.Error.WriteLine("无法识别的类型 " + v);
                return;
            }

            if(step == ECompilerStet.ScanMember)
            {
                foreach (var ve in v.Declaration.Variables)
                {
                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.name = ve.Identifier.Text;
                    dB_Member.is_static = ContainModifier(v.Modifiers, "static");
                    dB_Member.declaring_type = type.full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.Field;
                    dB_Member.modifier = GetModifier(v.Modifiers);
                    dB_Member.field_type_fullname = v_type.full_name;

                    //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                    AddMember(type.full_name, dB_Member);
                }
            }


        }


        static Dictionary<MethodDeclarationSyntax, Metadata.DB_Member> MemberMap = new Dictionary<MethodDeclarationSyntax, Metadata.DB_Member>();
        static void ExportMethod(MethodDeclarationSyntax f, Metadata.DB_Type type)
        {
            if(step == ECompilerStet.ScanMember)
            {
                Console.WriteLine("\tIdentifier:" + f.Identifier);
                Console.WriteLine("\tModifiers:" + f.Modifiers);
                Console.WriteLine("\tReturnType:" + f.ReturnType);
                //TypeInfo ti = model.GetTypeInfo(f.ReturnType);

                

                Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                dB_Member.name = f.Identifier.Text;
                dB_Member.is_static = ContainModifier(f.Modifiers, "static");
                dB_Member.declaring_type = type.full_name;
                dB_Member.member_type = (int)Metadata.MemberTypes.Field;
                dB_Member.modifier = GetModifier(f.Modifiers);

                dB_Member.method_args = new Metadata.DB_Member.Argument[f.ParameterList.Parameters.Count];
                for (int i = 0; i < f.ParameterList.Parameters.Count; i++)
                {
                    dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                    dB_Member.method_args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                    dB_Member.method_args[i].type_fullname = GetType(f.ParameterList.Parameters[i].Type).full_name;
                    dB_Member.method_args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                    dB_Member.method_args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
                }

                Metadata.DB_Type retType = GetType(f.ReturnType);
                if(retType!=null)
                    dB_Member.method_ret_type = retType.full_name;

                AddMember(type.full_name, dB_Member);
                MemberMap[f] = dB_Member;
                Console.WriteLine();
            }
            else if(step == ECompilerStet.Compile)
            {
                Metadata.DB_Member dB_Member = MemberMap[f];
                dB_Member.method_body = ExportBody(f.Body);
            }
        }

        static Metadata.DB_BlockSyntax ExportBody(BlockSyntax bs)
        {
            return ExportStatement(bs) as Metadata.DB_BlockSyntax;
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
            db_ss.Type = GetType(ss.Declaration.Type).full_name;
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

            db_les.Type = GetType(es.Type).full_name;
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
