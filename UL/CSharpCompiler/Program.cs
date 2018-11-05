using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.Odbc;
using System.IO;

namespace CSharpCompiler
{
    class Finder : Metadata.IModelTypeFinder
    {
        //查找一个数据库类型
        public Metadata.DB_Type FindType(string full_name)
        {
            return CSharpModel.GetType(full_name);
        }
        //查找一个类型，如果是动态类型，构造一个
        public Metadata.DB_Type FindType(Metadata.Expression.TypeSyntax refType,Metadata.Model model)
        {
            return CSharpModel.GetType(refType, model);
        }
    }

    public class CSharpModel:Metadata.Model
    {

        Stack<List<string>> usingStack = new Stack<List<string>>();

        public CSharpModel(Metadata.IModelTypeFinder finder) : base(finder) { }

        public void StartUsing(List<string> namespaceList)
        {
            usingStack.Push(namespaceList);
        }
        public void EndUsing()
        {
            usingStack.Pop();
        }

        public override IndifierInfo GetIndifierInfo(string name, string name_space = "", EIndifierFlag flag = EIndifierFlag.IF_All)
        {
            IndifierInfo info = base.GetIndifierInfo(name, name_space, flag);
            if (info != null)
                return info;

            

            if ((flag & EIndifierFlag.IF_Type) != 0)
            {
                //当前命名空间查找
                if (!string.IsNullOrEmpty(CurrentNameSpace))
                {
                    Metadata.DB_Type type = FindTypeInNamespace(name, CurrentNameSpace);
                    if (type != null)
                    {
                        info = new IndifierInfo();
                        info.is_type = true;
                        info.type = type;
                        return info;
                    }
                }
                foreach (var nsList in usingStack)
                {
                    foreach(var nsName in nsList)
                    {
                        Metadata.DB_Type type = FindTypeInNamespace(name, nsName);
                        if (type != null)
                        {
                            info = new IndifierInfo();
                            info.is_type = true;
                            info.type = type;
                            return info;
                        }
                    }
                }
            }

            return null;
        }

        public static CSharpModel Instance = new CSharpModel(new Finder());
        //引用的外部类型
        public static Dictionary<string, Metadata.DB_Type> refTypes = new Dictionary<string, Metadata.DB_Type>();
        //编译中的类型
        public static Dictionary<string, Metadata.DB_Type> compilerTypes = new Dictionary<string, Metadata.DB_Type>();
        ////当前类的命名空间
        //public static List<string> usingNamespace = new List<string>();
        //static string currentUsing;
        ////当前处理的类型
        //static Metadata.DB_Type currentType;
        //static Metadata.DB_Member currentMethod;
        ////当前函数的本地变量和参数
        //static Stack<Dictionary<string, Metadata.DB_Type>> stackLocalVariables = new Stack<Dictionary<string, Metadata.DB_Type>>();

        public static void AddRefType(Metadata.DB_Type type)
        {
            refTypes.Add(type.static_full_name, type);
        }

        public static void AddCompilerType(Metadata.DB_Type type)
        {
            compilerTypes.Add(type.static_full_name, type);
        }

        public static void MergeCompilerType(Metadata.DB_Type type)
        {
            if(!compilerTypes.ContainsKey(type.static_full_name))
            {
                AddCompilerType(type);
                return;
            }

            Metadata.DB_Type last_type = compilerTypes[type.static_full_name];
            if (last_type.base_type.IsVoid && !type.base_type.IsVoid)
                last_type.base_type = type.base_type;

            last_type.interfaces.AddRange(type.interfaces);

        }

        public static void AddMember(string full_name, Metadata.DB_Member member)
        {
            Metadata.DB_Type type = GetType(full_name);
            if (type != null)
            {
                type.members.Add(member.identifier, member);
            }
        }

        //查找指定的命名空间的所有类型
        public static Dictionary<string, Metadata.DB_Type> FindNamespace(string ns)
        {
            Dictionary<string, Metadata.DB_Type> rt = new Dictionary<string, Metadata.DB_Type>();


            foreach (var v in compilerTypes.Select(a => { if (a.Value._namespace == ns) return a.Value; return null; }))
            {
                if(v!=null)
                    rt.Add(v.unique_name, v);
            }

            foreach (var v in refTypes.Select(a => { if (a.Value._namespace == ns) return a.Value; return null; }))
            {
                if (v != null)
                    rt.Add(v.unique_name, v);
            }


            return rt;
        }

        static HashSet<string> try_loaded = new HashSet<string>();
        public static Metadata.DB_Type GetType(string full_name)
        {
            if (compilerTypes.ContainsKey(full_name))
                return compilerTypes[full_name];
            if (refTypes.ContainsKey(full_name))
                return refTypes[full_name];

            if(!try_loaded.Contains(full_name))
            {
                try_loaded.Add(full_name);
                Metadata.DB_Type type = Metadata.DB.LoadType(Path.Combine(Program.project_dir, Program.project_data.ref_library_dir), full_name);
                if(type!=null)
                {
                    AddRefType(type);
                }
                return type;
            }
            return null;
        }


        public static Metadata.DB_Type GetType(Metadata.Expression.TypeSyntax typeSyntax,Metadata.Model model = null)
        {
            if(typeSyntax.isGenericType)
            {
                Metadata.DB_Type ma = GetType(typeSyntax.GetTypeDefinitionFullName());
                return Metadata.DB_Type.MakeGenericType(ma, typeSyntax.args, new Metadata.Model(new Finder()));
            }

            if(typeSyntax.isGenericParameter)
            {
                //return Metadata.DB_Type.MakeGenericParameterType(GetType(typeSyntax), new Metadata.GenericParameterDefinition() { type_name = gps.Name });
                if (model == null)
                    model = Instance;
                return model.GetIndifierInfo(typeSyntax.Name).type;
            }

            return GetType(typeSyntax.GetTypeDefinitionFullName());
        }
        
    }

    class Program
    {

        //public static OdbcConnection _con;
        //static OdbcTransaction _trans;

        static bool ingore_method_body;

        //static Dictionary<string, Dictionary<string, Metadata.DB_Member>> dicMembers = new Dictionary<string, Dictionary<string, Metadata.DB_Member>>();

        enum ECompilerStet
        {
            ScanType,
            ScanMember,
            Compile
        }
        static ECompilerStet step;

        //public static Metadata.DB_TypeRef GetTypeRef(Metadata.DB_Type type)
        //{
        //    Metadata.DB_TypeRef refData = new Metadata.DB_TypeRef();
        //    refData.identifer = type.full_name;
        //    foreach(var d in type.generic_parameters)
        //    {
        //        refData.parameters.Add(d);
        //    }
        //    if(type.is_generic_paramter)
        //    {
        //        refData.template_parameter_name = type.name;
        //    }

        //    return refData;
        //}

        public static Metadata.Expression.TypeSyntax GetTypeParameterSyntax(TypeParameterConstraintSyntax tpcs)
        {
            if(tpcs is TypeConstraintSyntax)
            {
                TypeConstraintSyntax tcs = tpcs as TypeConstraintSyntax;
                return GetTypeSyntax(tcs.Type);
            }
            else if(tpcs is ConstructorConstraintSyntax)
            {
                return new Metadata.Expression.TypeSyntax() { Name = "Object", name_space = "ul.System" ,isGenericParameter = true};
            }
            else
            {
                throw new Exception("不支持的约束类型 "+tpcs.GetType().FullName);
            }
            return null;
        }

