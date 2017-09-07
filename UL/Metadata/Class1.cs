using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata
{
    public class DB_Type
    {
        public string id;
        public string name;
        public string comments = "";
        public int modifier;
        public bool is_abstract;
        public string parent_id = "";
        public string _namespace;
        public int[] imports;
        public string ext = "";
        public bool is_value_type;
        public bool is_interface;
    }

    public class DB_Member
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

    public class MemberVaiable
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
            public string default_value = "";
        }
        public Args[] args;

        public class Body
        {
            public int stack;

        }

        public Body body;
    }

    //语句
    public class DB_StatementSyntax
    {

    }

    public class DB_IfStatementSyntax:DB_StatementSyntax
    {
        public DB_ExpressionSyntax Exp;

    }

    //表达式
    public class DB_ExpressionSyntax
    {

    }



    public class DB
    {
        public static void SaveDBType(DB_Type type, OdbcConnection _con, OdbcTransaction _trans)
        {


            string cmdText = string.Format("insert into type(id,name,comments,modifier,is_abstract,parent_id,namespace,imports,ext,is_value_type,is_interface) values(\"{0}\",\"{1}\",\"{2}\",{3},{4},\"{5}\",\"{6}\",?,?,?,?);",
                type.id, type.name, type.comments, type.modifier, type.is_abstract, type.parent_id, type._namespace);


            OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
            cmd.Parameters.AddWithValue("1", type.imports == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(type.imports));
            cmd.Parameters.AddWithValue("2", type.ext);
            cmd.Parameters.AddWithValue("3", type.is_value_type);
            cmd.Parameters.AddWithValue("4", type.is_interface);
            cmd.ExecuteNonQuery();

        }

        public static void SaveDBMember(DB_Member member, OdbcConnection _con, OdbcTransaction _trans)
        {
            string CommandText = string.Format("insert into member(declaring_type_id, id,name,comments,modifier,is_static,member_type,ext,child) values(\"{0}\",{1},\"{2}\",\"{3}\",{4},{5},{6},\"{7}\",?);",
                member.declaring_type_id, member.id, member.name, member.comments, member.modifier, member.is_static, member.member_type, member.ext);

            OdbcCommand cmd = new OdbcCommand(CommandText, _con, _trans);

            cmd.Parameters.AddWithValue("1", member.child);


            cmd.ExecuteNonQuery();
        }
    }
}
