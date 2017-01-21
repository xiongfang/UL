using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
    namespace Metadata
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



        public class Sentence
        {
            public enum ESentenceType
            {
                Move,
                List,
                Loop_For,
                Loop_WhileDo,
                Loop_DoWhile,
                If,
                Switch,
                Return
            }

            public ESentenceType Type;
        }


        public abstract class Exp
        {
            public enum EType
            {
                Variable,
                Call
            }

            public EType Type;
        }

        public class Variable : Exp
        {
            public string TypeName;
            public string ObjectName;

        }

        public class Call : Exp
        {
            //调用函数的对象，或者类
            public Variable Object;
            //调用的函数名
            public string Function;

            //调用的参数
            public Exp[] Args;

            //返回值
            public Variable Ret;
        }

        public class Sentence_Assign : Sentence
        {
            //调用函数的对象，或者类
            public Variable A;
            //调用函数的对象，或者类
            public Exp B;
        }

        public class Sentence_Return : Sentence
        {
            public Variable Object;

            public Sentence_Return() { Type = ESentenceType.Return; }
        }

        public class Sentence_List : Sentence
        {
            public Sentence[] Body;

            public Sentence_List() { Type = ESentenceType.List; }
        }

        public class Sentence_List_Loop_For : Sentence
        {
            public Sentence_Assign Start;
            public Exp Condition;
            public Sentence_Assign End;
            public Sentence_List Body;

            public Sentence_List_Loop_For() { Type = ESentenceType.Loop_For; }
        }

        public class Sentence_Loop_WhileDo : Sentence
        {
            public Exp ConditionObject;
            public Sentence_List Body;

            public Sentence_Loop_WhileDo() { Type = ESentenceType.Loop_WhileDo; }
        }
        public class Sentence_Loop_DoWhile : Sentence
        {
            public Sentence_List Body;
            public Exp ConditionObject;
            public Sentence_Loop_DoWhile() { Type = ESentenceType.Loop_DoWhile; }
        }
        public class Sentence_If : Sentence
        {
            public Exp Condition;
            public Sentence_List TrueBody;
            public Sentence_List FalseBody;

            public Sentence_If() { Type = ESentenceType.If; }
        }

        public class Sentence_Switch : Sentence
        {
            public Exp Condition;
            public class Case
            {
                public Variable CaseValue;
                public Sentence_List Body;
            }
            public Case[] Cases;
            public Sentence_List Default;

            public Sentence_Switch() { Type = ESentenceType.Switch; }
        }

        public class FunctionBody
        {
            public Sentence_List Body;
        }
    }
    
}
