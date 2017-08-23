using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Odbc;

namespace CSharpConverter
{
    class Program
    {
        class DB_Namespace
        {
            public int id;
            public string fullname;
        }
        class DB_Type
        {
            public string id;
            public string name;
            public string comments;
            public int modifier;
            public bool is_abstract;
            public int parent_id;
            public int _namespace;
            public int[] imports;
            public string ext;
            public bool is_value_type;
        }

        class DB_Member
        {
            public string declaring_type_id;
            public string name;
            public bool is_static;
            public int modifier;
            public string comments;
            public int id;
            public int member_type;
            public string ext;
            public string child;
        }

        class MemberVaiable
        {
            public string type_id;
        }

        class MemberFunc
        {
            class Args
            {
                public int type;
                public string name;
            }
            public Args[] args;
        }


        static Dictionary<Guid, DB_Type> typeList = new Dictionary<Guid, DB_Type>();
        static void AddType(Type type)
        {
            if (typeList.ContainsKey(type.GUID))
                return;

            DB_Type db_t = new DB_Type();
            db_t.name = type.Name;
            db_t.is_abstract = type.IsAbstract;
            db_t.is_value_type = type.IsValueType;
            db_t.modifier = type.IsPublic ? 0 : 1;
            db_t.id = type.GUID.ToString();
            typeList.Add(type.GUID,db_t);
            if (type.BaseType != null)
            {
                AddType(type.BaseType);
            }
        }

        static int MakeModifier(bool isPublic,bool isPrivate,bool isProtected)
        {
            if (isPublic)
                return 1;
            else if (isPrivate)
                return 2;
            else if (isProtected)
                return 3;
            return 0;
        }

        static void AddMember(string type_id,int id,int modifier,MemberInfo mi)
        {
            DB_Member member = new DB_Member();
            member.id = id;
            member.declaring_type_id = type_id;
            member.name = mi.Name;

            if(mi is FieldInfo )
            {
                FieldInfo fi = mi as FieldInfo;
                member.is_static = fi.IsStatic;
                member.modifier = MakeModifier(fi.IsPublic, fi.IsPrivate, false);


                AddType(fi.FieldType);

                MemberVaiable mv = new MemberVaiable();
                mv.type_id = typeList[fi.FieldType.GUID].id;

            }

            
        }

        static void Main(string[] args)
        {

            

            //using(OdbcConnection con = new OdbcConnection(""))
            {
                //con.Open();

                Assembly sysAs = Assembly.GetAssembly(typeof(Int32));

                Type[] types = sysAs.GetExportedTypes();
                foreach (var t in types)
                {
                    AddType(t);

                    MemberInfo[] members = t.GetMembers();
                    foreach (var m in members)
                    {
                        //OdbcCommand cmd = new OdbcCommand("",con);
                        if(m is FieldInfo)
                        {

                        }
                    }
                }
            }

            
        }
    }
}
