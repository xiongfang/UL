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
        //class DB_Namespace
        //{
        //    public int id;
        //    public string fullname;
        //}
        class DB_Type
        {
            public string id;
            public string name;
            public string comments = "";
            public int modifier;
            public bool is_abstract;
            public string parent_id="";
            public string _namespace;
            public int[] imports;
            public string ext = "";
            public bool is_value_type;
            public bool is_interface;
        }

        class DB_Member
        {
            public string declaring_type_id;
            public string name;
            public bool is_static;
            public int modifier;
            public string comments = "";
            public int id;
            public int member_type;
            public string ext = "";
            public string child = "";
        }

        class MemberVaiable
        {
            public string type_id;
        }

        public class MemberFunc
        {
            public class Args
            {
                public string type_id;
                public string name;
                public bool is_in;
                public bool is_ret;
                public bool is_out;
                public string default_value="";
            }
            public Args[] args;
        }

        static void SaveDBType( DB_Type type)
        {


            string cmdText = string.Format("insert into type(id,name,comments,modifier,is_abstract,parent_id,namespace,imports,ext,is_value_type,is_interface) values(\"{0}\",\"{1}\",\"{2}\",{3},{4},\"{5}\",\"{6}\",?,?,?,?);",
                type.id,type.name,type.comments,type.modifier,type.is_abstract,type.parent_id,type._namespace);


            OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
            cmd.Parameters.AddWithValue("1",type.imports==null?"":Newtonsoft.Json.JsonConvert.SerializeObject( type.imports));
            cmd.Parameters.AddWithValue("2", type.ext);
            cmd.Parameters.AddWithValue("3", type.is_value_type);
            cmd.Parameters.AddWithValue("4", type.is_interface);
            cmd.ExecuteNonQuery();

        }

        static void SaveDBMember( DB_Member member)
        {
            string CommandText = string.Format("insert into member(declaring_type_id, id,name,comments,modifier,is_static,member_type,ext,child) values(\"{0}\",{1},\"{2}\",\"{3}\",{4},{5},{6},\"{7}\",?);",
                member.declaring_type_id,member.id,member.name,member.comments,member.modifier,member.is_static,member.member_type,member.ext);

            OdbcCommand cmd = new OdbcCommand(CommandText, _con, _trans);

            cmd.Parameters.AddWithValue("1", member.child);


            cmd.ExecuteNonQuery();
        }

        static Dictionary<Guid, DB_Type> typeList = new Dictionary<Guid, DB_Type>();
        static void AddType(Type type)
        {
            if (typeList.ContainsKey(type.GUID))
                return;


            Console.WriteLine("ExportType: " + type.Name);
            

            DB_Type db_t = new DB_Type();
            db_t.name = type.Name;
            db_t.is_abstract = type.IsAbstract;
            db_t.is_value_type = type.IsValueType;
            db_t.modifier = type.IsPublic ? 0 : 1;
            db_t.id = type.GUID.ToString();
            db_t._namespace = type.Namespace;
            db_t.is_interface = type.IsInterface;
            typeList.Add(type.GUID,db_t);

            if (type.BaseType != null)
            {
                AddType(type.BaseType);
                db_t.parent_id = typeList[type.BaseType.GUID].id;
            }

            SaveDBType(db_t);


            

            MemberInfo[] members = type.GetMembers();
            for(int i=0;i<members.Length;i++)
            {
                AddMember( type.GUID.ToString(), i, members[i]);
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

        static void AddMember(string type_id, int id, MemberInfo mi)
        {
            DB_Member member = new DB_Member();
            member.id = id;
            member.declaring_type_id = type_id;
            member.name = mi.Name;
            member.member_type = (int)mi.MemberType;

            if(mi is FieldInfo )
            {
                FieldInfo fi = mi as FieldInfo;
                member.is_static = fi.IsStatic;
                member.modifier = MakeModifier(fi.IsPublic, fi.IsPrivate, false);


                AddType(fi.FieldType);

                MemberVaiable mv = new MemberVaiable();
                mv.type_id = typeList[fi.FieldType.GUID].id;

                member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mv);
            }
            
            if(mi is MethodInfo)
            {
                MethodInfo method = mi as MethodInfo;
                member.is_static = method.IsStatic;
                member.modifier = MakeModifier(method.IsPublic, method.IsPrivate, false);


                MemberFunc mf = new MemberFunc();

                ParameterInfo[] pars = method.GetParameters();

                mf.args = new MemberFunc.Args[pars.Length];
                for (int i = 0; i < pars.Length;i++ )
                {
                    mf.args[i] = new MemberFunc.Args();
                    mf.args[i].is_in = pars[i].IsIn;
                    mf.args[i].is_out = pars[i].IsOut;
                    mf.args[i].is_ret = pars[i].IsRetval;
                    mf.args[i].name = pars[i].Name;

                    AddType(pars[i].ParameterType);

                    mf.args[i].type_id = typeList[ pars[i].ParameterType.GUID].id;

                    if(pars[i].HasDefaultValue)
                    {
                        if (pars[i].RawDefaultValue != null)
                            mf.args[i].default_value = pars[i].RawDefaultValue.ToString();
                        else
                        {
                            mf.args[i].default_value = "null";
                        }
                    }

                }

                member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mf);
            }

            SaveDBMember(member);
        }

        static OdbcConnection _con;
        static OdbcTransaction _trans;

        static void Main(string[] args)
        {



            using (OdbcConnection con = new OdbcConnection("Dsn=MySql"))
            {
                con.Open();
                _con = con;

                OdbcTransaction trans = con.BeginTransaction();
                _trans = trans;

                Assembly sysAs = Assembly.GetAssembly(typeof(Int32));

                Type[] types = sysAs.GetExportedTypes();
                foreach (var t in types)
                {
                    AddType(t);
                }

                Console.WriteLine("Commit...");
                trans.Commit();
            }

            
        }
    }
}
