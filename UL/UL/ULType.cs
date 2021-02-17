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
        //public static Dictionary<string, ULMemberInfo> members = new Dictionary<string, ULMemberInfo>();
        //public static Dictionary<string,ULGraph> graphics = new Dictionary<string,ULGraph>();

        public static ULTypeInfo GetType(string id)
        {
            if(types.TryGetValue(id,out var t))
            {
                return t;
            }
            return null;
        }
        public static ULMemberInfo GetMember(string id)
        {
            if (string.IsNullOrEmpty(id) || !id.Contains("."))
                return null;
            var type_id = id.Substring(0, id.LastIndexOf("."));
            var type = GetType(type_id);
            if(type!=null)
            {
                return type.Members.Find((v) => v.ID == id);
            }
            return null;
        }
    }


    public enum EModifier
    {
        Public,
        Protected,
        Private
    }


    public class ULTypeInfo
    {
        public string ID { get { return Namespace + "." + Name; } }
        [System.ComponentModel.ReadOnly(true)]
        public string Name { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string Namespace { get; set; }

        //修饰符：0 public,1 protected,2 private
        public EModifier Modifier { get; set; }

        List<ULMemberInfo> _Members = new List<ULMemberInfo>();
        [System.ComponentModel.Browsable(false)]
        public List<ULMemberInfo> Members { get=> _Members; set=> _Members=value; }


        
    }

    public class ULMemberInfo
    {
        public string ID { get { return DeclareTypeID + "." + Name; } }
        public string Name { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string DeclareTypeID { get; set; }

        public string TypeID { get; set; }

        //修饰符：0 public,1 protected,2 private
        public EModifier Modifier { get; set; }

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
        ULGraph _Graph = new ULGraph();
        public ULGraph Graph { get=> _Graph; set=> _Graph=value; }

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
        //[System.ComponentModel.Browsable(false)]
        List<string> _Inputs = new List<string>();
        public List<string> Inputs { get=> _Inputs; set=> _Inputs = value; }          //参数输入类型：常量，某个节点的输出

        List<string> _ControlInputs = new List<string>();
        [System.ComponentModel.Browsable(false)]
        public List<string> ControlInputs { get=> _ControlInputs; set=> _ControlInputs = value; }        //控制输入

        List<string> _ControlOutputs = new List<string>();
        [System.ComponentModel.Browsable(false)]
        public List<string> ControlOutputs { get=> _ControlOutputs; set=> _ControlOutputs = value; }       //控制输出
        [System.ComponentModel.Browsable(false)]
        public int X { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int Y { get; set; }

        void CheckSize(List<string> list,int size)
        {
            while(list.Count<size)
            {
                list.Add("");
            }
        }

        public void LinkControlTo(ULNode to,int fromControlIndex=0, int toControlIndex=0)
        {
            CheckSize(ControlOutputs, fromControlIndex + 1);
            ControlOutputs[fromControlIndex] = to.NodeID + "." + toControlIndex;
            CheckSize(to.ControlInputs, toControlIndex + 1);
            to.ControlInputs[toControlIndex] = NodeID + "." + fromControlIndex;
        }
        public void UnLinkControlTo(ULNode to, int fromControlIndex = 0, int toControlIndex = 0)
        {
            ControlOutputs[fromControlIndex] = "";
            to.ControlInputs[toControlIndex] = "";
        }
        public void LinkDataTo(ULNode to,int fromIndex=0, int toIndex = 0)
        {
            CheckSize(to.Inputs, toIndex + 1);
            to.Inputs[toIndex] = NodeID + "." + fromIndex;
        }
        public void UnLinkDataTo(ULNode to, int fromIndex = 0, int toIndex = 0)
        {
            to.Inputs[toIndex] = "";
        }

        public enum ENodeType
        {
            Method,
            Control
        }

        public ENodeType Type { get; set; }

        public const string name_if = "if";
        public const string name_switch = "switch";
        public const string name_while = "while";
        public const string name_do = "do";
        public const string name_loop = "loop";
        public const string name_for = "for";
        public const string name_entry = "entry";
        public const string name_const = "const";
        public const string name_getthis = "get_this";
        public static readonly string[] keywords = { name_entry, name_const,name_if, name_switch, name_while, name_do, name_loop, name_for, name_getthis };
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
        //[ReadOnly(true)]
        //public string MethodID { get; set; }       //所属的方法ID
        [Browsable(false)]
        public int OffsetX { get; set; }
        [Browsable(false)]
        public int OffsetY { get; set; }
        List<ULArg> _Args = new List<ULArg>();
        public List<ULArg> Args { get => _Args; set => _Args = value; }
        List<ULArg> _Outputs = new List<ULArg>();
        public List<ULArg> Outputs { get => _Outputs; set => _Outputs = value; }
        public List<ULNode> Nodes = new List<ULNode>();

        public ULNode FindNode(string node_id)
        {
            return Nodes.Find((v) => v.NodeID == node_id);
        }
    }

    [TypeConverter(typeof(CustomExpandableObjectConverter))]
    public class ULArg
    {
        public string TypeID { get; set; }
        public string Name { get; set; }
    }
}
