using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [System.ComponentModel.ReadOnly(true)]
        public string ID { get; set; }

        public string Name { get; set; }
        public string Namespace { get; set; }

        //修饰符：0 public,1 protected,2 private
        public int Modifier { get; set; }

        [System.ComponentModel.Browsable(false)]
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
        [System.ComponentModel.ReadOnly(true)]
        public string ID { get; set; }
        public string Name { get; set; }
        [System.ComponentModel.ReadOnly(true)]
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

        //[System.ComponentModel.Browsable(false)]
        public ULGraph Graph { 
            get
            {
                if (Data.graphics.TryGetValue(ID, out var v))
                {
                    return v;
                }
                    
                var g = new ULGraph();
                g.MethodID = ID;
                Data.graphics[ID] = g;
                return g;
            }
            //set
            //{
            //    if (value != null)
            //    {
            //        value.MethodID = ID;
            //        Data.graphics[ID] = value;
            //    }
            //    else
            //    {
            //        Data.graphics.Remove(ID);
            //    }
            //}
        }

    }

    //参数输入：
    //__type.id
    //__method.id
    //__arg.name
    //__node.[id].output[0] 节点输出参数

    public class ULNode
    {
        [System.ComponentModel.ReadOnly(true)]
        public string NodeID { get; set; }          //节点ID，每个方法体内部唯一
        string _Name = "";
        public string Name { get => _Name; set => _Name = value; }            //调用的方法ID，或者特殊关键字节点
        [System.ComponentModel.Browsable(false)]
        public string[] Inputs { get; set; }          //参数输入类型：常量，某个节点的输出
        [System.ComponentModel.Browsable(false)]
        public string[] ControlInputs { get; set; }        //控制输入
        [System.ComponentModel.Browsable(false)]
        public string[] ControlOutputs { get; set; }       //控制输出
        [System.ComponentModel.Browsable(false)]
        public int X { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int Y { get; set; }

        public static readonly string[] keywords = { "if","switch","while","do","loop","for" };
    }

    class CustomExpandableObjectConverter:ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return false;
            return base.CanConvertTo(context, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return false;
            return base.CanConvertFrom(context, sourceType);
        }
    }

    [TypeConverter(typeof(CustomExpandableObjectConverter))]
    public class ULGraph
    {
        [ReadOnly(true)]
        public string MethodID { get; set; }       //所属的方法ID
        [Browsable(false)]
        public int OffsetX { get; set; }
        [Browsable(false)]
        public int OffsetY { get; set; }
        List<string[]> _Args = new List<string[]>();
        public List<string[]> Args { get => _Args; set => _Args = value; }
        public List<string[]> Outputs = new List<string[]>();
        public List<ULNode> Nodes = new List<ULNode>();
    }
}
