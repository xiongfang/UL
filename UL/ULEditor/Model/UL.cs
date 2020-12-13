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

    public enum EExportScope
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

        string _name;

        public string Name { get { return _name; } set { if (_name != value) { string tempName = _name; _name = value; onNameChanged?.Invoke(tempName, _name); } } }

        public string Namespace { get; set; }

        public string Parent { get; set; }

        public EExportScope ExportType { get; set; }

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

        public string ReflectTypeName;

        [BsonIgnore]
        public ULTypeInfo ReflectType { get { return Model.ModelData.FindTypeByFullName(ReflectTypeName); } }


        public string MemberTypeName;
        [BsonIgnore]
        public ULTypeInfo MemberType { get { return Model.ModelData.FindTypeByFullName(MemberTypeName); } }


        public string Name { get; set; }

        [BsonIgnore]
        public string FullName { get { return ReflectTypeName + "." + Name; } }


        public EExportScope ExportType { get; set; }


        public bool IsStatic { get; set; }


        public Dictionary<string, string> Ext { get; set; }


        public enum EMemberMark
        {
            Field,
            Property,
            Method,
            Event
        }
        

        public EMemberMark MemberMark { get; set; }


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

        public string Member;


        public string[] Args;

        public string[] Outputs;


        public bool get; //get or set
    }
}
