using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core
{
    public class JSON
    {
        public static object ToObject(string json, Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type, Setting);
			//return fastJSON.JSON.ToObject(json,type);
        }
        public static T ToObject<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, Setting);
			//return fastJSON.JSON.ToObject<T>(json);
        }

        //public class JsonDynamicContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        //{
        //    protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        //    {
        //        List<MemberInfo> RetList = new List<MemberInfo>();
        //        RetList.AddRange(objectType.GetFields(BindingFlags.Public | BindingFlags.Instance));
        //        return RetList;
        //    }
        //}
        static JsonSerializerSettings Setting = new JsonSerializerSettings { /* ContractResolver = new JsonDynamicContractResolver(),*/ NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore, Formatting = Formatting.Indented };

        public static string ToJSON<T>(T o)
        {
			//return fastJSON.JSON.ToJSON(o);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, Setting);
        }
    }
}