        public static Metadata.Expression.TypeSyntax GetTypeSyntax(TypeSyntax typeSyntax,string ns = "")
        {
            if (typeSyntax == null)
                return null;

            if (typeSyntax is PredefinedTypeSyntax)
            {
                PredefinedTypeSyntax predefinedTypeSyntax = typeSyntax as PredefinedTypeSyntax;
                string typeName = GetKeywordTypeName(predefinedTypeSyntax.Keyword.Text);
                if (typeName == "void")
                    return Metadata.Expression.TypeSyntax.Void;
                //Metadata.Expression.QualifiedNameSyntax qns = new Metadata.Expression.QualifiedNameSyntax();
                //qns.Left = new Metadata.Expression.IdentifierNameSyntax() { Identifier = "System" };
                Metadata.Expression.TypeSyntax Right = new Metadata.Expression.TypeSyntax() { Name = typeName };
                Right.name_space = "ul.System";
                return Right;
            }
            else if (typeSyntax is ArrayTypeSyntax)
            {
                ArrayTypeSyntax ts = typeSyntax as ArrayTypeSyntax;
                Metadata.Expression.TypeSyntax elementType = GetTypeSyntax(ts.ElementType,"");
                List<Metadata.Expression.TypeSyntax> parameters = new List<Metadata.Expression.TypeSyntax>();
                parameters.Add(elementType);
                Metadata.Expression.TypeSyntax gns = new Metadata.Expression.TypeSyntax();
                gns.Name = "ArrayT";
                gns.args = parameters.ToArray();
                gns.isGenericType = true;
                if(elementType.isGenericParameter)
                {
                    gns.isGenericTypeDefinition = true;
                }
                //Metadata.Expression.QualifiedNameSyntax qns = new Metadata.Expression.QualifiedNameSyntax();
                //qns.Left = new Metadata.Expression.IdentifierNameSyntax() { Identifier = "System" };
                //qns.Right = gns;
                gns.name_space = "ul.System";
                //Metadata.DB_Type arrayType = Model.GetType("System.ArrayT[1]");
                return gns;
            }
            else if (typeSyntax is IdentifierNameSyntax)
            {
                IdentifierNameSyntax ts = typeSyntax as IdentifierNameSyntax;
                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(ts.Identifier.Text,ns,Metadata.Model.EIndifierFlag.IF_Type).type;
    

                //Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(Identifier).type;
                //CSharpModel.Instance.GetIndifierInfo(ts.Identifier.Text).type;
                if (type.is_generic_paramter)
                {
                    Metadata.Expression.TypeSyntax ins = new Metadata.Expression.TypeSyntax();

                    ins.Name = (ts.Identifier.Text);
                    ins.name_space = type._namespace;
                    ins.isGenericParameter = true;
                    //ins.declare_type = type.static_full_name;
                    return ins;
                }
                else
                {
                    Metadata.Expression.TypeSyntax ins = new Metadata.Expression.TypeSyntax();

                    ins.Name = (ts.Identifier.Text);
                    ins.name_space = type._namespace;
                    return ins;
                }

            }
            else if (typeSyntax is GenericNameSyntax)
            {
                GenericNameSyntax ts = typeSyntax as GenericNameSyntax;
                string Name = ts.Identifier.Text;
                List<Metadata.Expression.TypeSyntax> parameters = new List<Metadata.Expression.TypeSyntax>();
                foreach (var p in ts.TypeArgumentList.Arguments)
                {
                    parameters.Add(GetTypeSyntax(p,""));
                }
                Metadata.Expression.TypeSyntax gns = new Metadata.Expression.TypeSyntax();
                gns.Name = Name;
                gns.args = parameters.ToArray();
                gns.isGenericType = true;
                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(gns.GetTypeDefinitionName()).type;
                gns.name_space = type._namespace;
                return gns;
            }
            else if (typeSyntax is QualifiedNameSyntax)
            {
                QualifiedNameSyntax qns = typeSyntax as QualifiedNameSyntax;
                string name_space = qns.Left.ToString();
                if(!string.IsNullOrEmpty(ns))
                {
                    ns = ns + "." + name_space;
                }
                else
                {
                    ns = name_space;
                }
                //Metadata.Expression.QualifiedNameSyntax my_qns = new Metadata.Expression.QualifiedNameSyntax();
                //my_qns.Left = GetTypeSyntax(qns.Left) as Metadata.Expression.NameSyntax;
                Metadata.Expression.TypeSyntax ts = GetTypeSyntax(qns.Right, ns);
                ts.name_space = ns;
                return ts;
            }
            else
            {
                Console.Error.WriteLine("不支持的类型语法 " + typeSyntax.GetType().FullName);
            }

            return null;
        }

        static string GetKeywordTypeName(string kw)
        {
            switch (kw)
            {
                case "char":
                    return "Char";
                case "sbyte":
                    return "SByte";
                case "int":
                    return "Int32";
                case "string":
                    return "String";
                case "short":
                    return "Int16";
                case "byte":
                    return "Byte";
                case "float":
                    return "Single";
                case "double":
                    return "Double";
                case "object":
                    return "Object";
                case "bool":
                    return "Boolean";
                case "uint":
                    return "UInt32";
                case "ulong":
                    return "UInt64";
                case "long":
                    return "Int64";
                case "ushort":
                    return "UInt16";
                default:
                    return "void";
            }
        }

        public class ULProject
        {
            public string ref_library_dir;
            public string[] source_dirs;
            public bool ingore_method_body;
            public string output_dir = "Data";
        }

        public static ULProject project_data;
        public static string project_dir;

        /*
         *  第一步：从数据库加载引用的类
         *  第二步：扫描所有类型
         *  第三步：扫描所有成员
         *  第四步：编译方法
         *  第五步：存储数据库
         * */
        static void Main(string[] args)
        {
            List<string> arg_list = new List<string>(args);
            string project = System.IO.File.ReadAllText(args[0]);

            project_data = Metadata.DB.ReadJsonObject<ULProject>(project);
            ingore_method_body = project_data.ingore_method_body;
            project_dir = System.IO.Path.GetFullPath(args[0]);
            project_dir = project_dir.Substring(0, project_dir.Length - System.IO.Path.GetFileName(project_dir).Length - 1);

            //分析文件
            List<SyntaxTree> treeList = new List<SyntaxTree>();

            foreach (var dir in project_data.source_dirs)
            {
                string[] files = Directory.GetFiles(System.IO.Path.Combine(project_dir, dir), "*.cs");
                foreach (var file in files)
                {
                    string file_full_path = file;// System.IO.Path.Combine(pj_dir, file);
                    string code = System.IO.File.ReadAllText(file_full_path);
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                    treeList.Add(tree);
                }
            }


            foreach (var tree in treeList)
            {
                var root = (CompilationUnitSyntax)tree.GetRoot();

                List<string> usingList = new List<string>();
                if (root.Usings != null)
                {
                    foreach (var u in root.Usings)
                    {
                        usingList.Add(u.Name.ToString());
                    }
                }
                CSharpModel.Instance.StartUsing(usingList);

                step = ECompilerStet.ScanType;

                IEnumerable<SyntaxNode> nodes = root.ChildNodes();

                foreach(var n in nodes)
                {
                    if(n is NamespaceDeclarationSyntax)
                    {
                        ExportNameSpace(n as NamespaceDeclarationSyntax);
                    }
                }


                CSharpModel.Instance.EndUsing();

            }

            foreach (var tree in treeList)
            {
                var root = (CompilationUnitSyntax)tree.GetRoot();

                List<string> usingList = new List<string>();
                if (root.Usings != null)
                {
                    foreach (var u in root.Usings)
                    {
                        usingList.Add(u.Name.ToString());
                    }
                }
                CSharpModel.Instance.StartUsing(usingList);

                IEnumerable<SyntaxNode> nodes = root.ChildNodes();

                step = ECompilerStet.ScanMember;
                foreach (var n in nodes)
                {
                    if (n is NamespaceDeclarationSyntax)
                    {
                        ExportNameSpace(n as NamespaceDeclarationSyntax);
                    }
                }
                CSharpModel.Instance.EndUsing();
            }

            foreach (var tree in treeList)
            {
                var root = (CompilationUnitSyntax)tree.GetRoot();

                List<string> usingList = new List<string>();
                if (root.Usings != null)
                {
                    foreach (var u in root.Usings)
                    {
                        usingList.Add(u.Name.ToString());
                    }
                }
                CSharpModel.Instance.StartUsing(usingList);

                IEnumerable<SyntaxNode> nodes = root.ChildNodes();
                step = ECompilerStet.Compile;
                foreach (var n in nodes)
                {
                    if (n is NamespaceDeclarationSyntax)
                    {
                        ExportNameSpace(n as NamespaceDeclarationSyntax);
                    }
                }
                CSharpModel.Instance.EndUsing();
            }


            //存储数据库
            foreach (var v in CSharpModel.compilerTypes)
            {
                //foreach (var c in v.Value)
                {
                    Metadata.DB.SaveType(Path.Combine(project_dir, project_data.output_dir), v.Value);
                }
            }
        }

        static void ExportNameSpace(NamespaceDeclarationSyntax nds)
        {
            CSharpModel.Instance.EnterNamespace(nds.Name.ToString());

            List<string> usingList = new List<string>();
            if (nds.Usings != null)
            {
                foreach (var u in nds.Usings)
                {
                    usingList.Add(u.Name.ToString());
                }
            }
            CSharpModel.Instance.StartUsing(usingList);

            foreach (var n in nds.ChildNodes())
            {
                if (n is NamespaceDeclarationSyntax)
                {
                    ExportNameSpace(n as NamespaceDeclarationSyntax);
                }
                else if(n is MemberDeclarationSyntax)
                {
                    ExportType(n as MemberDeclarationSyntax);
                }
            }
            CSharpModel.Instance.EndUsing();
            CSharpModel.Instance.LeaveNamespace();
        }


