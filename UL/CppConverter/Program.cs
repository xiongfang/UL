using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CppConverter
{
    class Program
    {
        //public List<string> GetDependenceTypes()
        //{
        //    List<string> result = new List<string>();
        //    if (!string.IsNullOrEmpty(_parent))
        //        result.Add(_parent);
        //    foreach (var m in members.Values)
        //    {
        //        if (m.member_type == (int)MemberTypes.Field)
        //        {
        //            result.Add(m.field_type_fullname);
        //        }
        //        else if (m.member_type == (int)MemberTypes.Method)
        //        {
        //            if (!string.IsNullOrEmpty(m.method_ret_type))
        //                result.Add(m.method_ret_type);
        //            foreach (var a in m.method_args)
        //            {
        //                result.Add(a.type_fullname);
        //            }
        //        }
        //    }
        //}

        static OdbcConnection _con;
        static StringBuilder sb = new StringBuilder();
        static int depth;

        static void Main(string[] args)
        {
            using (OdbcConnection con = new OdbcConnection("Dsn=MySql;Database=ul"))
            {
                con.Open();
                _con = con;
                Dictionary<string,Metadata.DB_Type> types = Metadata.DB.LoadTypes("HelloWorld",_con);
                
                foreach(var t in types.Values)
                {
                    ConvertType(t);
                }

                System.IO.File.WriteAllText("Test.h", sb.ToString());
            }
        }

        static void AppendDepth()
        {
            for(int i=0;i<depth;i++)
            {
                sb.Append("\t");
            }
        }

        static void AppendLine(string msg)
        {
            AppendDepth();
            sb.AppendLine(msg);
        }

        static void Append(string msg)
        {
            AppendDepth();
            sb.Append(msg);
        }

        static void ConvertType(Metadata.DB_Type type)
        {
            sb.AppendLine(string.Format("namespace {0}{{",type._namespace));
            {
                depth++;
                AppendLine(string.Format("class {0}{{", type.name));
                {
                    depth++;

                    foreach(var m in type.members.Values)
                    {
                        ConvertMember(m);
                    }


                    depth--;
                }

                AppendLine("};");
                depth--;
            }
            
            sb.AppendLine("}");

        }

        static string GetModifierString(int modifier)
        {
            switch((Metadata.Modifier) modifier)
            {
                case Metadata.Modifier.Private:
                    return "private";
                case Metadata.Modifier.Protected:
                    return "protected";
                case Metadata.Modifier.Public:
                    return "public";
            }

            return "";
        }

        static void ConvertMember(Metadata.DB_Member member)
        {
            if(member.member_type == (int)Metadata.MemberTypes.Field)
            {
                AppendLine(GetModifierString(member.modifier) + ":");
                AppendLine(string.Format("{0} {1};", member.field_type_fullname, member.name));
            }
            else if(member.member_type == (int)Metadata.MemberTypes.Method)
            {
                AppendLine(GetModifierString(member.modifier) + ":");
                Append(string.Format("{0} {1}", string.IsNullOrEmpty(member.method_ret_type)?"void": member.method_ret_type, member.name));
                sb.Append("(");
                if(member.method_args!=null)
                {
                    for (int i = 0; i < member.method_args.Length; i++)
                    {
                        sb.Append(string.Format("{0} {1}", member.method_args[i].type_fullname, member.method_args[i].name));
                        if (i < member.method_args.Length-1)
                            sb.Append(",");
                    }
                }
                sb.AppendLine(");");
            }
        }
    }

}
