using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{

    public class ModelData
    {
        public const string GloableNamespaceName = "gloable";

        static ULTypeInfo _void = new ULTypeInfo();
        public static ULTypeInfo Void { get { return _void; } }


        static Dictionary<string, ULTypeInfo> Types = new Dictionary<string, ULTypeInfo>();
        static Dictionary<string, ULTypeInfo> TypeNames = new Dictionary<string, ULTypeInfo>();


        public static List<ULTypeInfo> GetTypeList() { return new List<ULTypeInfo>(Types.Values); }
        public static ULTypeInfo FindTypeByFullName(string full_name)
        {
            if(TypeNames.TryGetValue(full_name,out var v))
            {
                return v;
            }
            return null;
        }
        public static ULTypeInfo FindTypeByGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return _void;
            if (Types.TryGetValue(guid, out var v))
            {
                return v;
            }
            return null;
        }

        public static void AddType(ULTypeInfo typeInfo)
        {
            typeInfo.onNameChanged += (last, newName) =>
            {
                TypeNames.Remove(typeInfo.Namespace+"."+ last);
                TypeNames.Add(typeInfo.Namespace + "." + newName, typeInfo);

            };
            Types.Add(typeInfo.Guid, typeInfo);
            TypeNames.Add(typeInfo.FullName, typeInfo);
        }
        public static void RemoveType(ULTypeInfo typeInfo)
        {
            RemoveType(typeInfo.Guid);
        }
        public static void RemoveType(string Guid)
        {
            if(Types.TryGetValue(Guid,out var typeInfo))
            {
                Types.Remove(typeInfo.Guid);
                TypeNames.Remove(typeInfo.FullName);
            }
        }

        public static void UpdateType(ULTypeInfo typeInfo)
        {
            RemoveType(typeInfo);
            AddType(typeInfo);
        }


        public static void Save()
        {
            try
            {
                var fs = new System.IO.FileStream("model.dat", System.IO.FileMode.OpenOrCreate);
                List<ULTypeInfo> type_list = new List<ULTypeInfo>(Types.Values);
                ProtoBuf.Serializer.Serialize(fs, type_list);
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
                var type_list = ProtoBuf.Serializer.Deserialize<List<ULTypeInfo>>(fs);
                foreach(var t in type_list)
                {
                    AddType(t);
                }
                fs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            _void.Name = "Void";
            _void.Namespace = "System";
            _void.SetGUID("0");
            if (!Types.ContainsKey("0"))
            {
                AddType(_void);
            }
            else
            {
                _void = Types["0"];
            }
        }
    }
    [ProtoBuf.ProtoContract]
    public enum EExportType
    {
        Public,
        Protected,
        Private,
    }

    [ProtoBuf.ProtoContract]
    public class ULTypeInfo
    {
        public System.Action<string, string> onNameChanged; 

        [ProtoBuf.ProtoMember(1)]
        string _guid;

        public void SetGUID(string newGuid) { _guid = newGuid; }

        
        public string Guid { get { return _guid; } }



        [ProtoBuf.ProtoMember(2)]
        string _name;

        public string Name { get { return _name; } set { if (_name != value) { string tempName = _name; _name = value; onNameChanged?.Invoke(tempName, _name); } } }
        [ProtoBuf.ProtoMember(3)]
        public string Namespace { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public string Parent { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public EExportType ExportType { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public List<ULMemberInfo> Methods = new List<ULMemberInfo>();

        public string FullName { 
            get
            {
                return Namespace + "." + Name;
            } 
        }

    }

    [ProtoBuf.ProtoContract]
    public class ULMemberInfo
    {
        public ULMemberInfo() { }

        public ULMemberInfo(ULTypeInfo type)
        {
            _ref_guid = type.Guid;
        }

        //[ProtoBuf.ProtoMember(10)]
        //string _guid;

        //public string Guid { get { return _guid; } }

        //public void SetGuid(string g) { _guid = g; }

        [ProtoBuf.ProtoMember(1)]
        string _ref_guid;

        
        public string ReflectTypeId { get { return _ref_guid; } }

        public ULTypeInfo ReflectType {
            get {
                return ModelData.FindTypeByGuid(_ref_guid);
            }
        }
        [ProtoBuf.ProtoMember(2)]
        public string TypeId { get; set; }

        public ULTypeInfo Type
        {
            get
            {
                return ModelData.FindTypeByGuid(TypeId);
            }
        }
        [ProtoBuf.ProtoMember(3)]
        public string Name { get; set; }

        [ProtoBuf.ProtoMember(4)]
        public EExportType ExportType { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public bool IsStatic { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public Dictionary<string, string> Ext { get; set; }
        [ProtoBuf.ProtoContract]
        public enum EMemberType
        {
            Field,
            Property,
            Method,
            Event
        }
        
        [ProtoBuf.ProtoMember(7)]
        public EMemberType MemberType { get; set; }

        [ProtoBuf.ProtoMember(11)]
        public string PropertyGetMethod { get; set; }
        [ProtoBuf.ProtoMember(12)]
        public string PropertySetMethod { get; set; }

        [ProtoBuf.ProtoMember(13)]
        public ULStatementBlock MethodBody;
    }

    [ProtoBuf.ProtoInclude(105, typeof(ULStatementBreak))]
    [ProtoBuf.ProtoInclude(104, typeof(ULStatementFor))]
    [ProtoBuf.ProtoInclude(103, typeof(ULStatementWhile))]
    [ProtoBuf.ProtoInclude(102, typeof(ULStatementIf))]
    [ProtoBuf.ProtoInclude(101, typeof(ULStatementBlock))]
    [ProtoBuf.ProtoContract]
    public class ULStatement
    {
        public ULStatementBlock Parent;
    }

    [ProtoBuf.ProtoContract]
    public class ULStatementBlock: ULStatement
    {
        [ProtoBuf.ProtoMember(1)]
        public List<ULStatement> statements = new List<ULStatement>();

        [ProtoBuf.ProtoAfterDeserialization]
        void AfterDeserialization()
        {
            foreach(var s in statements)
            {
                s.Parent = this;
            }
        }
    }

    [ProtoBuf.ProtoContract]
    public class ULStatementIf : ULStatement
    {

    }
    [ProtoBuf.ProtoContract]
    public class ULStatementWhile : ULStatement
    {

    }
    [ProtoBuf.ProtoContract]
    public class ULStatementFor : ULStatement
    {

    }
    [ProtoBuf.ProtoContract]
    public class ULStatementBreak : ULStatement
    {

    }

    [ProtoBuf.ProtoInclude(101,typeof(ULExpCall))]
    [ProtoBuf.ProtoContract]
    public class ULExp
    {

    }

    [ProtoBuf.ProtoContract]
    public class ULExpCall:ULExp
    {
        [ProtoBuf.ProtoMember(1)]
        public string MemberGuid;
    }

    [ProtoBuf.ProtoContract]
    public class ULExpNew : ULExp
    {

    }
}