        static bool ContainModifier(SyntaxTokenList Modifiers,string token)
        {
            return Modifiers.Count > 0 && Modifiers.Count((a) => { return a.Text == token; }) > 0;
        }

        static int GetModifier(Metadata.DB_Type current_type, SyntaxTokenList Modifiers)
        {
            bool isPublic = current_type.is_interface?true: ContainModifier(Modifiers, "public");
            bool isProtected = ContainModifier(Modifiers, "protected");
            bool isPrivate = !isPublic && !isProtected;

            if (isProtected)
                return 2;
            if (isPublic)
                return 0;
            if (isPrivate)
                return 1;

            return 1;
        }

        //static void LoadTypesIfNotLoaded(string ns)
        //{
        //    if(FindNamespace(ns)==null)
        //    {
        //        Dictionary<string,Metadata.DB_Type> dictionary = Metadata.DB.Load(ns, _con);
        //        refTypes.Add(ns, dictionary);
        //    }
        //}

        static void ExportType(MemberDeclarationSyntax c)
        {
            if(c is ClassDeclarationSyntax)
            {
                ExportClass(c as ClassDeclarationSyntax);
            }
            else if(c is StructDeclarationSyntax)
            {
                ExportStruct(c as StructDeclarationSyntax);
            }
            else if(c is InterfaceDeclarationSyntax)
            {
                ExportInterface(c as InterfaceDeclarationSyntax);
            }
            else if(c is EnumDeclarationSyntax)
            {
                ExportEnum(c as EnumDeclarationSyntax);
            }
            else if(c is DelegateDeclarationSyntax)
            {
                ExportDelegate(c as DelegateDeclarationSyntax);
            }
        }


