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

        public Dictionary<string,ULMemberInfo> Members = new Dictionary<string, ULMemberInfo>();

    }
    [Serializable]
    public class ULMemberInfo
    {
        public string ReflectTypeId;
        public string MemberTypeId;
        public string Name;
        public EExportType ExportType;

        public bool IsStatic;

        public enum EMemberType
        {
            Field,
            Property,
            Method
        }

        
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
