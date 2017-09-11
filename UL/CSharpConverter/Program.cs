using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Odbc;
using Metadata;
using System.Reflection.Emit;
using System.IO;

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
            db_t.full_name = type.FullName;
            db_t.is_abstract = type.IsAbstract;
            db_t.is_value_type = type.IsValueType;
            db_t.modifier = type.IsPublic ? 0 : 1;
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

        static int PrintIL_Count = 0;

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

                List<MemberFunc.Args> args = new List<MemberFunc.Args>();
                for (int i = 0; i < pars.Length;i++ )
                {
                    MemberFunc.Args arg = new MemberFunc.Args();
                    arg.is_out = pars[i].IsOut;
                    arg.is_ref = pars[i].IsIn;
                    arg.name = pars[i].Name;

                    AddType(pars[i].ParameterType);

                    arg.type_fullname = pars[i].ParameterType.FullName;

                    if (pars[i].HasDefaultValue)
                    {
                        if (pars[i].RawDefaultValue != null)
                            arg.default_value = pars[i].RawDefaultValue.ToString();
                        else
                        {
                            arg.default_value = "null";
                        }
                    }

                }

                PrintIL_Count++;
                if(PrintIL_Count<3)
                {
                    SDILReader.MethodBodyReader reader = new SDILReader.MethodBodyReader(method);

                    if (reader.instructions != null)
                    {
                        foreach (var ins in reader.instructions)
                        {
                            Console.WriteLine(ins.GetCode());
                        }
                    }
                }



                member.child = Newtonsoft.Json.JsonConvert.SerializeObject(mf);
            }

            DB.SaveDBMember(member, _con, _trans);
        }


        public static OpCode[] GetOpCodes(byte[] data)
        {
            List<OpCode> opCodes = new List<OpCode>();

            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);

            short code = br.ReadByte();
            int size = 1;
            if (code == 0xfe)
            {
                ms.Position -= 1;
                code = br.ReadByte();
                size = 2;
            }

            try
            {
                OpCode result = (OpCode)typeof(OpCodes).GetFields(BindingFlags.Static).First((a) =>
                {
                    OpCode oc = (OpCode)a.GetValue(null);
                    if (oc != null)
                    {
                        return oc.Value == code && oc.Size == size;
                    }
                    return false;
                }).GetValue(null);

                opCodes.Add(result);
            }
            catch(Exception )
            {

            }


            return opCodes.ToArray();
        }

        static OdbcConnection _con;
        static OdbcTransaction _trans;

        
        static void Main(string[] args)
        {

            SDILReader.Globals.LoadOpCodes();

            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
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
