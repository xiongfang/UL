using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata
{
    public interface IModelTypeFinder
    {
        DB_Type FindType(DB_TypeRef refType);
        Dictionary<string, Metadata.DB_Type> FindNamespace(string ns);
    }

    public class Model
    {
        public static IModelTypeFinder Finder;

        //当前类的命名空间
        static HashSet<string> usingNamespace = new HashSet<string>();
        static string outNamespace;
        //当前处理的类型
        static Metadata.DB_Type currentType;
        //当前函数的本地变量和参数
        static Stack<Dictionary<string, Metadata.DB_Type>> stackLocalVariables = new Stack<Dictionary<string, Metadata.DB_Type>>();

        public static void StartUsing(List<string> usingList)
        {
            foreach (var ns in usingList)
                usingNamespace.Add(ns);
        }

        public static void ClearUsing()
        {
            usingNamespace.Clear();
        }

        public static void EnterNamespace(string ns)
        {
            outNamespace = ns;
        }

        public static void LeaveNamespace()
        {
            outNamespace = null;
        }

        public static void EnterType(Metadata.DB_Type type)
        {
            currentType = type;
        }
        public static void LeaveType()
        {
            currentType = null;
        }

        public static void EnterBlock()
        {
            stackLocalVariables.Push(new Dictionary<string, Metadata.DB_Type>());
        }

        public static void LeaveBlock()
        {
            stackLocalVariables.Pop();
        }

        public static void AddLocal(string name, Metadata.DB_Type type)
        {
            stackLocalVariables.Peek().Add(name, type);
        }











        ////查找指定的命名空间的所有类型
        //public static Dictionary<string, Metadata.DB_Type> FindNamespace(string ns)
        //{
        //    Dictionary<string, Metadata.DB_Type> rt = new Dictionary<string, Metadata.DB_Type>();


        //    foreach (var v in types.Select(a => { if (a.Value._namespace == ns) return a.Value; return null; }))
        //    {
        //        if (v != null)
        //            rt.Add(v.unique_name, v);
        //    }
        //    return rt;
        //}

        //public static Metadata.DB_Type GetType(string full_name)
        //{
        //    if (types.ContainsKey(full_name))
        //        return types[full_name];

        //    return null;
        //}

        //public static Metadata.DB_Type GetType(Metadata.DB_TypeRef typeRef)
        //{
        //    if (types.ContainsKey(typeRef.identifer))
        //    {
        //        if (typeRef.parameters.Count > 0)
        //            return Metadata.DB_Type.MakeGenericType(types[typeRef.identifer], typeRef.parameters);
        //        if (!string.IsNullOrEmpty(typeRef.template_parameter_name))
        //        {
        //            Metadata.DB_Type declareType = types[typeRef.identifer];
        //            Metadata.DB_Type.GenericParameterDefinition typeDef = declareType.generic_parameter_definitions.Find((a) => { return a.type_name == typeRef.template_parameter_name; });
        //            return Metadata.DB_Type.MakeGenericParameterType(declareType, typeDef);
        //        }
        //        return types[typeRef.identifer];
        //    }

        //    return null;
        //}

        public static Metadata.DB_Type FindVariable(string name)
        {
            //查找本地变量
            foreach (var v in stackLocalVariables)
            {
                if (v.ContainsKey(name))
                    return v[name];
            }

            //查找成员变量
            if (currentType != null)
            {
                if (currentType.members.ContainsKey(name))
                    return Finder.FindType(currentType.members[name].typeName);
                //查找泛型
                if (currentType.is_generic_type_definition)
                {
                    foreach (var gd in currentType.generic_parameter_definitions)
                    {
                        if (gd.type_name == name)
                        {
                            return Metadata.DB_Type.MakeGenericParameterType(currentType, gd);
                        }
                    }
                }
            }

            //当前命名空间查找
            if(!string.IsNullOrEmpty(outNamespace))
            {
                Dictionary<string, Metadata.DB_Type> nsTypes = Finder.FindNamespace(outNamespace);
                if (nsTypes != null && nsTypes.ContainsKey(name))
                    return nsTypes[name];
            }

            //当前命名空间查找
            foreach (var nsName in usingNamespace)
            {
                Dictionary<string, Metadata.DB_Type> nsTypes = Finder.FindNamespace(nsName);
                if (nsTypes != null && nsTypes.ContainsKey(name))
                    return nsTypes[name];
            }

            return null;
        }
    }
}
