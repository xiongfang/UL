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

        public Dictionary<string, DB_Member> members = new Dictionary<string, DB_Member>();

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
    
        public string typeName
        {
            get
            {
                if (member_type == (int)MemberTypes.Field)
                    return field_type_fullname;
                else if (member_type == (int)MemberTypes.Method)
                    return method_ret_type;

                return "void";
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
        public override bool CanWrite
        {
            get
            {
                return false;   
            }
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
            JToken jToken = null;
            if(jsonObject.TryGetValue("$Type",out jToken))
            {
                string typeName = jToken.ToString();
                return System.Activator.CreateInstance(Type.GetType(typeName)) as TBase;
            }
            return System.Activator.CreateInstance(objectType) as TBase;
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
            throw new NotImplementedException();
            //serializer.Serialize(writer, value);
            //JObject jo = JObject.FromObject(value);
            //jo.Add("$type", value.GetType().FullName);
            //jo.WriteTo(writer, serializer.Converters.ToArray());
        }
    }

    [JsonConverter(typeof(JsonConverterType<DB_Syntax>))]
    public class DB_Syntax
    {
        [JsonProperty("$Type")]
        public string JsonType
        {
            get
            {
                return GetType().FullName;
            }
        }
    }

    //语句
    [JsonConverter(typeof(JsonConverterType<DB_StatementSyntax>))]
    public class DB_StatementSyntax:DB_Syntax
    {
       
    }
    [JsonConverter(typeof(JsonConverterType<DB_BlockSyntax>))]
    public class DB_BlockSyntax: DB_StatementSyntax
    {
        public List<DB_StatementSyntax> List = new List<DB_StatementSyntax>();
    }
    [JsonConverter(typeof(JsonConverterType<DB_IfStatementSyntax>))]
    public class DB_IfStatementSyntax:DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
        public DB_StatementSyntax Else;
    }
    [JsonConverter(typeof(JsonConverterType<DB_ExpressionStatementSyntax>))]
    public class DB_ExpressionStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Exp;
    }
    [JsonConverter(typeof(JsonConverterType<DB_LocalDeclarationStatementSyntax>))]
    public class DB_LocalDeclarationStatementSyntax : DB_StatementSyntax
    {
        public string Type;
        public List<VariableDeclaratorSyntax> Variables = new List<VariableDeclaratorSyntax>();

    }
    [JsonConverter(typeof(JsonConverterType<VariableDeclarationSyntax>))]
    public sealed class VariableDeclarationSyntax : DB_Syntax
    {
        public string Type;
        public List<VariableDeclaratorSyntax> Variables = new List<VariableDeclaratorSyntax>();
    }
    [JsonConverter(typeof(JsonConverterType<VariableDeclaratorSyntax>))]
    public class VariableDeclaratorSyntax : DB_Syntax
    {
        public string Identifier;
        public Expression.Exp Initializer;
    }

    [JsonConverter(typeof(JsonConverterType<DB_ForStatementSyntax>))]
    public class DB_ForStatementSyntax : DB_StatementSyntax
    {
        public VariableDeclarationSyntax Declaration;
        public Expression.Exp Condition;
        public List<Expression.Exp> Incrementors = new List<Expression.Exp>();
        public DB_StatementSyntax Statement;
    }

    [JsonConverter(typeof(JsonConverterType<DB_DoStatementSyntax>))]
    public class DB_DoStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
    }
    [JsonConverter(typeof(JsonConverterType<DB_WhileStatementSyntax>))]
    public class DB_WhileStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
    }

    namespace Expression
    {
        [JsonConverter(typeof(JsonConverterType<Exp>))]
        public class Exp : DB_Syntax
        {

        }
        [JsonConverter(typeof(JsonConverterType<MethodExp>))]
        public class MethodExp : Exp
        {
            //调用函数的对象，或者类，如果为null，表示创建Name类型的对象
            public Exp Caller;
            //调用的函数名
            //public string Name;

            //调用的参数
            public List<Exp> Args = new List<Exp>();
        }
        [JsonConverter(typeof(JsonConverterType<FieldExp>))]
        public class FieldExp : Exp
        {
            //调用函数的对象，或者类，如果为null，表示访问本地变量，成员变量，全局类
            public Exp Caller;
            //调用的函数名
            public string Name;
        }

        //常量表达式(分为字符常量或者数值常量)
        [JsonConverter(typeof(JsonConverterType<ConstExp>))]
        public class ConstExp : Exp
        {
            public string value;
        }


        //变量访问表达式（可能是本地变量，成员变量，类）
        //public class VariableExp : Exp
        //{
        //    public string Name;
        //}

        //对象创建表达式
        [JsonConverter(typeof(JsonConverterType<ObjectCreateExp>))]
        public class ObjectCreateExp : Exp
        {
            //类型名称
            public string Type;
            //调用的参数
            public List<Exp> Args = new List<Exp>();
        }
    }




    ////表达式
    //[JsonConverter(typeof(JsonConverterType<DB_ExpressionSyntax>))]
    //public class DB_ExpressionSyntax : DB_Syntax
    //{
    //}


    //[JsonConverter(typeof(JsonConverterType<VariableDeclaratorSyntax>))]
    //public class VariableDeclaratorSyntax : DB_Syntax
    //{
    //    public string Identifier;
    //    //public List<DB_ArgumentSyntax> ArgumentList = new List<DB_ArgumentSyntax>();
    //    public DB_ExpressionSyntax Initializer;
    //}

    //[JsonConverter(typeof(JsonConverterType<DB_LiteralExpressionSyntax>))]
    //public class DB_LiteralExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public string token;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_MemberAccessExpressionSyntax>))]
    //public class DB_MemberAccessExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Exp;
    //    public string name;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_ArgumentSyntax>))]
    //public class DB_ArgumentSyntax:DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Expression;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_InvocationExpressionSyntax>))]
    //public class DB_InvocationExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Exp;
    //    public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();

    //}
    //[JsonConverter(typeof(JsonConverterType<DB_IdentifierNameSyntax>))]
    //public class DB_IdentifierNameSyntax : DB_ExpressionSyntax
    //{
    //    public string Name;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_InitializerExpressionSyntax>))]
    //public class DB_InitializerExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public List<DB_ExpressionSyntax> Expressions = new List<DB_ExpressionSyntax>();
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_ObjectCreationExpressionSyntax>))]
    //public class DB_ObjectCreationExpressionSyntax:DB_ExpressionSyntax
    //{
    //    public string Type;
    //    public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();
    //    public DB_InitializerExpressionSyntax Initializer;
    //}

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
            if (string.IsNullOrEmpty(json))
                return null;
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.Converters.Add(new JsonConverterType<T>());
            return JsonConvert.DeserializeObject<T>(json, jsetting);
        }
        public static string WriteObject<T>(T v) where T : class
        {
            if (v == null)
                return "";
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.Converters.Add(new JsonConverterType<T>());
            return JsonConvert.SerializeObject(v, jsetting);
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

        public static Dictionary<string,DB_Type> LoadNamespace(string ns, OdbcConnection _con)
        {
            Dictionary<string, DB_Type> results = new Dictionary<string, DB_Type>();
            string cmdText = string.Format("select * from type where full_name like '{0}%'", ns);
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            //cmd.Parameters.AddWithValue("1", ns);
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                DB_Type type = ReadType(reader);
                type.members = LoadMembers(type.full_name, _con);
            }

            return results;
        }

        public static DB_Type LoadType(string full_name, OdbcConnection _con)
        {
            string cmdText = string.Format("select * from type where full_name = '{0}'", full_name);
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            //cmd.Parameters.AddWithValue("1", ns);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                DB_Type type = ReadType(reader);
                type.members = LoadMembers(type.full_name, _con);
                return type;
            }

            return null;
        }

        static DB_Type ReadType(OdbcDataReader reader)
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

            return type;
        }

        public static Dictionary<string,DB_Member> LoadMembers(string type, OdbcConnection _con)
        {
            Dictionary<string, DB_Member> results = new Dictionary<string, DB_Member>();
            string cmdText = string.Format("select * from member where declaring_type = ?");
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            cmd.Parameters.AddWithValue("1", type);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DB_Member member = new DB_Member();
                member.declaring_type = type;
                member.comments = (string)reader["comments"];
                member.ext = (string)reader["ext"];
                member.field_type_fullname = (string)reader["field_type_fullname"];
                member.is_static = (bool)reader["is_static"];
                member.modifier = (int)reader["modifier"];
                member.name = (string)reader["name"];
                member.member_type = (int)reader["member_type"];
                member.method_args = ReadObject<DB_Member.Argument[]>((string)reader["method_args"]);
                member.method_body = ReadObject<DB_BlockSyntax>((string)reader["method_body"]);
                member.method_ret_type = (string)reader["method_ret_type"];
                results.Add(member.identifier, member);
            }

            return results;
        }
    }
}