        static void ExportDelegate(DelegateDeclarationSyntax c)
        {
            if (step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                type.is_abstract = ContainModifier(c.Modifiers, "abstract");
                type.type = (int)Metadata.DB_Type.EType.Delegate;
                bool partial = ContainModifier(c.Modifiers, "partial");
                type.modifier = GetModifier(type, c.Modifiers);
                type.name = c.Identifier.Text;
                type._namespace = CSharpModel.Instance.CurrentNameSpace;
                //type.usingNamespace = new List<string>();
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                //    type.usingNamespace.AddRange(CSharpModel.Instance.usingNamespace);
                //    foreach (var ns in namespaceDeclarationSyntax.Usings)
                //    {
                //        type.usingNamespace.Add(ns.Name.ToString());
                //    }
                //    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                //}



                //泛型
                if (c.TypeParameterList != null)
                {
                    type.is_generic_type_definition = true;
                    foreach (var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                if (partial)
                {
                    CSharpModel.MergeCompilerType(type);
                }
                else
                    CSharpModel.AddCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;

                if (type.static_full_name != "ul.System.Object" && type.base_type.IsVoid)
                    type.base_type = CSharpModel.GetType("ul.System.Delegate").GetRefType();

                //属性
                type.attributes = ExportAttributes(c.AttributeLists);

                CSharpModel.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericSyntax genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                {
                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.name = "Invoke";
                    //dB_Member.is_static = ContainModifier(c.Modifiers, "static");
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                    dB_Member.modifier = GetModifier(type, c.Modifiers);
                    dB_Member.method_virtual = ContainModifier(c.Modifiers, "virtual");
                    dB_Member.method_override = ContainModifier(c.Modifiers, "override");
                    dB_Member.method_abstract = ContainModifier(c.Modifiers, "abstract");
                    dB_Member.method_args = new Metadata.DB_Member.Argument[c.ParameterList.Parameters.Count];
                    for (int i = 0; i < c.ParameterList.Parameters.Count; i++)
                    {
                        dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                        dB_Member.method_args[i].name = c.ParameterList.Parameters[i].Identifier.Text;
                        dB_Member.method_args[i].type = GetTypeSyntax(c.ParameterList.Parameters[i].Type);
                        dB_Member.method_args[i].is_out = ContainModifier(c.ParameterList.Parameters[i].Modifiers, "out");
                        dB_Member.method_args[i].is_ref = ContainModifier(c.ParameterList.Parameters[i].Modifiers, "ref");
                        dB_Member.method_args[i].is_params = ContainModifier(c.ParameterList.Parameters[i].Modifiers, "params");
                    }

                    if (c.TypeParameterList != null)
                    {
                        foreach (var p in c.TypeParameterList.Parameters)
                        {
                            Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                            genericParameterDefinition.type_name = p.Identifier.Text;
                            dB_Member.method_generic_parameter_definitions.Add(genericParameterDefinition);
                        }

                        //泛型参数
                        if (c.ConstraintClauses != null)
                        {
                            foreach (var Constraint in c.ConstraintClauses)
                            {

                                Metadata.GenericSyntax genericParameterDefinition = dB_Member.method_generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                                foreach (var tpc in Constraint.Constraints)
                                {
                                    genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                                }

                            }
                        }
                    }

                    //属性
                    dB_Member.attributes = ExportAttributes(c.AttributeLists);

                    CSharpModel.Instance.EnterMethod(dB_Member);

                    Metadata.Expression.TypeSyntax retType = GetTypeSyntax(c.ReturnType);
                    if (retType != null)
                        dB_Member.type = retType;
                    else
                        dB_Member.type = Metadata.Expression.TypeSyntax.Void;

                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                    //MemberMap[c] = dB_Member;
                    //Console.WriteLine();

                    CSharpModel.Instance.LeaveMethod();
                }


               

                CSharpModel.Instance.LeaveType();
            }
        }

        static void ExportEnum(EnumDeclarationSyntax c)
        {
            if (step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                type.is_abstract = ContainModifier(c.Modifiers, "abstract");
                type.type = (int)Metadata.DB_Type.EType.Value;
                type.modifier = GetModifier(type, c.Modifiers);
                type.name = c.Identifier.Text;
                type._namespace = CSharpModel.Instance.CurrentNameSpace;
                //type.usingNamespace = new List<string>();
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                //    type.usingNamespace.AddRange(CSharpModel.Instance.usingNamespace);
                //    foreach (var ns in namespaceDeclarationSyntax.Usings)
                //    {
                //        type.usingNamespace.Add(ns.Name.ToString());
                //    }
                //    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                //}

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                CSharpModel.AddCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;

                type.base_type = CSharpModel.GetType("ul.System.Enum").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }
                else
                {

                }

                CSharpModel.Instance.EnterType(type);


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<EnumMemberDeclarationSyntax>();
                int order = 0;
                foreach (var v in virableNodes)
                {
                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.name = v.Identifier.Text;
                    dB_Member.is_static = false;
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.EnumMember;
                    //dB_Member.modifier = GetModifier(type, v.Modifiers);
                    //dB_Member.field_type = v_type.GetRefType();
                    //dB_Member.order = order++;
                    //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                }

                CSharpModel.Instance.LeaveType();
                //Console.WriteLine();
            }
        }


        static void ExportInterface(InterfaceDeclarationSyntax c)
        {
            if (step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                //bool isPublic = ContainModifier(c.Modifiers, "public");
                //bool isProtected = ContainModifier(c.Modifiers, "protected");
                //bool isPrivate = !isPublic && !isProtected;
                type.is_abstract = ContainModifier(c.Modifiers, "abstract");
                type.type = (int)Metadata.DB_Type.EType.Inerface;
                //Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;
                type._namespace = CSharpModel.Instance.CurrentNameSpace;
                //type.usingNamespace = new List<string>();
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                //    type.usingNamespace.AddRange(CSharpModel.Instance.usingNamespace);
                //    foreach (var ns in namespaceDeclarationSyntax.Usings)
                //    {
                //        type.usingNamespace.Add(ns.Name.ToString());
                //    }
                //    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                //}

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }
                else
                {

                }

                //泛型
                if (c.TypeParameterList != null)
                {
                    type.is_generic_type_definition = true;
                    foreach (var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                CSharpModel.AddCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;


                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }
                else
                {

                }

                //属性
                type.attributes = ExportAttributes(c.AttributeLists);

                CSharpModel.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericSyntax genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<BaseFieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有属性
                var propertyNodes = c.ChildNodes().OfType<BasePropertyDeclarationSyntax>();
                foreach (var v in propertyNodes)
                {
                    ExportProperty(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
                CSharpModel.Instance.LeaveType();
                //Console.WriteLine();
            }
        }
        static void ExportStruct(StructDeclarationSyntax c)
        {
            if (step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                //bool isPublic = ContainModifier(c.Modifiers, "public");
                //bool isProtected = ContainModifier(c.Modifiers, "protected");
                //bool isPrivate = !isPublic && !isProtected;
                type.is_abstract = ContainModifier(c.Modifiers, "abstract");

                //Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                type.type = (int)Metadata.DB_Type.EType.Value;
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;
                type._namespace = CSharpModel.Instance.CurrentNameSpace;
                //type.usingNamespace = new List<string>();
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                //    type.usingNamespace.AddRange(CSharpModel.Instance.usingNamespace);
                //    foreach (var ns in namespaceDeclarationSyntax.Usings)
                //    {
                //        type.usingNamespace.Add(ns.Name.ToString());
                //    }
                //    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                //}

                //泛型
                if (c.TypeParameterList != null)
                {
                    type.is_generic_type_definition = true;
                    foreach (var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                bool partial = ContainModifier(c.Modifiers, "partial");
                if (!partial)
                    CSharpModel.AddCompilerType(type);
                else
                    CSharpModel.MergeCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;

                type.base_type = CSharpModel.GetType("ul.System.ValueType").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }
                else
                {

                }

                //属性
                type.attributes = ExportAttributes(c.AttributeLists);

                CSharpModel.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericSyntax genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<BaseFieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有属性
                var propertyNodes = c.ChildNodes().OfType<PropertyDeclarationSyntax>();
                foreach (var v in propertyNodes)
                {
                    ExportProperty(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }


                var operatorNodes = c.ChildNodes().OfType<OperatorDeclarationSyntax>();
                foreach (var f in operatorNodes)
                {
                    ExportOperator(f, type);
                }
                var conversion_operatorNodes = c.ChildNodes().OfType<ConversionOperatorDeclarationSyntax>();
                foreach (var f in conversion_operatorNodes)
                {
                    ExportConversionOperator(f, type);
                }

                

                CSharpModel.Instance.LeaveType();
                //Console.WriteLine();
            }
            else if (step == ECompilerStet.Compile)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;
                CSharpModel.Instance.EnterType(type);

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }

                //导出所有属性
                var propertyNodes = c.ChildNodes().OfType<BasePropertyDeclarationSyntax>();
                foreach (var v in propertyNodes)
                {
                    ExportProperty(v, type);
                }

                var operatorNodes = c.ChildNodes().OfType<OperatorDeclarationSyntax>();
                foreach (var f in operatorNodes)
                {
                    ExportOperator(f, type);
                }
                var conversion_operatorNodes = c.ChildNodes().OfType<ConversionOperatorDeclarationSyntax>();
                foreach (var f in conversion_operatorNodes)
                {
                    ExportConversionOperator(f, type);
                }

                CSharpModel.Instance.LeaveType();
            }
        }

        static List<Metadata.DB_AttributeSyntax> ExportAttributes(SyntaxList<AttributeListSyntax> attList )
        {
            List<Metadata.DB_AttributeSyntax> List = new List<Metadata.DB_AttributeSyntax>();
            if (attList == null)
                return List;

            foreach (var a in attList)
            {
                foreach(var att in a.Attributes)
                {
                    Metadata.DB_AttributeSyntax db_att = new Metadata.DB_AttributeSyntax();
                    db_att.TypeName = GetTypeSyntax(att.Name);
                    foreach(var arg in att.ArgumentList.Arguments)
                    {
                        Metadata.DB_AttributeArgumentSyntax db_att_arg = new Metadata.DB_AttributeArgumentSyntax();
                        db_att_arg.name = arg.NameEquals.Name.Identifier.Text;
                        db_att_arg.exp = ExportExp(arg.Expression);
                        db_att.AttributeArgumentList.Add(db_att_arg);
                        
                    }
                    List.Add(db_att);
                }
            }

            return List;
        }

        static void ExportClass(ClassDeclarationSyntax c)
        {
            if(step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                //bool isPublic = ContainModifier(c.Modifiers, "public");
                //bool isProtected = ContainModifier(c.Modifiers, "protected");
                //bool isPrivate = !isPublic && !isProtected;
                type.is_abstract = ContainModifier(c.Modifiers, "abstract");
                type.type = (int)Metadata.DB_Type.EType.Class;
                // Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                bool partial = ContainModifier(c.Modifiers, "partial");
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;
                type._namespace = CSharpModel.Instance.CurrentNameSpace;
                //type.usingNamespace = new List<string>();
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                //    type.usingNamespace.AddRange(CSharpModel.Instance.usingNamespace);
                //    foreach (var ns in namespaceDeclarationSyntax.Usings)
                //    {
                //        type.usingNamespace.Add(ns.Name.ToString());
                //    }
                //    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                //}

                
                
                //泛型
                if(c.TypeParameterList!=null)
                {
                    type.is_generic_type_definition = true;
                    foreach(var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                if(partial)
                {
                    CSharpModel.MergeCompilerType(type);
                }
                else
                    CSharpModel.AddCompilerType(type);
            }
            else if(step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace( namespaceDeclarationSyntax.Name.ToString());
                //}
                if(c.TypeParameterList!=null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;

                if (type.static_full_name != "ul.System.Object" && type.base_type.IsVoid)
                    type.base_type = CSharpModel.GetType("ul.System.Object").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = CSharpModel.GetType(GetTypeSyntax(b.Type));
                        if (dB_Type.is_interface)
                        {
                            type.interfaces.Add(dB_Type.GetRefType());
                        }
                        else
                        {
                            type.base_type = dB_Type.GetRefType();
                        }
                    }
                }
                else
                {

                }

                //属性
                type.attributes = ExportAttributes(c.AttributeLists);


                CSharpModel.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericSyntax genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<BaseFieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有属性
                var propertyNodes = c.ChildNodes().OfType<BasePropertyDeclarationSyntax>();
                foreach (var v in propertyNodes)
                {
                    ExportProperty(v, type);
                }


                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }

                var operatorNodes = c.ChildNodes().OfType<OperatorDeclarationSyntax>();
                foreach (var f in operatorNodes)
                {
                    ExportOperator(f, type);
                }
                var conversion_operatorNodes = c.ChildNodes().OfType<ConversionOperatorDeclarationSyntax>();
                foreach (var f in conversion_operatorNodes)
                {
                    ExportConversionOperator(f, type);
                }

                CSharpModel.Instance.LeaveType();
                //Console.WriteLine();
            }
            else if(step == ECompilerStet.Compile)
            {
                string typeName = c.Identifier.Text;
                //NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                //if (namespaceDeclarationSyntax != null)
                //{
                //    CSharpModel.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                //}
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = CSharpModel.Instance.GetIndifierInfo(typeName).type;
                CSharpModel.Instance.EnterType(type);

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }

                //导出所有属性
                var propertyNodes = c.ChildNodes().OfType<BasePropertyDeclarationSyntax>();
                foreach (var v in propertyNodes)
                {
                    ExportProperty(v, type);
                }

                var operatorNodes = c.ChildNodes().OfType<OperatorDeclarationSyntax>();
                foreach (var f in operatorNodes)
                {
                    ExportOperator(f, type);
                }
                var conversion_operatorNodes = c.ChildNodes().OfType<ConversionOperatorDeclarationSyntax>();
                foreach (var f in conversion_operatorNodes)
                {
                    ExportConversionOperator(f, type);
                }

                CSharpModel.Instance.LeaveType();
            }
        }

       
        //static string GetGenericTypeName(string GenericType)
        //{

        //}

        
        static void ExportVariable(BaseFieldDeclarationSyntax v, Metadata.DB_Type type)
        {
            Metadata.Expression.TypeSyntax v_type = GetTypeSyntax(v.Declaration.Type);

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
                    dB_Member.is_static = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.type = v_type;

                    if (v is FieldDeclarationSyntax)
                        dB_Member.member_type = (int)Metadata.MemberTypes.Field;
                    else if(v is EventFieldDeclarationSyntax)
                    {
                        dB_Member.member_type = (int)Metadata.MemberTypes.Event;
                        //Metadata.Expression.GenericNameSyntax genType = new Metadata.Expression.GenericNameSyntax();
                        //genType.name_space = "System";
                        //genType.Name = "DelegateImpl";
                        //genType.Arguments = new List<Metadata.Expression.TypeSyntax>();
                        //genType.Arguments.Add(v_type.GetRefType());
                        //dB_Member.type = genType;
                    } 
                    else
                    {
                        Console.Error.WriteLine("无法识别的类成员 " + v);
                    }
                    dB_Member.modifier = GetModifier(type,v.Modifiers);
                    if(ve.Initializer!=null)
                        dB_Member.field_initializer = ExportExp(ve.Initializer.Value);

                    dB_Member.attributes = ExportAttributes(v.AttributeLists);
                    //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                }
            }


        }
        static Metadata.DB_Member.Argument GetArgument(ParameterSyntax parameter)
        {
            Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
            arg.name = parameter.Identifier.Text;
            arg.type = GetTypeSyntax(parameter.Type);
            arg.is_out = ContainModifier(parameter.Modifiers, "out");
            arg.is_ref = ContainModifier(parameter.Modifiers, "ref");
            arg.is_params = ContainModifier(parameter.Modifiers, "params");
            return arg;
        }


        static void ExportProperty(BasePropertyDeclarationSyntax v, Metadata.DB_Type type)
        {
            Metadata.DB_Type v_type = CSharpModel.GetType(GetTypeSyntax(v.Type));

            if (v_type == null)
            {
                Console.Error.WriteLine("无法识别的类型 " + v);
                return;
            }

            string name = "";
            if (v is PropertyDeclarationSyntax)
            {
                name = ((PropertyDeclarationSyntax)v).Identifier.Text;
            }
            else if (v is EventDeclarationSyntax)
            {
                name = ((EventDeclarationSyntax)v).Identifier.Text;
            }
            else if(v is IndexerDeclarationSyntax)
            {
                name = "Index";
            }

            if (step == ECompilerStet.ScanMember)
            {
                Metadata.DB_Member property = new Metadata.DB_Member();
                property.name = name; 
                property.declaring_type = type.static_full_name;
                property.member_type = (int)Metadata.MemberTypes.Property;
                property.is_static = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                property.modifier = GetModifier(type, v.Modifiers);
                property.attributes = ExportAttributes(v.AttributeLists);
                property.type = v_type.GetRefType();
                foreach (var ve in v.AccessorList.Accessors)
                {
                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                    dB_Member.is_static = property.is_static;
                    dB_Member.modifier = property.modifier;
                    dB_Member.method_abstract = ContainModifier(v.Modifiers, "abstract");
                    dB_Member.method_virtual = ContainModifier(v.Modifiers, "virtual");
                    dB_Member.method_override = ContainModifier(v.Modifiers, "override");
                    if (ve.Keyword.Text == "get")
                    {
                        dB_Member.type = v_type.GetRefType();
                        dB_Member.name = property.property_get;
                        dB_Member.method_is_property_get = true;
                        if (v is IndexerDeclarationSyntax)
                        {
                            IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
                            List<Metadata.DB_Member.Argument> args = new List<Metadata.DB_Member.Argument>();
                            foreach(var a in indexerDeclarationSyntax.ParameterList.Parameters)
                            {
                                args.Add(GetArgument(a));
                            }
                            dB_Member.method_args = args.ToArray();
                        }
                        else
                        {
                            dB_Member.method_args = new Metadata.DB_Member.Argument[0];
                        }
                        
                    }
                    else if(ve.Keyword.Text == "set")
                    {
                        dB_Member.method_is_property_set = true;
                        dB_Member.name = property.property_set;
                        if (v is IndexerDeclarationSyntax)
                        {
                            IndexerDeclarationSyntax indexerDeclarationSyntax = v as IndexerDeclarationSyntax;
                            List<Metadata.DB_Member.Argument> args = new List<Metadata.DB_Member.Argument>();
                            foreach (var a in indexerDeclarationSyntax.ParameterList.Parameters)
                            {
                                args.Add(GetArgument(a));
                            }
                            Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                            arg.name = "value";
                            arg.type = v_type.GetRefType();
                            args.Add(arg);
                            dB_Member.method_args = args.ToArray();
                        }
                        else
                        {
                            Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                            arg.name = "value";
                            arg.type = v_type.GetRefType();
                            dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                        }
                    }
                    else if(ve.Keyword.Text == "add")
                    {
                        dB_Member.method_is_event_add = true;
                        dB_Member.name = property.property_add;
                        Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                        arg.name = "value";
                        arg.type = v_type.GetRefType();
                        dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                    }
                    else if (ve.Keyword.Text == "remove")
                    {
                        dB_Member.method_is_event_remove = true;
                        dB_Member.name = property.property_remove;
                        Metadata.DB_Member.Argument arg = new Metadata.DB_Member.Argument();
                        arg.name = "value";
                        arg.type = v_type.GetRefType();
                        dB_Member.method_args = new Metadata.DB_Member.Argument[] { arg };
                    }
                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                }
                CSharpModel.AddMember(type.static_full_name, property);
            }
            else if(step == ECompilerStet.Compile)
            {
                Metadata.DB_Member property = new Metadata.DB_Member();
                property.name = name;
                property.is_static = ContainModifier(v.Modifiers, "static") || ContainModifier(v.Modifiers, "const");
                property.modifier = GetModifier(type, v.Modifiers);
                property.attributes = ExportAttributes(v.AttributeLists);
                foreach (var ve in v.AccessorList.Accessors)
                {
                    Metadata.DB_Member dB_Member = null;
                    if (ve.Keyword.Text == "get")
                    {
                        if (v is IndexerDeclarationSyntax)
                        {
                            dB_Member = type.FindMethod(property.property_get, CSharpModel.Instance)[0];
                        }
                        else
                        {
                            dB_Member = type.FindMethod(property.property_get, new List<Metadata.DB_Type>(), CSharpModel.Instance);
                        }
                        CSharpModel.Instance.EnterMethod(dB_Member);
                        if (ve.Body != null)
                            if (!ingore_method_body)
                            {
                                dB_Member.method_body = ExportBody(ve.Body);
                            }
                        CSharpModel.Instance.LeaveMethod();

                    }
                    else if (ve.Keyword.Text == "set")
                    {
                        if (v is IndexerDeclarationSyntax)
                        {
                            dB_Member = type.FindMethod(property.property_set, CSharpModel.Instance)[0];
                        }
                        else
                        {
                            List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                            argTypes.Add(v_type);
                            dB_Member = type.FindMethod(property.property_set, argTypes, CSharpModel.Instance);
                        }

                        CSharpModel.Instance.EnterMethod(dB_Member);
                        if (ve.Body != null)
                            if (!ingore_method_body && ve.Body != null)
                            {
                                dB_Member.method_body = ExportBody(ve.Body);
                            }
                        CSharpModel.Instance.LeaveMethod();
                    }
                    else if (ve.Keyword.Text == "add")
                    {
                        List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                        argTypes.Add(v_type);
                        dB_Member = type.FindMethod(property.property_add, argTypes, CSharpModel.Instance);
                        CSharpModel.Instance.EnterMethod(dB_Member);
                        if (ve.Body != null)
                            if (!ingore_method_body)
                            {
                                dB_Member.method_body = ExportBody(ve.Body);
                            }
                        CSharpModel.Instance.LeaveMethod();
                    }
                    else if (ve.Keyword.Text == "remove")
                    {
                        List<Metadata.DB_Type> argTypes = new List<Metadata.DB_Type>();
                        argTypes.Add(v_type);
                        dB_Member = type.FindMethod(property.property_remove, argTypes, CSharpModel.Instance);
                        CSharpModel.Instance.EnterMethod(dB_Member);
                        if (ve.Body != null)
                            if (!ingore_method_body && ve.Body != null)
                            {
                                dB_Member.method_body = ExportBody(ve.Body);
                            }
                        CSharpModel.Instance.LeaveMethod();
                    }

                }
            }
        }
        


        static Dictionary<BaseMethodDeclarationSyntax, Metadata.DB_Member> MemberMap = new Dictionary<BaseMethodDeclarationSyntax, Metadata.DB_Member>();
        static void ExportMethod(BaseMethodDeclarationSyntax v, Metadata.DB_Type type)
        {
            if(v is MethodDeclarationSyntax)
            {
                MethodDeclarationSyntax f = v as MethodDeclarationSyntax;
                if (step == ECompilerStet.ScanMember)
                {
                    //Console.WriteLine("\tIdentifier:" + f.Identifier);
                    //Console.WriteLine("\tModifiers:" + f.Modifiers);
                    //Console.WriteLine("\tReturnType:" + f.ReturnType);
                    //TypeInfo ti = GetTypeInfo(f.ReturnType);



                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.name = f.Identifier.Text;
                    dB_Member.is_static = ContainModifier(f.Modifiers, "static");
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                    dB_Member.modifier = GetModifier(type,f.Modifiers);
                    dB_Member.method_virtual = ContainModifier(f.Modifiers, "virtual");
                    dB_Member.method_override = ContainModifier(f.Modifiers, "override");
                    dB_Member.method_abstract = ContainModifier(f.Modifiers, "abstract");
                    dB_Member.method_args = new Metadata.DB_Member.Argument[f.ParameterList.Parameters.Count];
                    for (int i = 0; i < f.ParameterList.Parameters.Count; i++)
                    {
                        dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                        dB_Member.method_args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                        dB_Member.method_args[i].type = GetTypeSyntax(f.ParameterList.Parameters[i].Type);
                        dB_Member.method_args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                        dB_Member.method_args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
                        dB_Member.method_args[i].is_params = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "params");
                    }

                    if(f.TypeParameterList != null)
                    {
                        foreach (var p in f.TypeParameterList.Parameters)
                        {
                            Metadata.GenericSyntax genericParameterDefinition = new Metadata.GenericSyntax();
                            genericParameterDefinition.type_name = p.Identifier.Text;
                            dB_Member.method_generic_parameter_definitions.Add(genericParameterDefinition);
                        }

                        //泛型参数
                        if (f.ConstraintClauses != null)
                        {
                            foreach (var Constraint in f.ConstraintClauses)
                            {

                                Metadata.GenericSyntax genericParameterDefinition = dB_Member.method_generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                                foreach (var tpc in Constraint.Constraints)
                                {
                                    genericParameterDefinition.constraint = (GetTypeParameterSyntax(tpc));
                                }

                            }
                        }
                    }

                    //属性
                    dB_Member.attributes = ExportAttributes(v.AttributeLists);

                    CSharpModel.Instance.EnterMethod(dB_Member);

                    Metadata.Expression.TypeSyntax retType = GetTypeSyntax(f.ReturnType);
                    if (retType != null)
                        dB_Member.type = retType;
                    else
                        dB_Member.type = Metadata.Expression.TypeSyntax.Void;

                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                    MemberMap[f] = dB_Member;
                    //Console.WriteLine();

                    CSharpModel.Instance.LeaveMethod();
                }
                else if (step == ECompilerStet.Compile)
                {
                    Metadata.DB_Member dB_Member = MemberMap[f];
                    CSharpModel.Instance.EnterMethod(dB_Member);
                    if (f.Body != null)
                        if (!ingore_method_body && f.Body!=null)
                        {
                            dB_Member.method_body = ExportBody(f.Body);
                        }
                    CSharpModel.Instance.LeaveMethod();
                }
            }
            if (v is ConstructorDeclarationSyntax)
            {
                ConstructorDeclarationSyntax f = v as ConstructorDeclarationSyntax;
                if (step == ECompilerStet.ScanMember)
                {
                    //Console.WriteLine("\tIdentifier:" + f.Identifier);
                    //Console.WriteLine("\tModifiers:" + f.Modifiers);
                    //Console.WriteLine("\tReturnType:" + f.ReturnType);
                    //TypeInfo ti = GetTypeInfo(f.ReturnType);



                    Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                    dB_Member.name = f.Identifier.Text;
                    dB_Member.is_static = ContainModifier(f.Modifiers, "static");
                    dB_Member.declaring_type = type.static_full_name;
                    dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                    dB_Member.method_is_constructor = true;
                    dB_Member.modifier = GetModifier(type,f.Modifiers);

                    dB_Member.method_args = new Metadata.DB_Member.Argument[f.ParameterList.Parameters.Count];
                    for (int i = 0; i < f.ParameterList.Parameters.Count; i++)
                    {
                        dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                        dB_Member.method_args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                        dB_Member.method_args[i].type = GetTypeSyntax(f.ParameterList.Parameters[i].Type);
                        dB_Member.method_args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                        dB_Member.method_args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
                    }

                    //Metadata.DB_Type retType = GetType(f.ReturnType);
                    //if (retType != null)
                    //    dB_Member.method_ret_type = retType.GetRefType();
                    //else
                    dB_Member.type = Metadata.Expression.TypeSyntax.Void;

                    CSharpModel.AddMember(type.static_full_name, dB_Member);
                    MemberMap[f] = dB_Member;
                    //Console.WriteLine();
                }
                else if (step == ECompilerStet.Compile)
                {
                    if(!ingore_method_body && f.Body!=null)
                    {
                        Metadata.DB_Member dB_Member = MemberMap[f];
                        dB_Member.method_body = ExportBody(f.Body);
                    }
                }
            }
        }


        static void ExportOperator(OperatorDeclarationSyntax f, Metadata.DB_Type type)
        {
            if (step == ECompilerStet.ScanMember)
            {

                Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                dB_Member.name = f.OperatorToken.Text;
                dB_Member.is_static = ContainModifier(f.Modifiers, "static");
                dB_Member.method_is_operator = true;
                dB_Member.declaring_type = type.static_full_name;
                dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                dB_Member.modifier = GetModifier(type, f.Modifiers);
                dB_Member.method_virtual = ContainModifier(f.Modifiers, "virtual");
                dB_Member.method_override = ContainModifier(f.Modifiers, "override");
                dB_Member.method_abstract = ContainModifier(f.Modifiers, "abstract");
                dB_Member.method_args = new Metadata.DB_Member.Argument[f.ParameterList.Parameters.Count];
                for (int i = 0; i < f.ParameterList.Parameters.Count; i++)
                {
                    dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                    dB_Member.method_args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                    dB_Member.method_args[i].type = GetTypeSyntax(f.ParameterList.Parameters[i].Type);
                    dB_Member.method_args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                    dB_Member.method_args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
                    dB_Member.method_args[i].is_params = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "params");
                }

                //属性
                dB_Member.attributes = ExportAttributes(f.AttributeLists);

                CSharpModel.Instance.EnterMethod(dB_Member);

                Metadata.Expression.TypeSyntax retType = GetTypeSyntax(f.ReturnType);
                if (retType != null)
                    dB_Member.type = retType;
                else
                    dB_Member.type = Metadata.Expression.TypeSyntax.Void;

                CSharpModel.AddMember(type.static_full_name, dB_Member);
                MemberMap[f] = dB_Member;
                //Console.WriteLine();

                CSharpModel.Instance.LeaveMethod();
            }
            else if (step == ECompilerStet.Compile)
            {
                Metadata.DB_Member dB_Member = MemberMap[f];
                CSharpModel.Instance.EnterMethod(dB_Member);
                if (f.Body != null)
                    if (!ingore_method_body && f.Body != null)
                    {
                        dB_Member.method_body = ExportBody(f.Body);
                    }
                CSharpModel.Instance.LeaveMethod();
            }
        }

