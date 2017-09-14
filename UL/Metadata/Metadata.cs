using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public string full_name
        {
            get
            {
                return _namespace + "." + name;
            }
            set
            {
                name = GetName(value);
                _namespace = GetNamespace(value);
            }
        }

        public string comments = "";
        public int modifier;
        public bool is_abstract;
        public string _parent = "";

        public int[] imports;
        public string ext = "";
        public bool is_value_type;
        public bool is_interface;
        //public bool is_array;
        //public string element_type;

        public string _namespace;
        public string name;

        public static string GetNamespace(string full_name)
        {
            if (full_name.Contains("."))
                return full_name.Substring(0, full_name.LastIndexOf("."));
            return "";
        }

        public static string GetName(string full_name)
        {
            if (full_name.Contains("."))
                return full_name.Substring(full_name.LastIndexOf(".") + 1);
            return full_name;
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
        //*****************变量***********************/
        public string field_type_fullname= "";
        //********************************************/

        //*****************方法***********************/
        public class Argument
        {
            public string type_fullname;
            public string name;
            public bool is_ref;
            public bool is_out;
            public string default_value = "";
        }
        public Argument[] method_args;

        public string method_ret_type = "";

        public DB_BlockSyntax method_body;
        //********************************************/

        //签名，一个类唯一
        public string identifier
        {
            get
            {
                if(member_type == (int)MemberTypes.Field)
                {
                    return name;
                }
                else if(member_type == (int)MemberTypes.Method)
                {
                    StringBuilder sb = new StringBuilder(name);
                    sb.Append("(");
                    for (int i = 0; i < method_args.Length; i++)
                    {
                        sb.Append(method_args[i].type_fullname);
                        if (i < method_args.Length - 1)
                            sb.Append(",");
                    }
                    sb.Append(")");
                    return sb.ToString();
                }
                else
                {
                    return name;
                }
            }
        }
    }


    public class JsonConverterType<TBase>:Newtonsoft.Json.JsonConverter where TBase:class
    {
        //
        // 摘要:
        //     Determines whether this instance can convert the specified object type.
        //
        // 参数:
        //   objectType:
        //     Type of the object.
        //
        // 返回结果:
        //     true if this instance can convert the specified object type; otherwise, false.
        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsArray)
                return false;
            return typeof(TBase).IsAssignableFrom(objectType);
        }
        //
        // 摘要:
        //     Reads the JSON representation of the object.
        //
        // 参数:
        //   reader:
        //     The Newtonsoft.Json.JsonReader to read from.
        //
        //   objectType:
        //     Type of the object.
        //
        //   existingValue:
        //     The existing value of object being read.
        //
        //   serializer:
        //     The calling serializer.
        //
        // 返回结果:
        //     The object value.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        TBase Create(Type objectType, JObject jsonObject)
        {
            var typeName = jsonObject["$type"].ToString();
            return System.Activator.CreateInstance(Type.GetType(typeName)) as TBase; 
        }

        //
        // 摘要:
        //     Writes the JSON representation of the object.
        //
        // 参数:
        //   writer:
        //     The Newtonsoft.Json.JsonWriter to write to.
        //
        //   value:
        //     The value.
        //
        //   serializer:
        //     The calling serializer.
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = JObject.FromObject(value);
            jo.Add("$type", value.GetType().Name);
            jo.WriteTo(writer, serializer.Converters.ToArray());
        }
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

        public static T ReadObject<T>(string json) where T:class
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonConverterType<T>());
        }
        public static string WriteObject<T>(T v) where T : class
        {
            return JsonConvert.SerializeObject(v, new JsonConverterType<T>());
        }


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
                cmd.Parameters.AddWithValue("1", type.imports == null ? "" : WriteObject(type.imports));
                cmd.Parameters.AddWithValue("2", type.ext);
                cmd.Parameters.AddWithValue("3", type.is_value_type);
                cmd.Parameters.AddWithValue("4", type.is_interface);
                cmd.ExecuteNonQuery();
            }
        }

        public static void SaveDBMember(DB_Member member, OdbcConnection _con, OdbcTransaction _trans)
        {
            //{
            //    string cmdText = string.Format("delete from member where declaring_type='{0}' and name='{1}' and child=?", member.declaring_type, member.name);
            //    OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
            //    cmd.Parameters.AddWithValue("1", member.child);
            //    cmd.ExecuteNonQuery();
            //}

            {
                string CommandText = string.Format("insert into member(declaring_type,identifier,name,comments,modifier,is_static,member_type,ext,field_type_fullname,method_args,method_ret_type,method_body) values(\"{0}\",\"{1}\",\"{2}\",\"{3}\",{4},{5},\"{6}\",\"{7}\",?,?,?,?);",
                member.declaring_type, member.identifier, member.name, member.comments, member.modifier, member.is_static, member.member_type, member.ext);

                OdbcCommand cmd = new OdbcCommand(CommandText, _con, _trans);

                cmd.Parameters.AddWithValue("1", member.field_type_fullname);
                cmd.Parameters.AddWithValue("2", WriteObject( member.method_args));
                cmd.Parameters.AddWithValue("3", member.method_ret_type);
                cmd.Parameters.AddWithValue("4", WriteObject(member.method_body));


                cmd.ExecuteNonQuery();
            }

        }

        public static Dictionary<string,DB_Type> Load(string ns, OdbcConnection _con)
        {
            Dictionary<string, DB_Type> results = new Dictionary<string, DB_Type>();
            string cmdText = string.Format("select * from type where full_name like ?%");
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            cmd.Parameters.AddWithValue("1", ns);
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                DB_Type type = new DB_Type();
                type.full_name = (string)reader["full_name"];
                type.modifier = (int)reader["modifier"];
                type.comments = (string)reader["comments"];
                type.ext = (string)reader["ext"];
                //type.imports = (string)reader["imports"];
                type.is_abstract = (bool)reader["is_abstract"];
                type.is_interface = (bool)reader["is_interface"];
                type.is_value_type = (bool)reader["is_value_type"];
                type._parent = (string)reader["parent"];
                results.Add(type.name, type);
            }

            return results;
        }
    }
}
