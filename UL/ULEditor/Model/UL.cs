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
                fs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

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

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

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
            Enum
        }
        

        public EMemberType MemberType { get; set; }


        public ULNodeBlock MethodBody;
    }


    [BsonKnownTypes(typeof(ULCall))]
    [BsonKnownTypes(typeof(ULStatementBreak))]
    [BsonKnownTypes(typeof(ULStatementFor))]
    [BsonKnownTypes(typeof(ULStatementWhile))]
    [BsonKnownTypes(typeof(ULStatementIf))]
    [BsonKnownTypes(typeof(ULNodeBlock))]
    public class UIStatement
    {
        [BsonIgnore]
        public ULNodeBlock Parent;
    }


    public class ULNodeBlock: UIStatement
    {

        public List<UIStatement> statements = new List<UIStatement>();


        void AfterDeserialization()
        {
            foreach(var s in statements)
            {
                s.Parent = this;
            }
        }
    }


    public class ULStatementIf : UIStatement
    {

    }

    public class ULStatementWhile : UIStatement
    {


    }

    public class ULStatementFor : UIStatement
    {

    }

    public class ULStatementBreak : UIStatement
    {

    }


    public class ULCall : UIStatement
    {

        public string Member { get; set; }


        public string[] Args { get; set; }

        public string[] Outputs { get; set; }


        public enum ECallType
        { 
            Method,
            GetProperty,
            SetProperty,
            Constructor,
            GetField,
            SetField,
        }

        public ECallType callType { get; set; }
    }
}