        static void ExportConversionOperator(ConversionOperatorDeclarationSyntax f, Metadata.DB_Type type)
        {
            if (step == ECompilerStet.ScanMember)
            {

                Metadata.DB_Member dB_Member = new Metadata.DB_Member();
                
                dB_Member.is_static = ContainModifier(f.Modifiers, "static");
                dB_Member.method_is_conversion_operator = true;
                dB_Member.declaring_type = type.static_full_name;
                dB_Member.member_type = (int)Metadata.MemberTypes.Method;
                dB_Member.modifier = GetModifier(type, f.Modifiers);
                dB_Member.method_virtual = ContainModifier(f.Modifiers, "virtual");
                dB_Member.method_override = ContainModifier(f.Modifiers, "override");
                dB_Member.method_abstract = ContainModifier(f.Modifiers, "abstract");
                dB_Member.method_args = new Metadata.DB_Member.Argument[f.ParameterList.Parameters.Count];
                for (int i = 0; i < f.ParameterList.Parameters.Count; i++)
                {
                    dB_Member.method_args[i] = new Metadata.DB_Member.Argument();
                    dB_Member.method_args[i].name = f.ParameterList.Parameters[i].Identifier.Text;
                    dB_Member.method_args[i].type = GetTypeSyntax(f.ParameterList.Parameters[i].Type);
                    dB_Member.method_args[i].is_out = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "out");
                    dB_Member.method_args[i].is_ref = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "ref");
                    dB_Member.method_args[i].is_params = ContainModifier(f.ParameterList.Parameters[i].Modifiers, "params");
                }

