using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Odbc;
using Metadata;

namespace CSharpConverter
{
    class Program
    {
        //class DB_Namespace
        //{
        //    public int id;
        //    public string fullname;
        //}
        

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
            db_t._namespace = type.Namespace;
            db_t.is_interface = type.IsInterface;
            typeList.Add(type.GUID,db_t);

            if (type.BaseType != null)
            {
                AddType(type.BaseType);
                db_t._parent = type.BaseType.FullName;
            }

            DB.SaveDBType(db_t,_con,_trans);


            

            MemberInfo[] members = type.GetMembers();
            for(int i=0;i<members.Length;i++)
            {
                AddMember( type.GUID.ToString(), i, members[i]);
            }

            
        }


        static void AddMember(string type_id, int id, MemberInfo mi)
        {
            DB_Member member = new DB_Member();
            //member.id = id;
            member.declaring_type = type_id;
            member.name = mi.Name;
            member.member_type = (int)mi.MemberType;

            if(mi is FieldInfo )
            {
                FieldInfo fi = mi as FieldInfo;
                member.is_static = fi.IsStatic;
                member.modifier = DB.MakeModifier(fi.IsPublic, fi.IsPrivate, false);


                AddType(fi.FieldType);

                MemberVaiable mv = new MemberVaiable();
                mv.type_fullname = fi.FieldType.FullName;

                member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mv);
            }
            
            if(mi is MethodInfo)
            {
                MethodInfo method = mi as MethodInfo;
                member.is_static = method.IsStatic;
                member.modifier = DB.MakeModifier(method.IsPublic, method.IsPrivate, false);


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

                    mf.args[i].type_fullname = pars[i].ParameterType.FullName;

                    if (pars[i].HasDefaultValue)
                    {
                        if (pars[i].RawDefaultValue != null)
                            mf.args[i].default_value = pars[i].RawDefaultValue.ToString();
                        else
                        {
                            mf.args[i].default_value = "null";
                        }
                    }

                }

                MethodBody body = method.GetMethodBody();

                member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mf);
            }

            DB.SaveDBMember(member, _con, _trans);
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
