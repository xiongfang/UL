using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metadata;

namespace CppConverter
{
   
    class CppConverter : IConverter
    {

        static Project project;
        Dictionary<string, TypeConfig> configs = new Dictionary<string, TypeConfig>();
        SortedDictionary<int, ITypeConverter> Converters = new SortedDictionary<int, ITypeConverter>();

        IDefaultTypeConverter DefaultConverter;

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

        public IDefaultTypeConverter DefaultTypeConverter { get { return DefaultConverter; } }

        public Project GetProject() { return project; }


        public CppConverter()
        {
        }

        public void RegistTypeConverter(ITypeConverter tc)
        {
            this.Converters[tc.priority] = tc;
        }

        class Model
        {
            public static Metadata.Model Instance = new Metadata.Model(new TypeFinder());
        }

        class TypeFinder
        : Metadata.IModelTypeFinder
        {
            //已经加载的类
            static Dictionary<string, Metadata.DB_Type> types = new Dictionary<string, DB_Type>();

            public static Metadata.DB_Type LoadType(string full_name)
            {
                Metadata.DB_Type type = null;
                foreach (var p in project.search_path)
                {
                    type = Metadata.DB.LoadType(p, full_name);
                    if (type != null)
                    {
                        types.Add(full_name, type);
                        break;
                    }
                }

                return type;
            }
            public static Dictionary<string,Metadata.DB_Type> LoadNamespace(string namespaceName)
            {
                Dictionary<string, Metadata.DB_Type> type = null;
                foreach (var p in project.search_path)
                {
                    type = Metadata.DB.LoadNamespace(p, namespaceName);
                    if (type != null && type.Count>0)
                    {
                        foreach(var t in type)
                        {
                            types.Add(t.Key, t.Value);
                        }
                        break;
                    }
                }

                return type;
            }



            //查找一个数据库类型
            public Metadata.DB_Type FindType(string full_name)
            {
                if (types.ContainsKey(full_name))
                    return types[full_name];

                Metadata.DB_Type type = LoadType( full_name);
                if(type!=null)
                {
                    types.Add(full_name, type);
                } 

                return type;
            }
            //查找一个类型，如果是动态类型，构造一个
            public Metadata.DB_Type FindType(Metadata.Expression.TypeSyntax typeSyntax, Metadata.Model model)
            {
                if (typeSyntax.IsVoid)
                    return null;

                if (typeSyntax.isGenericType)
                {
                    Metadata.DB_Type ma = FindType(typeSyntax.GetTypeDefinitionFullName());
                    return Metadata.DB_Type.MakeGenericType(ma, typeSyntax.args, new Metadata.Model(new TypeFinder()));
                }

                if (typeSyntax.isGenericParameter)
                {
                    //return Metadata.DB_Type.MakeGenericParameterType(GetType(typeSyntax), new Metadata.GenericParameterDefinition() { type_name = gps.Name });
                    return model.GetIndifierInfo(typeSyntax.Name).type;
                }

                return FindType(typeSyntax.GetTypeDefinitionFullName());
            }
        }