                //属性
                dB_Member.attributes = ExportAttributes(f.AttributeLists);

                CSharpModel.Instance.EnterMethod(dB_Member);

                Metadata.Expression.TypeSyntax retType = GetTypeSyntax(f.Type);
                if (retType != null)
                    dB_Member.type = retType;
                else
                    dB_Member.type = Metadata.Expression.TypeSyntax.Void;
                dB_Member.name = dB_Member.type.Name;

                CSharpModel.AddMember(type.static_full_name, dB_Member);
                MemberMap[f] = dB_Member;
                //Console.WriteLine();

                CSharpModel.Instance.LeaveMethod();
            }
            else if (step == ECompilerStet.Compile)
            {
                Metadata.DB_Member dB_Member = MemberMap[f];
                CSharpModel.Instance.EnterMethod(dB_Member);
                if (f.Body != null)
                    if (!ingore_method_body && f.Body != null)
                    {
                        dB_Member.method_body = ExportBody(f.Body);
                    }
                CSharpModel.Instance.LeaveMethod();
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
            else if (node is BlockSyntax)
            {
                return ExportStatement(node as BlockSyntax);
            }
            else if (node is LocalDeclarationStatementSyntax)
            {
                return ExportStatement(node as LocalDeclarationStatementSyntax);
            }
            else if (node is ForStatementSyntax)
            {
                return ExportStatement(node as ForStatementSyntax);
            }
            else if(node is DoStatementSyntax)
            {
                return ExportStatement(node as DoStatementSyntax);
            }
            else if(node is WhileStatementSyntax)
            {
                return ExportStatement(node as WhileStatementSyntax);
            }
            else if(node is SwitchStatementSyntax)
            {
                return ExportStatement(node as SwitchStatementSyntax);
            }
            else if(node is BreakStatementSyntax)
            {
                return ExportStatement(node as BreakStatementSyntax);
            }
            else if(node is ReturnStatementSyntax)
            {
                return ExportStatement(node as ReturnStatementSyntax);
            }
            else if(node is TryStatementSyntax)
            {
                return ExportStatement(node as TryStatementSyntax);
            }
            else if(node is ThrowStatementSyntax)
            {
                return ExportStatement(node as ThrowStatementSyntax);
            }
            else
            {
                Console.Error.WriteLine("error:Unsopproted StatementSyntax" + node);
            }
            return null;
        }

