using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{

    public class ModelData
    {
        public static Action<ULTypeInfo> onAddType;
        public static Action<ULTypeInfo> onRemoveType;

        public const string GloableNamespaceName = "gloable";

        static Dictionary<string, ULTypeInfo> TypeNames = new Dictionary<string, ULTypeInfo>();


        public static List<ULTypeInfo> GetTypeList() { return new List<ULTypeInfo>(TypeNames.Values); }

        public static ULTypeInfo FindTypeByFullName(string full_name)
        {
            if (string.IsNullOrEmpty(full_name))
                return null;
            if(TypeNames.TryGetValue(full_name,out var v))
            {
                return v;
            }
            return null;
        }
        public static ULTypeInfo FindTypeInNamepsace(string name,List<string> namespaceList)
        {
            foreach(var t in TypeNames.Values)
            {
                if(t.Name == name && namespaceList.Contains(t.Namespace))
                {
                    return t;
                }
            }
            return null;
        }

        public static bool HasNamepsace(string ns)
        {
            foreach (var t in TypeNames.Values)
            {
                if (t.Namespace == ns)
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddType(ULTypeInfo typeInfo)
        {
            typeInfo.onNameChanged = (last, newName) =>
            {
                TypeNames.Remove(typeInfo.Namespace+"."+ last);
                TypeNames.Add(typeInfo.Namespace + "." + newName, typeInfo);

            };
            TypeNames.Add(typeInfo.FullName, typeInfo);
            onAddType?.Invoke(typeInfo);
        }

        public static void RemoveType(string FullName)
        {
            if(TypeNames.TryGetValue(FullName,out var t))
            {
                t.onNameChanged = null;
                TypeNames.Remove(FullName);
                onRemoveType?.Invoke(t);
            }
            
        }

        public static void UpdateType(ULTypeInfo typeInfo)
        {
            RemoveType(typeInfo.FullName);
            AddType(typeInfo);
        }


        public static void Save()
        {
            try
            {
                var fs = new System.IO.FileStream("model.dat", System.IO.FileMode.OpenOrCreate);
                //List<ULTypeInfo> type_list = new List<ULTypeInfo>(TypeNames.Values);
                MongoDB.Bson.Serialization.BsonSerializer.Serialize(new MongoDB.Bson.IO.BsonBinaryWriter(fs), TypeNames);
                fs.SetLength(fs.Position);
                fs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Load()
        {
            if(!System.IO.File.Exists("model.dat"))
            {
                return;
            }
            try
            {
                var fs = new System.IO.FileStream("model.dat", System.IO.FileMode.Open);
                TypeNames = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Dictionary<string,ULTypeInfo>>(fs);
                List<ULTypeInfo> type_list = new List<ULTypeInfo>(TypeNames.Values);
                TypeNames.Clear();
                foreach(var t in type_list)
                {
                    TypeNames[t.FullName] = t;
                }
                fs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    //名称规范:
    //命名空间.类.成员.（参数，局部变量）
    //global. 全局类，无命名空间
    //local.局部变量，包括参数
    //output.id[0] 节点输出参数
    //this.成员
    //base.父类的方法


    public enum EModifier
    {
        Public,
        Protected,
        Private,
    }

    [BsonIgnoreExtraElements]
    public class ULTypeInfo
    {
        [BsonIgnore]
        public System.Action<string, string> onNameChanged;

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string id { get; set; }

        [BsonElement("Name")]
        string _name { get; set; }

        [BsonIgnore]
        public string Name { get { return _name; } set { if (_name != value) { string tempName = _name; _name = value; onNameChanged?.Invoke(tempName, _name); } } }

        public string Namespace { get; set; }

        public string Parent { get; set; }

        public EModifier ExportType { get; set; }

        public bool IsValueType { get; set; }

        public bool IsInterface { get; set; }

        public bool IsEnum { get; set; }

        public List<ULMemberInfo> Members = new List<ULMemberInfo>();

        public string FullName { 
            get
            {
                return Namespace + "." + Name;
            } 
        }

    }


    public class ULMemberInfo
    {

        public string DeclareTypeName { get; set; }

        [BsonIgnore]
        public ULTypeInfo DeclareType { get { return Model.ModelData.FindTypeByFullName(DeclareTypeName); } }


        public string TypeName { get; set; }

        [BsonIgnore]
        public ULTypeInfo Type { get { return Model.ModelData.FindTypeByFullName(TypeName); } }


        public string Name { get; set; }

        [BsonIgnore]
        public string FullName { get { return DeclareTypeName + "." + Name; } }


        public EModifier Modifier { get; set; }


        public bool IsStatic { get; set; }


        public Dictionary<string, string> Ext { get; set; }


        public enum EMemberType
        {
            Field,
            Property,
            Method,
            Event,
            Enum,
            PropertyGet,
            PropertySet,
            PropertyAdd,
            PropertyRemove
        }
        
        public string Name_PropertyGet { get { return Name + "_get"; } }
        public string Name_PropertySet { get { return Name + "_set"; } }
        public string Name_PropertyAdd { get { return Name + "_add"; } }
        public string Name_PropertyRemove { get { return Name + "_remove"; } }

        public EMemberType MemberType { get; set; }

        public class MethodArg
        {
            public string TypeName { get; set; }
            public string ArgName { get; set; }
        }
        public List<MethodArg> Args { get; set; }
        public ULNodeBlock MethodBody;
    }

    
    [BsonKnownTypes(typeof(ULStatementSwitch))]
    [BsonKnownTypes(typeof(ULStatementDo))]
    [BsonKnownTypes(typeof(ULStatementReturn))]
    [BsonKnownTypes(typeof(ULCall))]
    [BsonKnownTypes(typeof(ULStatementBreak))]
    [BsonKnownTypes(typeof(ULStatementFor))]
    [BsonKnownTypes(typeof(ULStatementWhile))]
    [BsonKnownTypes(typeof(ULStatementIf))]
    [BsonKnownTypes(typeof(ULNodeBlock))]
    public class ULStatement
    {
        [BsonIgnore]
        public ULNodeBlock Parent;
    }


    public class ULNodeBlock: ULStatement
    {

        public List<ULStatement> statements = new List<ULStatement>();


        void AfterDeserialization()
        {
            foreach(var s in statements)
            {
                s.Parent = this;
            }
        }
    }


    public class ULStatementIf : ULStatement
    {
        public string arg { get; set; }
        public ULNodeBlock trueBlock { get; set; }
        public ULNodeBlock falseBlock { get; set; }
    }

    public class ULStatementWhile : ULStatement
    {
        public string arg { get; set; }
        public ULNodeBlock block { get; set; }
    }

    public class ULStatementFor : ULStatement
    {
        public string Declaration { get; set; }
        public string Condition { get; set; }
        public List<string> Incrementors { get; set; }
        public ULNodeBlock block { get; set; }
    }

    public class ULStatementBreak : ULStatement
    {

    }

    public class ULStatementReturn : ULStatement
    {
        public string Arg;
    }
    public class ULStatementDo : ULStatement
    {
        public string arg { get; set; }
        public ULNodeBlock block { get; set; }
    }
    public class ULStatementSwitch : ULStatement
    {
        public string Condition { get; set; }
        public class Section
        {
            public List<string> Labels { get; set; }
            public List<ULNodeBlock> Statements { get; set; }
        }
        public List<Section> Sections { get; set; }
    }
    public class ULCall : ULStatement
    {
        public ULCall()
        {
            id = Guid.NewGuid().ToString();
        }
        public string id { get; set; }

        public string Name { get; set; }

        List<string> _args;
        public List<string> Args
        {
            get
            {
                if (_args == null)
                {
                    _args = new List<string>();
                }
                return _args;
            }
            set
            {
                _args = value;
            }
        }

        public string GetOutputName(int index)
        {
            return "output." + id + "[" + index + "]";
        }

        public enum ECallType
        { 
            Method,
            //GetProperty,
            //SetProperty,
            Constructor,
            GetField,
            SetField,
            GetLocal,
            SetLocal,
            GetArg,
            SetArg,
            GetThis,
            Const,
            Assign,
            Identifier,
            CreateArray,
            GetBase,
            ElementAccess,
            DeclarationLocal
        }

        public ECallType callType { get; set; }
    }
}
