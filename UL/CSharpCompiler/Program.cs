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
            return Model.GetType(full_name);
        }
        //查找一个类型，如果是动态类型，构造一个
        public Metadata.DB_Type FindType(Metadata.Expression.TypeSyntax refType)
        {
            return Model.GetType(refType);
        }
    }

    class Model
    {
        public static Metadata.Model Instance = new Metadata.Model(new Finder());
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
                Metadata.DB_Type type = Metadata.DB.LoadType(full_name,Program._con);
                if(type!=null)
                {
                    AddRefType(type);
                }
                return type;
            }
            return null;
        }


        public static Metadata.DB_Type GetType(Metadata.Expression.TypeSyntax typeSyntax)
        {
            if(typeSyntax is Metadata.Expression.IdentifierNameSyntax)
            {
                Metadata.Expression.IdentifierNameSyntax ins = typeSyntax as Metadata.Expression.IdentifierNameSyntax;
                return GetType(ins.GetStaticFullName());
            }

            if(typeSyntax is Metadata.Expression.GenericNameSyntax)
            {
                Metadata.Expression.GenericNameSyntax ins = typeSyntax as Metadata.Expression.GenericNameSyntax;
                Metadata.DB_Type ma = GetType(ins.GetStaticFullName());
                return Metadata.DB_Type.MakeGenericType(ma, ins.Arguments);
            }

            if(typeSyntax is Metadata.Expression.GenericParameterSyntax)
            {
                Metadata.Expression.GenericParameterSyntax gps = typeSyntax as Metadata.Expression.GenericParameterSyntax;
                return Metadata.DB_Type.MakeGenericParameterType(GetType(gps.declare_type), new Metadata.GenericParameterDefinition() { type_name = gps.Name });
            }

            return null;
        }

        //public static Metadata.DB_Type FindType(string name)
        //{
        //    //查找本地变量
        //    foreach (var v in stackLocalVariables)
        //    {
        //        if (v.ContainsKey(name))
        //            return v[name];
        //    }

        //    //泛型参数
        //    if(currentMethod!=null)
        //    {
        //        foreach (var gd in currentMethod.method_generic_parameter_definitions)
        //        {
        //            if (gd.type_name == name)
        //            {
        //                return Metadata.DB_Type.MakeGenericParameterType(currentType, gd);
        //            }
        //        }
        //    }

        //    //查找成员变量
        //    if (currentType != null)
        //    {
        //        if (currentType.FindField(name,this))
        //            return GetType(currentType.FindField(name, this).typeName.ToString());
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

        //        //当前命名空间
        //        {
        //            Dictionary<string, Metadata.DB_Type> ns = FindNamespace(currentType._namespace);
        //            if (ns.ContainsKey(name))
        //            {
        //                return ns[name];
        //            }
        //        }

        //        //命名空间查找
        //        foreach (var nsName in currentType.usingNamespace)
        //        {
        //            Dictionary<string, Metadata.DB_Type> ns = FindNamespace(nsName);
        //            if (ns.ContainsKey(name))
        //            {
        //                return ns[name];
        //            }
        //        }
        //    }

        //    //命名空间查找
        //    foreach (var nsName in usingNamespace)
        //    {
        //        Dictionary<string, Metadata.DB_Type> ns = FindNamespace(nsName);
        //        if (ns.ContainsKey(name))
        //        {
        //            return ns[name];
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(currentUsing))
        //    {
        //        Dictionary<string, Metadata.DB_Type> ns = FindNamespace(currentUsing);
        //        if (ns.ContainsKey(name))
        //        {
        //            return ns[name];
        //        }
        //    }

        //    return null;
        //}


        //public static void EnterNS(string ns)
        //{
        //    currentUsing = ns;
        //}

        //public static void LeaveNS()
        //{
        //    currentUsing = null;
        //}

        //public static void EnterType(Metadata.DB_Type type)
        //{
        //    currentType = type;
        //}
        //public static void LeaveType()
        //{
        //    currentType = null;
        //}

        //public static void EnterBlock()
        //{
        //    stackLocalVariables.Push(new Dictionary<string, Metadata.DB_Type>());
        //}

        //public static void LeaveBlock()
        //{
        //    stackLocalVariables.Pop();
        //}
        //public static void EnterMethod(Metadata.DB_Member member)
        //{
        //    currentMethod = member;
        //}
        //public static void LeaveMethod()
        //{
        //    currentMethod = null;
        //}



        //public static void AddLocal(string name,Metadata.DB_Type type)
        //{
        //    stackLocalVariables.Peek().Add(name, type);
        //}
    }

    class Program
    {

        public static OdbcConnection _con;
        static OdbcTransaction _trans;

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
                return new Metadata.Expression.IdentifierNameSyntax() { Name = "Object", name_space = "System" };
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
                Metadata.Expression.IdentifierNameSyntax Right = new Metadata.Expression.IdentifierNameSyntax() { Name = typeName };
                Right.name_space = "System";
                return Right;
            }
            else if (typeSyntax is ArrayTypeSyntax)
            {
                ArrayTypeSyntax ts = typeSyntax as ArrayTypeSyntax;
                Metadata.Expression.TypeSyntax elementType = GetTypeSyntax(ts.ElementType,"");
                List<Metadata.Expression.TypeSyntax> parameters = new List<Metadata.Expression.TypeSyntax>();
                parameters.Add(elementType);
                Metadata.Expression.GenericNameSyntax gns = new Metadata.Expression.GenericNameSyntax();
                gns.Name = "Array";
                gns.Arguments = parameters;
                //Metadata.Expression.QualifiedNameSyntax qns = new Metadata.Expression.QualifiedNameSyntax();
                //qns.Left = new Metadata.Expression.IdentifierNameSyntax() { Identifier = "System" };
                //qns.Right = gns;
                gns.name_space = "System";
                Metadata.DB_Type arrayType = Model.GetType("System.Array[1]");
                return gns;
            }
            else if (typeSyntax is IdentifierNameSyntax)
            {
                IdentifierNameSyntax ts = typeSyntax as IdentifierNameSyntax;
                Metadata.DB_Type type = null;
                if (!string.IsNullOrEmpty(ns))
                {
                    type = Model.Instance.GetIndifierInfo(ts.Identifier.Text,ns).type;
                }
                else
                {
                    type = Model.Instance.GetIndifierInfo(ts.Identifier.Text).type;
                }

                //Metadata.DB_Type type = Model.Instance.GetIndifierInfo(Identifier).type;
                //Model.Instance.GetIndifierInfo(ts.Identifier.Text).type;
                if (type.is_generic_paramter)
                {
                    Metadata.Expression.GenericParameterSyntax ins = new Metadata.Expression.GenericParameterSyntax();

                    ins.Name = (ts.Identifier.Text);
                    ins.name_space = type._namespace;
                    ins.declare_type = type.static_full_name;
                    return ins;
                }
                else
                {
                    Metadata.Expression.IdentifierNameSyntax ins = new Metadata.Expression.IdentifierNameSyntax();

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
                Metadata.Expression.GenericNameSyntax gns = new Metadata.Expression.GenericNameSyntax();
                gns.Name = Name;
                gns.Arguments = parameters;
                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(gns.GetUniqueName()).type;
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

        class ULProject
        {
            public string name;
            //public string[] files;
            public string[] ref_namespace;
            public string[] ref_type;
            public string[] dirs;
            public bool ingore_method_body;
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
            List<string> arg_list = new List<string>(args);
            string project = System.IO.File.ReadAllText(args[0]);
            
            ULProject pj = Metadata.DB.ReadObject<ULProject>(project);
            ingore_method_body = pj.ingore_method_body;
            string pj_dir = System.IO.Path.GetFullPath(args[0]);
            pj_dir = pj_dir.Substring(0, pj_dir.Length- System.IO.Path.GetFileName(pj_dir).Length-1);

            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
            {
                con.Open();
                _con = con;


                //加载引用
                //foreach (var ref_ns in pj.ref_namespace)
                //{
                //    Dictionary<string, Metadata.DB_Type> ns_types = Metadata.DB.LoadNamespace(ref_ns, _con);
                //    foreach (var v in ns_types)
                //    {
                //        Model.AddRefType(v.Value);
                //    }
                //}

                //foreach (var ref_ns in pj.ref_type)
                //{
                //    Model.AddRefType(Metadata.DB.LoadType(ref_ns, _con));
                //}

                //分析文件
                List<SyntaxTree> treeList = new List<SyntaxTree>();
                
                foreach (var dir in pj.dirs)
                {
                    string[] files = Directory.GetFiles(System.IO.Path.Combine(pj_dir, dir),"*.cs");
                    foreach (var file in files)
                    {
                        string file_full_path = file;// System.IO.Path.Combine(pj_dir, file);
                        string code = System.IO.File.ReadAllText(file_full_path);
                        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                        treeList.Add(tree);
                    }
                }

                
                foreach(var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();

                    Model.Instance.ClearUsing();
                    List<string> usingList = new List<string>();
                    if (root.Usings!=null)
                    {
                        foreach(var u in root.Usings)
                        {
                            usingList.Add(u.Name.ToString());
                        }
                    }
                    Model.Instance.StartUsing(usingList);

                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    //导出所有类
                    var classNodes = nodes.OfType<BaseTypeDeclarationSyntax>();
                    step = ECompilerStet.ScanType;
                    foreach (var c in classNodes)
                    {
                        ExportType(c);
                    }


                }

                foreach (var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();
                    Model.Instance.ClearUsing();
                    List<string> usingList = new List<string>();
                    if (root.Usings != null)
                    {
                        foreach (var u in root.Usings)
                        {
                            usingList.Add(u.Name.ToString());
                        }
                    }
                    Model.Instance.StartUsing(usingList);

                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    var classNodes = nodes.OfType<BaseTypeDeclarationSyntax>();

                    step = ECompilerStet.ScanMember;
                    foreach (var c in classNodes)
                    {
                        ExportType(c);
                    }
                }

                foreach (var tree in treeList)
                {
                    var root = (CompilationUnitSyntax)tree.GetRoot();
                    Model.Instance.ClearUsing();
                    List<string> usingList = new List<string>();
                    if (root.Usings != null)
                    {
                        foreach (var u in root.Usings)
                        {
                            usingList.Add(u.Name.ToString());
                        }
                    }
                    Model.Instance.StartUsing(usingList);

                    IEnumerable<SyntaxNode> nodes = root.DescendantNodes();
                    var classNodes = nodes.OfType<BaseTypeDeclarationSyntax>();

                    step = ECompilerStet.Compile;
                    foreach (var c in classNodes)
                    {
                        ExportType(c);
                    }
                }

                OdbcTransaction trans = con.BeginTransaction();
                _trans = trans;

                //存储数据库
                foreach (var v in Model.compilerTypes)
                {
                    //foreach (var c in v.Value)
                    {
                        Metadata.DB.SaveDBType(v.Value, _con, _trans);
                        //存储成员
                        foreach (var m in v.Value.members.Values)
                        {
                            Metadata.DB.SaveDBMember(m, _con, _trans);
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

        static int GetModifier(Metadata.DB_Type current_type, SyntaxTokenList Modifiers)
        {
            bool isPublic = current_type.is_interface?true: ContainModifier(Modifiers, "public");
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

        static void ExportType(BaseTypeDeclarationSyntax c)
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
        }

        static void ExportEnum(EnumDeclarationSyntax c)
        {
            if (step == ECompilerStet.ScanType)
            {
                Metadata.DB_Type type = new Metadata.DB_Type();

                //bool isPublic = ContainModifier(c.Modifiers, "public");
                //bool isProtected = ContainModifier(c.Modifiers, "protected");
                //bool isPrivate = !isPublic && !isProtected;
                type.is_abstract = ContainModifier(c.Modifiers, "abstract");
                type.is_interface = false;
                type.is_enum = true;
                type.is_value_type = false;
                //Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                type.modifier = GetModifier(type, c.Modifiers);
                type.name = c.Identifier.Text;

                type.usingNamespace = new List<string>();
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                    type.usingNamespace.AddRange(Model.Instance.usingNamespace);
                    foreach (var ns in namespaceDeclarationSyntax.Usings)
                    {
                        type.usingNamespace.Add(ns.Name.ToString());
                    }
                    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                }

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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

                //Metadata.DB.SaveDBType(type, _con, _trans);
                Model.AddCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;

                //if (type.full_name != "System.Object" && type.base_type.IsVoid)
                //    type.base_type = Model.GetType("System.Object").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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

                Model.Instance.EnterType(type);


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
                    dB_Member.order = order++;
                    //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                    Model.AddMember(type.static_full_name, dB_Member);
                }

                Model.Instance.LeaveType();
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
                type.is_interface = true;
                type.is_value_type = false;
                //Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;

                type.usingNamespace = new List<string>();
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                    type.usingNamespace.AddRange(Model.Instance.usingNamespace);
                    foreach (var ns in namespaceDeclarationSyntax.Usings)
                    {
                        type.usingNamespace.Add(ns.Name.ToString());
                    }
                    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                }

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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
                        Metadata.GenericParameterDefinition genericParameterDefinition = new Metadata.GenericParameterDefinition();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                Model.AddCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                }
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;

                //if (type.full_name != "System.Object" && type.base_type.IsVoid)
                //    type.base_type = Model.GetType("System.Object").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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

                Model.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericParameterDefinition genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint.Add(GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<FieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
                Model.Instance.LeaveType();
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
                type.is_interface = false;
                type.is_value_type = true;
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;

                type.usingNamespace = new List<string>();
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                    type.usingNamespace.AddRange(Model.Instance.usingNamespace);
                    foreach (var ns in namespaceDeclarationSyntax.Usings)
                    {
                        type.usingNamespace.Add(ns.Name.ToString());
                    }
                    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                }

                //泛型
                if (c.TypeParameterList != null)
                {
                    type.is_generic_type_definition = true;
                    foreach (var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericParameterDefinition genericParameterDefinition = new Metadata.GenericParameterDefinition();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                bool partial = ContainModifier(c.Modifiers, "partial");
                if (!partial)
                    Model.AddCompilerType(type);
                else
                    Model.MergeCompilerType(type);
            }
            else if (step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                }
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;

                if (type.static_full_name != "System.Object" && type.base_type.IsVoid)
                    type.base_type = Model.GetType("System.Object").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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

                Model.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericParameterDefinition genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint.Add(GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<FieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
                Model.Instance.LeaveType();
                //Console.WriteLine();
            }
            else if (step == ECompilerStet.Compile)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                }
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;
                Model.Instance.EnterType(type);

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }

                Model.Instance.LeaveType();
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
                type.is_interface = false;
                type.is_value_type = false;
                type.is_class = true;
               // Console.WriteLine("Identifier:" + c.Identifier);
                //Console.WriteLine("Modifiers:" + c.Modifiers);
                bool partial = ContainModifier(c.Modifiers, "partial");
                type.modifier = GetModifier(type,c.Modifiers);
                type.name = c.Identifier.Text;

                type.usingNamespace = new List<string>();
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    //type.usingNamespace.Add(namespaceDeclarationSyntax.Name.ToString());
                    type.usingNamespace.AddRange(Model.Instance.usingNamespace);
                    foreach (var ns in namespaceDeclarationSyntax.Usings)
                    {
                        type.usingNamespace.Add(ns.Name.ToString());
                    }
                    type._namespace = namespaceDeclarationSyntax.Name.ToString();
                }

                
                
                //泛型
                if(c.TypeParameterList!=null)
                {
                    type.is_generic_type_definition = true;
                    foreach(var p in c.TypeParameterList.Parameters)
                    {
                        Metadata.GenericParameterDefinition genericParameterDefinition = new Metadata.GenericParameterDefinition();
                        genericParameterDefinition.type_name = p.Identifier.Text;
                        type.generic_parameter_definitions.Add(genericParameterDefinition);
                    }
                }

                //Metadata.DB.SaveDBType(type, _con, _trans);
                if(partial)
                {
                    Model.MergeCompilerType(type);
                }
                else
                    Model.AddCompilerType(type);
            }
            else if(step == ECompilerStet.ScanMember)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace( namespaceDeclarationSyntax.Name.ToString());
                }
                if(c.TypeParameterList!=null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;

                if (type.static_full_name != "System.Object" && type.base_type.IsVoid)
                    type.base_type = Model.GetType("System.Object").GetRefType();

                //父类
                if (c.BaseList != null)
                {
                    foreach (var b in c.BaseList.Types)
                    {
                        Metadata.DB_Type dB_Type = Model.GetType(GetTypeSyntax(b.Type));
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


                Model.Instance.EnterType(type);

                //泛型参数
                if (c.ConstraintClauses != null)
                {
                    foreach (var Constraint in c.ConstraintClauses)
                    {

                        Metadata.GenericParameterDefinition genericParameterDefinition = type.generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                        foreach (var tpc in Constraint.Constraints)
                        {
                            genericParameterDefinition.constraint.Add(GetTypeParameterSyntax(tpc));
                        }

                    }
                }


                //导出所有变量
                var virableNodes = c.ChildNodes().OfType<FieldDeclarationSyntax>();
                foreach (var v in virableNodes)
                {
                    ExportVariable(v, type);
                }

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }
                Model.Instance.LeaveType();
                //Console.WriteLine();
            }
            else if(step == ECompilerStet.Compile)
            {
                string typeName = c.Identifier.Text;
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = c.Parent as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    Model.Instance.EnterNamespace(namespaceDeclarationSyntax.Name.ToString());
                }
                if (c.TypeParameterList != null)
                {
                    typeName += "[" + c.TypeParameterList.Parameters.Count + "]";
                }

                Metadata.DB_Type type = Model.Instance.GetIndifierInfo(typeName).type;
                Model.Instance.EnterType(type);

                //导出所有方法
                var funcNodes = c.ChildNodes().OfType<BaseMethodDeclarationSyntax>();
                foreach (var f in funcNodes)
                {
                    ExportMethod(f, type);
                }

                Model.Instance.LeaveType();
            }
        }

       
        //static string GetGenericTypeName(string GenericType)
        //{

        //}

        
        static void ExportVariable(FieldDeclarationSyntax v, Metadata.DB_Type type)
        {
            Metadata.DB_Type v_type = Model.GetType(GetTypeSyntax(v.Declaration.Type));

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
                    dB_Member.member_type = (int)Metadata.MemberTypes.Field;
                    dB_Member.modifier = GetModifier(type,v.Modifiers);
                    dB_Member.field_type = v_type.GetRefType();
                    if(ve.Initializer!=null)
                        dB_Member.field_initializer = ExportExp(ve.Initializer.Value);

                    dB_Member.attributes = ExportAttributes(v.AttributeLists);
                    //Metadata.DB.SaveDBMember(dB_Member, _con, _trans);
                    Model.AddMember(type.static_full_name, dB_Member);
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
                    }

                    if(f.TypeParameterList != null)
                    {
                        foreach (var p in f.TypeParameterList.Parameters)
                        {
                            Metadata.GenericParameterDefinition genericParameterDefinition = new Metadata.GenericParameterDefinition();
                            genericParameterDefinition.type_name = p.Identifier.Text;
                            dB_Member.method_generic_parameter_definitions.Add(genericParameterDefinition);
                        }

                        //泛型参数
                        if (f.ConstraintClauses != null)
                        {
                            foreach (var Constraint in f.ConstraintClauses)
                            {

                                Metadata.GenericParameterDefinition genericParameterDefinition = dB_Member.method_generic_parameter_definitions.First((a) => { return a.type_name == Constraint.Name.Identifier.Text; });
                                foreach (var tpc in Constraint.Constraints)
                                {
                                    genericParameterDefinition.constraint.Add(GetTypeParameterSyntax(tpc));
                                }

                            }
                        }
                    }

                    //属性
                    dB_Member.attributes = ExportAttributes(v.AttributeLists);

                    Model.Instance.EnterMethod(dB_Member);

                    Metadata.Expression.TypeSyntax retType = GetTypeSyntax(f.ReturnType);
                    if (retType != null)
                        dB_Member.method_ret_type = retType;
                    else
                        dB_Member.method_ret_type = Metadata.Expression.TypeSyntax.Void;

                    Model.AddMember(type.static_full_name, dB_Member);
                    MemberMap[f] = dB_Member;
                    //Console.WriteLine();

                    Model.Instance.LeaveMethod();
                }
                else if (step == ECompilerStet.Compile)
                {
                    Metadata.DB_Member dB_Member = MemberMap[f];
                    Model.Instance.EnterMethod(dB_Member);
                    if (f.Body != null)
                        if (!ingore_method_body)
                        {
                            dB_Member.method_body = ExportBody(f.Body);
                        }
                    Model.Instance.LeaveMethod();
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
                    dB_Member.method_ret_type = Metadata.Expression.TypeSyntax.Void;

                    Model.AddMember(type.static_full_name, dB_Member);
                    MemberMap[f] = dB_Member;
                    //Console.WriteLine();
                }
                else if (step == ECompilerStet.Compile)
                {
                    if(!ingore_method_body)
                    {
                        Metadata.DB_Member dB_Member = MemberMap[f];
                        dB_Member.method_body = ExportBody(f.Body);
                    }
                }
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
            else
            {
                Console.Error.WriteLine(string.Format("error:不支持的表达式 {0} {1}" , es.GetType().Name,es.ToString()));
            }
            return null;
        }
        static Metadata.Expression.Exp ExportExp(IdentifierNameSyntax es)
        {
            Metadata.Expression.IndifierExp db_les = new Metadata.Expression.IndifierExp();
            db_les.Name = es.Identifier.Text;
            return db_les;
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
            if(es.Expression is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax maes = es.Expression as MemberAccessExpressionSyntax;
                db_les.Name = (maes).Name.Identifier.Text;
                db_les.Caller = ExportExp(maes.Expression);
            }
            else if (es.Expression is IdentifierNameSyntax)
            {
                IdentifierNameSyntax nameSyntax = es.Expression as IdentifierNameSyntax;
                db_les.Name = nameSyntax.Identifier.Text;
                db_les.Caller = new Metadata.Expression.ThisExp();
            }
            else
            {
                Console.Error.WriteLine("不支持的方法调用表达式 " + es.ToString());
            }

            
            foreach(var a in es.ArgumentList.Arguments)
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
            //Metadata.Expression.FieldExp op_Equals = new Metadata.Expression.FieldExp();
            
            //db_les.Caller = ExportExp(es.Operand);

            Metadata.Expression.MethodExp db_Add = new Metadata.Expression.MethodExp();
            if (es.OperatorToken.Text == "++")
            {
                db_Add.Name = "op_PlusPlus";
            }
            else if(es.OperatorToken.Text == "--")
            {
                db_Add.Name = "op_SubSub";
            }
            else
            {
                Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
            }
            db_Add.Caller = ExportExp(es.Operand);


            
            //db_Add.Caller = op_Token;
            db_Add.Args.Add(new Metadata.Expression.ConstExp() { value = "1" });

            Metadata.Expression.MethodExp db_les = new Metadata.Expression.MethodExp();
            db_les.Caller = db_Add;
            db_les.Name = "op_Assign";
            db_les.Args.Add(db_Add);
            return db_les;
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
            if(es.Operand is LiteralExpressionSyntax)
            {
                Metadata.Expression.ConstExp exp = ExportExp(es.Operand) as Metadata.Expression.ConstExp;
                if(es.OperatorToken.Text == "-")
                    exp.value = "-" + exp.value;
                else
                    Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
                return exp;
            }

            {
                Metadata.Expression.MethodExp exp = new Metadata.Expression.MethodExp();
                if (es.OperatorToken.Text == "-")
                    exp.Name = "op_Invert";
                else
                    Console.Error.WriteLine("无法识别的操作符 " + es.OperatorToken.Text);
                exp.Caller = ExportExp(es.Operand);
                return exp;
            }
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
    }
}
