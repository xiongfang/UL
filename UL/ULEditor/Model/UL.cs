using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{

    public class ModelData
    {
        public static Dictionary<string, ULTypeInfo> Types = new Dictionary<string, ULTypeInfo>();

        public static void Save()
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var fs = new System.IO.FileStream("model.dat", System.IO.FileMode.OpenOrCreate);
            bf.Serialize(fs, Types);
            fs.Close();
        }

        public static void Load()
        {
            if(!System.IO.File.Exists("model.dat"))
            {
                return;
            }

            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var fs = new System.IO.FileStream("model.dat", System.IO.FileMode.Open);
            Types = bf.Deserialize(fs) as Dictionary<string, ULTypeInfo>;
            fs.Close();
        }
    }
    [Serializable]
    public enum EExportType
    {
        Public,
        Protected,
        Private,
    }

    [Serializable]
    public class ULTypeInfo
    {
        string _guid;

        public void SetGUID(string newGuid) { _guid = newGuid; }
        public string Guid { get { return _guid; } }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Parent { get; set; }

        public EExportType ExportType { get; set; }

        public List<ULMemberInfo> Members = new List<ULMemberInfo>();

    }
    [Serializable]
    public class ULMemberInfo
    {
        public ULMemberInfo(ULTypeInfo type)
        {
            _ref_guid = type.Guid;
        }

        string _ref_guid;
        public string ReflectTypeId { get { return _ref_guid; } }

        public ULTypeInfo ReflectType {
            get {
                if (string.IsNullOrEmpty(_ref_guid))
                    return null;

                if (ModelData.Types.TryGetValue(_ref_guid, out var v))
                {
                    return v;
                }
                return null;
            }
        }

        public string MemberTypeId { get; set; }

        public ULTypeInfo MemberType
        {
            get
            {
                if (string.IsNullOrEmpty(MemberTypeId))
                    return null;
                if (ModelData.Types.TryGetValue(MemberTypeId, out var v))
                {
                    return v;
                }
                return null;
            }
        }

        public string Name { get; set; }
        public EExportType ExportType { get; set; }

        public bool IsStatic { get; set; }

        public Dictionary<string, string> Ext { get; set; }
        public enum EMemberType
        {
            Field,
            Property,
            Method
        }

        public EMemberType type { get; set; }
    }
    [Serializable]
    public class ULStatement
    {

    }
    [Serializable]
    public class ULExp
    {

    }
}
