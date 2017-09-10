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
        //public string id;
        public string full_name;
        public string comments = "";
        public int modifier;
        public bool is_abstract;
        public string _parent = "";

        public int[] imports;
        public string ext = "";
        public bool is_value_type;
        public bool is_interface;

        public string _namespace
        {
            get
            {
                if (full_name.Contains("."))
                    return full_name.Substring(0,full_name.LastIndexOf("."));
                return "";
            }
        }

        public string name
        {
            get
            {
                if (full_name.Contains("."))
                    return full_name.Substring(full_name.LastIndexOf(".")+1);
                return full_name;
            }
        }

    }

    public class DB_Member
    {
        public string declaring_type;
        public string name;
        public bool is_static;
        public int modifier;
        public string comments = "";
        //public int id;
        public int member_type;
        public string ext = "";
        public string child = "";
    }

    public class MemberVaiable
    {
        public string type_fullname;
    }

    public class MemberFunc
    {
        public class Args
        {
            public string type_fullname;
            public string name;
            public bool is_ref;
            public bool is_out;
            public string default_value = "";
        }
        public Args[] args;

        public string ret_type;

        public DB_BlockSyntax body;
    }

    //语句
    public class DB_StatementSyntax
    {

    }

    public class DB_BlockSyntax: DB_StatementSyntax
    {
        public List<DB_StatementSyntax> List = new List<DB_StatementSyntax>();
    }

    public class DB_IfStatementSyntax:DB_StatementSyntax
    {
        public DB_ExpressionSyntax Condition;
        public DB_StatementSyntax Statement;
        public DB_StatementSyntax Else;
    }
    public class DB_ExpressionStatementSyntax : DB_StatementSyntax
    {
        public DB_ExpressionSyntax Exp;
    }
    public class DB_LocalDeclarationStatementSyntax : DB_StatementSyntax
    {
        public string Type;
        public List<VariableDeclaratorSyntax> Variables = new List<VariableDeclaratorSyntax>();

    }

    public class VariableDeclaratorSyntax
    {
        public string Identifier;
        //public List<DB_ArgumentSyntax> ArgumentList = new List<DB_ArgumentSyntax>();
        public DB_InitializerExpressionSyntax Initializer;
    }


    //表达式
    public class DB_ExpressionSyntax
    {

    }

    public class DB_LiteralExpressionSyntax : DB_ExpressionSyntax
    {
        public string token;
    }

    public class DB_MemberAccessExpressionSyntax : DB_ExpressionSyntax
    {
        public DB_ExpressionSyntax Exp;
        public string name;
    }

    public class DB_ArgumentSyntax:DB_ExpressionSyntax
    {
        public DB_ExpressionSyntax Expression;
    }
    public class DB_InvocationExpressionSyntax : DB_ExpressionSyntax
    {
        public DB_ExpressionSyntax Exp;
        public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();

    }

    public class DB_IdentifierNameSyntax : DB_ExpressionSyntax
    {
        public string Name;
    }

    public class DB_InitializerExpressionSyntax : DB_ExpressionSyntax
    {
        public List<DB_ExpressionSyntax> Expressions = new List<DB_ExpressionSyntax>();
    }
    public class DB_ObjectCreationExpressionSyntax:DB_ExpressionSyntax
    {
        public string Type;
        public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();
        public DB_InitializerExpressionSyntax Initializer;
    }

    //
    // 摘要:
    //     
    public enum MemberTypes
    {
        //
        // 摘要:
        //     指定该成员是一个构造函数
        Constructor = 1,
        //
        // 摘要:
        //     指定该成员是一个事件。
        Event = 2,
        //
        // 摘要:
        //     指定该成员是一个字段。
        Field = 4,
        //
        // 摘要:
        //     指定该成员是一种方法。
        Method = 8,
        //
        // 摘要:
        //     指定成员是属性。
        Property = 16,
        //
        // 摘要:
        //     指定该成员是一种类型。
        TypeInfo = 32,
        //
        // 摘要:
        //     指定该成员是自定义成员的指针类型。
        Custom = 64,
        //
        // 摘要:
        //     指定该成员是嵌套的类型。
        NestedType = 128,
        //
        // 摘要:
        //     指定所有成员类型。
        All = 191
    }

    public enum Modifier
    {
        Public,
        Private,
        Protected
    }

    public class DB
    {

        public static int MakeModifier(bool isPublic, bool isPrivate, bool isProtected)
        {
            if (isPublic)
                return (int)Modifier.Public;
            else if (isPrivate)
                return (int)Modifier.Private;
            else if (isProtected)
                return (int)Modifier.Protected;
            return (int)Modifier.Public;
        }

        public static void SaveDBType(DB_Type type, OdbcConnection _con, OdbcTransaction _trans)
        {
            {
                string cmdText = string.Format("delete from type where full_name='{0}'", type.full_name);
                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.ExecuteNonQuery();
            }

            {
                string cmdText = string.Format("delete from member where declaring_type='{0}'", type.full_name);
                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.ExecuteNonQuery();
            }

            {
                string cmdText = string.Format("insert into type(full_name,comments,modifier,is_abstract,parent,imports,ext,is_value_type,is_interface) values(\"{1}\",\"{2}\",{3},{4},\"{5}\",?,?,?,?);",
                    "",type.full_name, type.comments, type.modifier, type.is_abstract, type._parent);


                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.Parameters.AddWithValue("1", type.imports == null ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(type.imports));
                cmd.Parameters.AddWithValue("2", type.ext);
                cmd.Parameters.AddWithValue("3", type.is_value_type);
                cmd.Parameters.AddWithValue("4", type.is_interface);
                cmd.ExecuteNonQuery();
            }
        }

        public static void SaveDBMember(DB_Member member, OdbcConnection _con, OdbcTransaction _trans)
        {
            {
                string cmdText = string.Format("delete from member where declaring_type='{0}' and name='{1}' and child=?", member.declaring_type, member.name);
                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.Parameters.AddWithValue("1", member.child);
                cmd.ExecuteNonQuery();
            }

            {
                string CommandText = string.Format("insert into member(declaring_type,name,comments,modifier,is_static,member_type,ext,child) values(\"{0}\",\"{2}\",\"{3}\",{4},{5},\"{6}\",\"{7}\",?);",
                member.declaring_type, "", member.name, member.comments, member.modifier, member.is_static, member.member_type, member.ext);

                OdbcCommand cmd = new OdbcCommand(CommandText, _con, _trans);

                cmd.Parameters.AddWithValue("1", member.child);


                cmd.ExecuteNonQuery();
            }

        }
    }
}
