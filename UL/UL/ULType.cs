using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Data
    {

        public static Dictionary<string, ULTypeInfo> types = new Dictionary<string, ULTypeInfo>();
        public static Dictionary<string, ULMemberInfo> members = new Dictionary<string, ULMemberInfo>();
        public static Dictionary<string,ULGraph> graphics = new Dictionary<string,ULGraph>();

        public static ULTypeInfo GetType(string id)
        {
            if(types.TryGetValue(id,out var t))
            {
                return t;
            }
            return null;
        }
    }

    public class ULTypeInfo
    {
        public string ID { get; set; }

        public string Name { get; set; }
        public string Namespace { get; set; }

        //修饰符：0 public,1 protected,2 private
        public int Modifier { get; set; }

        public ULMemberInfo[] Members { 
            get
            {
                var r =  Data.members.Select((v) => v.Value).Where((v)=>v.DeclareTypeID == ID);
                return r.ToArray();
            } 
        }
    }

    public class ULMemberInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string DeclareTypeID { get; set; }

        public string TypeID { get; set; }

        //修饰符：0 public,1 protected,2 private
        public int Modifier { get; set; }

        public bool IsStatic { get; set; }


        public enum EMemberType
        {
            Field,
            Property,
            Method,
            Event,
            Enum
        }

        public EMemberType MemberType { get; set; }

        //属性
        public string PropertyGetMethodID { get; set; }
        public string PropertySetMethodID { get; set; }

        //方法
        public enum EMethodType
        {
            None,
            Constructor,
            PropertyGet,
            PropertySet
        }
        public EMethodType MethodType { get; set; }
        public bool MethodIsVirtual { get; set; }
        public string[][] MethodArgs { get; set; }
        public ULGraph Graph { 
            get
            {
                if (Data.graphics.TryGetValue(ID, out var v))
                    return v;
                return null;
            }
        }

    }

    //参数输入：
    //__type.id
    //__method.id
    //__arg.name
    //__node.[id].output[0] 节点输出参数

    public class ULNode
    {
        public string NodeID { get; set; }          //节点ID，每个方法体内部唯一
        public string Name { get; set; }            //调用的方法ID，或者特殊关键字节点
        public string[] Args { get; set; }          //参数输入类型：常量，某个节点的输出

        public string[] Inputs { get; set; }        //控制输入
        public string[] Outputs { get; set; }       //控制输出

        public int X { get; set; }
        public int Y { get; set; }

        public static readonly string[] keywords = { "if","switch","while","do","loop","for" };
    }


    public class ULGraph
    {
        public string MethodID;       //所属的方法ID
        public int OffsetX;
        public int OffsetY;
        public List<ULNode> Nodes = new List<ULNode>();
    }
}