        static Metadata.DB_StatementSyntax ExportStatement(TryStatementSyntax bs)
        {
            Metadata.DB_TryStatementSyntax ss = new Metadata.DB_TryStatementSyntax();
            ss.Block = ExportStatement(bs.Block) as Metadata.DB_BlockSyntax;
            if(bs.Catches!=null)
            {
                ss.Catches = new List<Metadata.CatchClauseSyntax>();
                foreach (var s in bs.Catches)
                {
                    Metadata.CatchClauseSyntax ccs = new Metadata.CatchClauseSyntax();
                    ccs.Type = GetTypeSyntax(s.Declaration.Type);
                    ccs.Identifier = s.Declaration.Identifier.Text;
                    ccs.Block = ExportStatement(s.Block) as Metadata.DB_BlockSyntax;
                    ss.Catches.Add(ccs);
                }
            }
            if(bs.Finally!=null)
            {
                ss.Finally = new Metadata.FinallyClauseSyntax();
                ss.Finally.Block = ExportStatement(bs.Finally.Block) as Metadata.DB_BlockSyntax;
            }
            return ss;
        }

        static Metadata.DB_StatementSyntax ExportStatement(ThrowStatementSyntax bs)
        {
            Metadata.DB_ThrowStatementSyntax ss = new Metadata.DB_ThrowStatementSyntax();
            ss.Expression = ExportExp(bs.Expression);
            return ss;
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
            db_ss.Declaration.Type = GetTypeSyntax(ss.Declaration.Type);
            foreach(var v in ss.Declaration.Variables)
            {
                db_ss.Declaration.Variables.Add(ExportExp(v));
            }
            return db_ss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(ForStatementSyntax ss)
        {
            Metadata.DB_ForStatementSyntax db_ss = new Metadata.DB_ForStatementSyntax();
            db_ss.Condition = ExportExp(ss.Condition);
            db_ss.Declaration = ExportExp(ss.Declaration);
            foreach(var inc in ss.Incrementors)
            {
                db_ss.Incrementors.Add(ExportExp(inc));
            }
            db_ss.Statement = ExportStatement(ss.Statement);

            return db_ss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(DoStatementSyntax ss)
        {
            Metadata.DB_DoStatementSyntax db_ss = new Metadata.DB_DoStatementSyntax();
            db_ss.Condition = ExportExp(ss.Condition);
            db_ss.Statement = ExportStatement(ss.Statement);

            return db_ss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(WhileStatementSyntax ss)
        {
            Metadata.DB_WhileStatementSyntax db_ss = new Metadata.DB_WhileStatementSyntax();
            db_ss.Condition = ExportExp(ss.Condition);
            db_ss.Statement = ExportStatement(ss.Statement);

            return db_ss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(SwitchStatementSyntax ss)
        {
            Metadata.DB_SwitchStatementSyntax db_ss = new Metadata.DB_SwitchStatementSyntax();
            db_ss.Expression = ExportExp(ss.Expression);
            foreach(var s in ss.Sections)
            {
                db_ss.Sections.Add(ExportSwitchSection(s));
            }

            return db_ss;
        }
        
        static Metadata.DB_SwitchStatementSyntax.SwitchSectionSyntax ExportSwitchSection(SwitchSectionSyntax sss)
        {
            Metadata.DB_SwitchStatementSyntax.SwitchSectionSyntax db_sss = new Metadata.DB_SwitchStatementSyntax.SwitchSectionSyntax();
            foreach(var l in sss.Labels)
            {
                if(l is CaseSwitchLabelSyntax)
                {
                    CaseSwitchLabelSyntax csls = l as CaseSwitchLabelSyntax;
                    db_sss.Labels.Add(ExportExp(csls.Value));
                }
            }
            
            foreach(var s in sss.Statements)
            {
                Metadata.DB_StatementSyntax ss = ExportStatement(s);
                if(ss!=null)
                    db_sss.Statements.Add(ss);
            }

            return db_sss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(BreakStatementSyntax ss)
        {
            Metadata.DB_BreakStatementSyntax db_ss = new Metadata.DB_BreakStatementSyntax();
            return db_ss;
        }
        static Metadata.DB_StatementSyntax ExportStatement(ReturnStatementSyntax ss)
        {
            Metadata.DB_ReturnStatementSyntax db_ss = new Metadata.DB_ReturnStatementSyntax();
            db_ss.Expression = ExportExp(ss.Expression);
            return db_ss;
        }



        static Metadata.Expression.Exp ExportExp(ExpressionSyntax es)
        {
            if(es is LiteralExpressionSyntax)
            {
                return ExportExp(es as LiteralExpressionSyntax);
            }
            else if (es is ThisExpressionSyntax)
            {
                return ExportExp(es as ThisExpressionSyntax);
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
            else if (es is IdentifierNameSyntax)
            {
                return ExportExp(es as IdentifierNameSyntax);
            }
            else if(es is AssignmentExpressionSyntax)
            {
                return ExportExp(es as AssignmentExpressionSyntax);
            }
            else if (es is BinaryExpressionSyntax)
            {
                return ExportExp(es as BinaryExpressionSyntax);
            }
            else if(es is PostfixUnaryExpressionSyntax)
            {
                return ExportExp(es as PostfixUnaryExpressionSyntax);
            }
            else if (es is ArrayCreationExpressionSyntax)
            {
                return ExportExp(es as ArrayCreationExpressionSyntax);
            }
            else if(es is PrefixUnaryExpressionSyntax)
            {
                return ExportExp(es as PrefixUnaryExpressionSyntax);
            }
            else if(es is BaseExpressionSyntax)
            {
                return ExportExp(es as BaseExpressionSyntax);
            }
            else if(es is ThrowExpressionSyntax)
            {
                return ExportExp(es as ThrowExpressionSyntax);
            }
            else if(es is ParenthesizedExpressionSyntax)
            {
                return ExportExp(es as ParenthesizedExpressionSyntax);
            }
            else if(es is ElementAccessExpressionSyntax)
            {
                return ExportExp(es as ElementAccessExpressionSyntax);
            }
            else
            {
                Console.Error.WriteLine(string.Format("error:不支持的表达式 {0} {1}" , es.GetType().Name,es.ToString()));
            }
            return null;
        }
        static Metadata.Expression.Exp ExportExp(IdentifierNameSyntax es)
        {
            string name = es.Identifier.Text;
            Metadata.Model.IndifierInfo info = CSharpModel.Instance.GetIndifierInfo(name,"", Metadata.Model.EIndifierFlag.IF_Type);
            if(info!=null && info.is_type)
            {
                Metadata.Expression.IndifierExp db_les = new Metadata.Expression.IndifierExp();
                db_les.Name = info.type.static_full_name;
                return db_les;
            }
            else
            {
                Metadata.Expression.IndifierExp db_les = new Metadata.Expression.IndifierExp();
                db_les.Name = es.Identifier.Text;
                return db_les;
            }
        }
        static Metadata.Expression.Exp ExportExp(LiteralExpressionSyntax es)
        {
            Metadata.Expression.ConstExp db_les = new Metadata.Expression.ConstExp();
            db_les.value = es.Token.Text;
            return db_les;
        }
        static Metadata.Expression.Exp ExportExp(MemberAccessExpressionSyntax es)
        {
            Metadata.Expression.FieldExp db_les = new Metadata.Expression.FieldExp();
            db_les.Caller = ExportExp(es.Expression);
            db_les.Name = es.Name.Identifier.Text;
            return db_les;
        }
        static Metadata.Expression.Exp ExportExp(InvocationExpressionSyntax es)
        {
            Metadata.Expression.MethodExp db_les = new Metadata.Expression.MethodExp();
            //if (es.Expression is MemberAccessExpressionSyntax)
            //{
            //    MemberAccessExpressionSyntax maes = es.Expression as MemberAccessExpressionSyntax;
            //    db_les.Name = (maes).Name.Identifier.Text;
            //    db_les.Caller = ExportExp(maes.Expression);
            //}
            //else if (es.Expression is IdentifierNameSyntax)
            //{
            //    IdentifierNameSyntax nameSyntax = es.Expression as IdentifierNameSyntax;
            //    db_les.Name = nameSyntax.Identifier.Text;
            //    db_les.Caller = new Metadata.Expression.ThisExp();
            //}
            //else
            //{
            //    Console.Error.WriteLine("不支持的方法调用表达式 " + es.ToString());
            //}
            
            db_les.Caller = ExportExp(es.Expression);

            foreach (var a in es.ArgumentList.Arguments)
            {
                db_les.Args.Add(ExportExp(a.Expression));
            }
            return db_les;
        }
        //static Metadata.Expression.Exp ExportExp(ArgumentSyntax es)
        //{
        //    Metadata.Expression.E db_les = new Metadata.DB_ArgumentSyntax();
        //    db_les.Expression = ExportExp(es.Expression);
        //    return db_les;
        //}
        //static Metadata.Expression.Exp ExportExp(InitializerExpressionSyntax es)
        //{
        //    Metadata.Expression.MethodExp db_les = new Metadata.Expression.MethodExp();
        //    if(es.Expressions!=null)
        //    {
        //        foreach(var e in es.Expressions)
        //        {
        //            db_les.Expressions.Add(ExportExp(e));
        //        }
        //    }
        //    return db_les;
        //}

        static Metadata.Expression.Exp ExportExp(ObjectCreationExpressionSyntax es)
        {
            Metadata.Expression.ObjectCreateExp db_les = new Metadata.Expression.ObjectCreateExp();
            //if(es.Initializer!=null)
            //    db_les.Initializer = ExportExp(es.Initializer) as Metadata.DB_InitializerExpressionSyntax;

            if(es.ArgumentList!=null)
            {
                foreach (var a in es.ArgumentList.Arguments)
                {
                    db_les.Args.Add(ExportExp(a.Expression));
                }
            }

            db_les.Type = GetTypeSyntax(es.Type);
            return db_les;
        }
        static Metadata.VariableDeclaratorSyntax ExportExp(VariableDeclaratorSyntax es)
        {
            Metadata.VariableDeclaratorSyntax db_les = new Metadata.VariableDeclaratorSyntax();
            db_les.Identifier = es.Identifier.Text;
            if(es.Initializer!=null)
                db_les.Initializer = ExportExp(es.Initializer.Value);
            return db_les;
        }
        static Metadata.VariableDeclarationSyntax ExportExp(VariableDeclarationSyntax es)
        {
            Metadata.VariableDeclarationSyntax db_les = new Metadata.VariableDeclarationSyntax();
            db_les.Type = GetTypeSyntax(es.Type);
            foreach(var v in es.Variables)
            {
                db_les.Variables.Add(ExportExp(v));
            }
            return db_les;
        }

        static Metadata.Expression.Exp ExportExp(AssignmentExpressionSyntax es)
        {
            Metadata.Expression.AssignmentExpressionSyntax db_les = new Metadata.Expression.AssignmentExpressionSyntax();
            db_les.Left = ExportExp(es.Left);
            db_les.OperatorToken = es.OperatorToken.Text;
            db_les.Right = ExportExp(es.Right);
            return db_les;
        }
        static Metadata.Expression.Exp ExportExp(BinaryExpressionSyntax es)
        {
            Metadata.Expression.BinaryExpressionSyntax db_les = new Metadata.Expression.BinaryExpressionSyntax();
            db_les.Left = ExportExp(es.Left);
            db_les.Right = ExportExp(es.Right);
            db_les.OperatorToken = es.OperatorToken.Text;
            return db_les;
        }
        static Metadata.Expression.Exp ExportExp(PostfixUnaryExpressionSyntax es)
        {

            Metadata.Expression.PostfixUnaryExpressionSyntax ss= new Metadata.Expression.PostfixUnaryExpressionSyntax();
            ss.Operand = ExportExp(es.Operand);
            ss.OperatorToken = es.OperatorToken.Text;
            return ss;
            //Metadata.Expression.MethodExp db_Add = new Metadata.Expression.MethodExp();
            //if (es.OperatorToken.Text == "++")
            //{
            //    db_Add.Name = "op_PlusPlus";
            //}
            //else if(es.OperatorToken.Text == "--")
            //{
            //    db_Add.Name = "op_SubSub";
            //}
            //else
            //{
            //    Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
            //}
            //db_Add.Caller = ExportExp(es.Operand);


            
            ////db_Add.Caller = op_Token;
            //db_Add.Args.Add(new Metadata.Expression.ConstExp() { value = "1" });

            //Metadata.Expression.MethodExp db_les = new Metadata.Expression.MethodExp();
            //db_les.Caller = db_Add;
            //db_les.Name = "op_Assign";
            //db_les.Args.Add(db_Add);
            //return db_les;
        }

        static Metadata.Expression.Exp ExportExp(ArrayCreationExpressionSyntax es)
        {
            Metadata.Expression.ObjectCreateExp db_les = new Metadata.Expression.ObjectCreateExp();
            //if(es.Initializer!=null)
            //    db_les.Initializer = ExportExp(es.Initializer) as Metadata.DB_InitializerExpressionSyntax;

            //db_les.Args.Add(ExportExp(es.Expression));

            db_les.Type = GetTypeSyntax(es.Type);
            foreach(var p in es.Type.RankSpecifiers)
            {
                foreach(var s in p.Sizes)
                    db_les.Args.Add(ExportExp(s));
            }
            return db_les;
        }
        static Metadata.Expression.Exp ExportExp(PrefixUnaryExpressionSyntax es)
        {
            Metadata.Expression.PrefixUnaryExpressionSyntax ss = new Metadata.Expression.PrefixUnaryExpressionSyntax();
            ss.Operand = ExportExp(es.Operand);
            ss.OperatorToken = es.OperatorToken.Text;
            return ss;
            //if(es.Operand is LiteralExpressionSyntax)
            //{
            //    Metadata.Expression.ConstExp exp = ExportExp(es.Operand) as Metadata.Expression.ConstExp;
            //    if(es.OperatorToken.Text == "-")
            //        exp.value = "-" + exp.value;
            //    else
            //        Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
            //    return exp;
            //}

            //{
            //    Metadata.Expression.MethodExp exp = new Metadata.Expression.MethodExp();
            //    if (es.OperatorToken.Text == "-")
            //        exp.Name = "op_Invert";
            //    else
            //        Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
            //    exp.Caller = ExportExp(es.Operand);
            //    return exp;
            //}
        }

        static Metadata.Expression.Exp ExportExp(BaseExpressionSyntax es)
        {
            Metadata.Expression.BaseExp exp = new Metadata.Expression.BaseExp();
            return exp;
        }

        static Metadata.Expression.Exp ExportExp(ThisExpressionSyntax es)
        {
            Metadata.Expression.ThisExp exp = new Metadata.Expression.ThisExp();
            return exp;
        }
        static Metadata.Expression.Exp ExportExp(ThrowExpressionSyntax es)
        {
            Metadata.Expression.ThrowExp throwExp = new Metadata.Expression.ThrowExp();
            throwExp.exp = ExportExp(es.Expression);
            return throwExp;
        }

        static Metadata.Expression.Exp ExportExp(ParenthesizedExpressionSyntax es)
        {
            Metadata.Expression.ParenthesizedExpressionSyntax exp = new Metadata.Expression.ParenthesizedExpressionSyntax();
            exp.exp = ExportExp(es.Expression);
            return exp;
        }

        static Metadata.Expression.Exp ExportExp(ElementAccessExpressionSyntax es)
        {
            Metadata.Expression.ElementAccessExp exp = new Metadata.Expression.ElementAccessExp();
            exp.exp = ExportExp(es.Expression);
            foreach(var a in es.ArgumentList.Arguments)
            {
                exp.args.Add(ExportExp(a.Expression));
            }

            return exp;
        }
    }
}