        //方法体的类，无需声明
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
                        result.Add(m.type);
                    }
                    else if (m.member_type == (int)Metadata.MemberTypes.Method)
                    {
                        if(m.type.isGenericType || type.is_delegate)
                            result.Add(m.type);
                        foreach (var a in m.method_args)
                        {
                            if(a.type.isGenericType)
                                result.Add(a.type);
                        }
                    }
                }
            }
        }
        //返回一个类型的不可以声明的类型
        public HashSet<string> GetTypeDependencesNoDeclareType(Metadata.DB_Type type)
        {
            HashSet<string> set = new HashSet<string>();

            Metadata.Model model = new Metadata.Model(new TypeFinder());

            MyCppHeaderTypeNoDeclareFinder f = new MyCppHeaderTypeNoDeclareFinder(model);
            model.AcceptTypeVisitor(f,type);

            //泛型需要添加方法体
            if(type.is_generic_type_definition)
            {
                set = GetMethodBodyDependences(type);
            }

            
            foreach (var s in f.result)
            {
                if (s.IsVoid)
                    continue;
                if (s.isGenericParameter)
                    continue;
                set.Add(s.GetTypeDefinitionFullName());
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

            set.Add(type.GetTypeDefinitionFullName());
            if (type.isGenericType)
            {
                foreach (var p in type.args)
                {
                    if (p.isGenericParameter || p.IsVoid)
                        continue;
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
            Metadata.Model model = new Metadata.Model(new TypeFinder());

            Metadata.MyCppHeaderTypeFinder f = new Metadata.MyCppHeaderTypeFinder(model);
            model.AcceptTypeVisitor( f, type);

            HashSet<string> set = new HashSet<string>();
            foreach (var s in f.result)
            {
                if (s.IsVoid)
                    continue;
                if (s.isGenericParameter)
                    continue;
                set.Add(s.GetTypeDefinitionFullName());
                foreach (var l in GetTypeList(s))
                {
                    set.Add(l);
                }
            }

            set.Remove(type.static_full_name);

            return set;
        }

        public HashSet<string> GetMethodBodyDependences(Metadata.DB_Type type)
        {
            Metadata.Model model = new Metadata.Model(new TypeFinder());

            Metadata.MyCppMethodBodyTypeFinder f = new Metadata.MyCppMethodBodyTypeFinder(model);
            model.AcceptTypeVisitor( f, type);

            HashSet<string> set = new HashSet<string>();
            foreach (var s in f.typeRef)
            {
                if (s.IsVoid)
                    continue;
                if (s.isGenericParameter)
                    continue;
                set.Add(s.GetTypeDefinitionFullName());
                foreach (var l in GetTypeList(s))
                {
                    set.Add(l);
                }
            }
            set.Remove(type.static_full_name);
            return set;
        }

        //void LoadTypeDependences(string full_name, Dictionary<string, Metadata.DB_Type> loaded)
        //{
        //    Metadata.DB_Type type = null;
        //    if (!loaded.ContainsKey(full_name))
        //    {
        //        type = Model.Instance.GetType(full_name);
        //        if (type == null)
        //            return;
        //        loaded.Add(type.static_full_name, type);
        //    }
        //    else
        //    {
        //        type = loaded[full_name];
        //    }

        //    HashSet<string> dep = GetTypeDependences(type);

        //    foreach (var t in dep)
        //    {
        //        //string database_type = Metadata.DB_Type.GetGenericDefinitionName(t);
        //        if (!loaded.ContainsKey(t))
        //        {
        //            LoadTypeDependences(t, loaded);
        //        }
        //    }
        //    HashSet<string> body_Dep = GetMethodBodyDependences(type);
        //    foreach (var t in body_Dep)
        //    {
        //        //string database_type = Metadata.DB_Type.GetGenericDefinitionName(t);
        //        if (!loaded.ContainsKey(t))
        //        {
        //            LoadTypeDependences(t, loaded);
        //        }
        //    }
        //}

        public void GO(string[] args)
        {
            project = Metadata.DB.ReadJsonObject<Project>(System.IO.File.ReadAllText(args[0]));
            if(project.type_settings!=null)
            foreach (var c in project.type_settings)
            {
                configs[c.name] = c;
            }

            List<string> ref_ns = new List<string>();
            if(project.ref_namespace!=null)
                ref_ns.AddRange(project.ref_namespace);

            string pj_dir = System.IO.Path.GetFullPath(args[0]);
            pj_dir = pj_dir.Substring(0, pj_dir.Length - System.IO.Path.GetFileName(pj_dir).Length - 1);
            project.output_dir = System.IO.Path.Combine(pj_dir, project.output_dir);
            for(int i=0;i<project.search_path.Length;i++)
            {
                project.search_path[i] = System.IO.Path.Combine(pj_dir, project.search_path[i]);
            }

            if(project.converterType == "cpp")
            {
                DefaultConverter = new DefaultTypeConverter(this);
            }
            else if(project.converterType == "lua")
            {
                DefaultConverter = new LuaTypeConverter(this);
            }
            else if(project.converterType == "ue4")
            {
                DefaultConverter = new UE4_TypeConverter(this);
            }

            List<DB_Type> types = new List<DB_Type>();
            //加载命名空间和导出的类
            foreach (var ns in project.export_namespace)
            {
                types.AddRange(TypeFinder.LoadNamespace(ns).Values);
                //foreach (var t in nsTypes)
                //{
                //    Model.types.Add(t.Value.static_full_name, t.Value);
                //}
            }
            foreach (var ns in project.export_type)
            {
                Metadata.DB_Type type = TypeFinder.LoadType(ns);
                if(type!=null)
                {
                    types.Add(type);
                }
                //Model.types.Add(type.static_full_name, type);
            }

            //加载依赖的类
            //List<Metadata.DB_Type> typeList = new List<Metadata.DB_Type>();
            //typeList.AddRange(Model.types.Values);
            //Dictionary<string, DB_Type> loaded = new Dictionary<string, DB_Type>(Model.types);
            //foreach (var t in typeList)
            //{
            //    LoadTypeDependences(t.static_full_name, loaded);
            //}

            //导出所有非引用的类型
            foreach (var t in types)
            {
                if (!ref_ns.Contains(t._namespace))
                {
                    DefaultConverter.ConvertType(t);
                }
            }
        }

        

        
    }

}
