using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
   
    

    class CppConverter : IConverter
    {

        Project project;
        OdbcConnection _con;
        Dictionary<string, TypeConfig> configs = new Dictionary<string, TypeConfig>();
        SortedDictionary<int, ITypeConverter> Converters = new SortedDictionary<int, ITypeConverter>();

        DefaultTypeConverter DefaultConverter;

        public Metadata.Model GetModel()
        {
            return Model.Instance;
        }

        public TypeConfig GetTypeConfig(Metadata.DB_Type type)
        {
            if (configs.ContainsKey(type.static_full_name))
                return configs[type.static_full_name];

            return null;
        }

        public ITypeConverter GetTypeConverter(Metadata.DB_Type type)
        {
            foreach(var c in Converters)
            {
                if (c.Value.SupportType(this,type))
                    return c.Value;
            }
            return null;
        }

        public Project GetProject() { return project; }

        public string ExpressionToString(Metadata.Expression.Exp exp) { return DefaultConverter.ExpressionToString(exp); }

        public CppConverter()
        {
            DefaultConverter = new DefaultTypeConverter(this);
        }

        public void RegistTypeConverter(ITypeConverter tc)
        {
            this.Converters[tc.priority] = tc;
        }

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
            public static Dictionary<string, Metadata.DB_Type> types;

            public static Metadata.Model Instance = new Metadata.Model(new Finder());


            public static Metadata.DB_Type GetType(string full_name)
            {
                if (types.ContainsKey(full_name))
                    return types[full_name];
                return null;
            }

            public static Metadata.DB_Type GetType(Metadata.Expression.TypeSyntax typeRef)
            {
                Metadata.DB_Type type = GetType(typeRef.GetStaticFullName());
                if (type == null)
                    return null;

                if (typeRef is Metadata.Expression.IdentifierNameSyntax)
                    return type;
                if (typeRef is Metadata.Expression.GenericNameSyntax)
                {
                    Metadata.Expression.GenericNameSyntax gns = typeRef as Metadata.Expression.GenericNameSyntax;
                    return Metadata.DB_Type.MakeGenericType(type, gns.Arguments);
                }
                if (typeRef is Metadata.Expression.GenericParameterSyntax)
                {
                    Metadata.DB_Type declareType = type;
                    Metadata.GenericParameterDefinition typeDef = declareType.generic_parameter_definitions.Find((a) => { return a.type_name == typeRef.Name; });
                    return Metadata.DB_Type.MakeGenericParameterType(declareType, typeDef);
                }


                return null;
            }
        }


        class DataBaseFinder
        : Metadata.IModelTypeFinder
        {
            OdbcConnection _con;
            public DataBaseFinder(OdbcConnection con) { this._con = con; }
            //查找一个数据库类型
            public Metadata.DB_Type FindType(string full_name)
            {
                Metadata.DB_Type type = Model.GetType(full_name);
                if (type == null)
                {
                    type = Metadata.DB.LoadType(full_name, _con);
                    Model.types.Add(full_name, type);
                }

                return type;
            }
            //查找一个类型，如果是动态类型，构造一个
            public Metadata.DB_Type FindType(Metadata.Expression.TypeSyntax refType)
            {
                Metadata.DB_Type type = Model.GetType(refType);
                if (type == null)
                {
                    type = Metadata.DB.LoadType(refType.GetStaticFullName(), _con);
                    Model.types.Add(refType.GetStaticFullName(), type);
                    return Model.GetType(refType); ;
                }

                return type;
            }
        }

        public class MyCppHeaderTypeNoDeclareFinder : Metadata.ITypeVisitor
        {
            Metadata.Model model;
            public MyCppHeaderTypeNoDeclareFinder(Metadata.Model model)
            {
                this.model = model;
            }
            public HashSet<Metadata.Expression.TypeSyntax> result = new HashSet<Metadata.Expression.TypeSyntax>();
            public void VisitType(Metadata.DB_Type type)
            {
                if (!type.base_type.IsVoid)
                    result.Add(type.base_type);
                foreach (var i in type.interfaces)
                {
                    result.Add(i);
                }
                foreach (var m in type.members.Values)
                {
                    if (m.member_type == (int)Metadata.MemberTypes.Field)
                    {
                        result.Add(m.field_type);
                    }
                    else if (m.member_type == (int)Metadata.MemberTypes.Method || m.member_type == (int)Metadata.MemberTypes.Constructor)
                    {
                        if (m.method_ret_type is Metadata.Expression.GenericNameSyntax)
                            result.Add(m.method_ret_type);
                        foreach (var a in m.method_args)
                        {
                            if (a.type is Metadata.Expression.GenericNameSyntax)
                                result.Add(a.type);
                        }
                    }
                }
            }
            public bool VisitMember(Metadata.DB_Type type, Metadata.DB_Member member)
            {
                return false;
            }
            public void VisitStatement(Metadata.DB_Type type, Metadata.DB_Member member, Metadata.DB_StatementSyntax statement)
            {

            }
            public void VisitExp(Metadata.DB_Type type, Metadata.DB_Member member, Metadata.DB_StatementSyntax statement, Metadata.Expression.Exp exp)
            {

            }
        }
        //返回一个类型的不可以声明的类型
        public HashSet<string> GetTypeDependencesNoDeclareType(Metadata.DB_Type type)
        {
            Metadata.Model model = new Metadata.Model(new DataBaseFinder(_con));

            MyCppHeaderTypeNoDeclareFinder f = new MyCppHeaderTypeNoDeclareFinder(model);
            model.Visit(type, f);

            HashSet<string> set = new HashSet<string>();
            foreach (var s in f.result)
            {
                if (s.IsVoid)
                    continue;
                set.Add(s.GetStaticFullName());
                foreach (var l in GetTypeList(s))
                {
                    set.Add(l);
                }
            }

            return set;
        }

        static HashSet<string> GetTypeList(Metadata.Expression.TypeSyntax type)
        {
            HashSet<string> set = new HashSet<string>();

            set.Add(type.GetStaticFullName());
            if (type is Metadata.Expression.GenericNameSyntax)
            {
                Metadata.Expression.GenericNameSyntax gns = type as Metadata.Expression.GenericNameSyntax;
                foreach (var p in gns.Arguments)
                {
                    foreach (var l in GetTypeList(p))
                    {
                        set.Add(l);
                    }
                }
            }

            return set;
        }

        public HashSet<string> GetTypeDependences(Metadata.DB_Type type)
        {
            Metadata.Model model = new Metadata.Model(new DataBaseFinder(_con));

            Metadata.MyCppHeaderTypeFinder f = new Metadata.MyCppHeaderTypeFinder(model);
            model.Visit(type, f);

            HashSet<string> set = new HashSet<string>();
            foreach (var s in f.result)
            {
                if (s.IsVoid)
                    continue;
                set.Add(s.GetStaticFullName());
                foreach (var l in GetTypeList(s))
                {
                    set.Add(l);
                }
            }

            return set;
        }

        public HashSet<string> GetMethodBodyDependences(Metadata.DB_Type type)
        {
            Metadata.Model model = new Metadata.Model(new DataBaseFinder(_con));

            Metadata.MyCppMethodBodyTypeFinder f = new Metadata.MyCppMethodBodyTypeFinder(model);
            model.Visit(type, f);

            HashSet<string> set = new HashSet<string>();
            foreach (var s in f.typeRef)
            {
                if (s.IsVoid)
                    continue;
                set.Add(s.GetStaticFullName());
                foreach (var l in GetTypeList(s))
                {
                    set.Add(l);
                }
            }

            return set;
        }

        void LoadTypeDependences(string full_name, Dictionary<string, Metadata.DB_Type> loaded)
        {
            Metadata.DB_Type type = null;
            if (!loaded.ContainsKey(full_name))
            {
                type = Metadata.DB.LoadType(full_name, _con);
                if (type == null)
                    return;
                loaded.Add(type.static_full_name, type);
            }
            else
            {
                type = loaded[full_name];
            }

            HashSet<string> dep = GetTypeDependences(type);

            foreach (var t in dep)
            {
                //string database_type = Metadata.DB_Type.GetGenericDefinitionName(t);
                if (!loaded.ContainsKey(t))
                {
                    LoadTypeDependences(t, loaded);
                }
            }
            HashSet<string> body_Dep = GetMethodBodyDependences(type);
            foreach (var t in body_Dep)
            {
                //string database_type = Metadata.DB_Type.GetGenericDefinitionName(t);
                if (!loaded.ContainsKey(t))
                {
                    LoadTypeDependences(t, loaded);
                }
            }
        }

        public void GO(string[] args)
        {
            project = Metadata.DB.ReadObject<Project>(System.IO.File.ReadAllText(args[0]));
            foreach (var c in project.type_settings)
            {
                configs[c.name] = c;
            }

            List<string> ref_ns = new List<string>();
            ref_ns.AddRange(project.ref_namespace);

            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
            {
                con.Open();
                _con = con;

                Model.types = new Dictionary<string, Metadata.DB_Type>();

                ////加载引用类
                //foreach(var ns in cfg.ref_namespace)
                //{
                //    Dictionary<string, Metadata.DB_Type> nsTypes = Metadata.DB.LoadNamespace(ns, _con);
                //    foreach (var t in nsTypes)
                //    {
                //        Model.types.Add(t.Value.static_full_name, t.Value);
                //    }
                //}

                //加载命名空间和导出的类
                foreach (var ns in project.export_namespace)
                {
                    Dictionary<string, Metadata.DB_Type> nsTypes = Metadata.DB.LoadNamespace(ns, _con);
                    foreach (var t in nsTypes)
                    {
                        Model.types.Add(t.Value.static_full_name, t.Value);
                    }
                }
                foreach (var ns in project.export_type)
                {
                    Metadata.DB_Type type = Metadata.DB.LoadType(ns, _con);
                    Model.types.Add(type.static_full_name, type);
                }

                //加载依赖的类
                List<Metadata.DB_Type> typeList = new List<Metadata.DB_Type>();
                typeList.AddRange(Model.types.Values);
                foreach (var t in typeList)
                {
                    LoadTypeDependences(t.static_full_name, Model.types);
                }

                //导出所有非引用的类型
                foreach (var t in Model.types.Values)
                {
                    if (!ref_ns.Contains(t._namespace))
                    {
                        DefaultConverter.ConvertType(t);
                    }
                }


            }
        }

        

        
    }

}
