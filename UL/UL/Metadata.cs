using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
    
    public class Metadata
    {
        public string[] MetaStrings;
    }

    public enum EAccessModifier
    {
        Public,
        Protected,
        Private
    }

    public class UL_Member
    {
        public EAccessModifier Modifier;
        public string Type;
        public string Name;
        public bool IsStatic;
        public bool IsInternal;
        public Metadata Metadata;
        [NonSerialized]
        public UL_Type Owner;
    }

    public class UL_Function : UL_Member
    {
        public enum EParamModifier
        {
            In,
            Out,
            Ref
        }
        public struct Parameter
        {
            public EParamModifier Modifier;
            public string Type;
            public string Name;
        }
        public List<Parameter> Params = new List<Parameter>();

        public bool IsVirsual;

        public FunctionBody Body;
    }

    public class UL_Variable : UL_Member
    {
        public bool can_read;
        public bool can_write;
    }


    public class UL_Type
    {
        public List<UL_Function> Functions = new List<UL_Function>();
        public List<UL_Variable> Variables = new List<UL_Variable>();
        public string Name;
        public string ParentName;
        public List<UL_Type> Inners = new List<UL_Type>();
        public EAccessModifier Modifier;

        [NonSerialized]
        public UL_Type Outer;
        [NonSerialized]
        public UL_Type Parent;
    }
}
