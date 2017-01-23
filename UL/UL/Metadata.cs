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
            public List<string> MetaStrings = new List<string>();

            public string Find(string Key)
            {
                for(int i=0;i<MetaStrings.Count;i++)
                {
                    if (MetaStrings[i].StartsWith(Key))
                        return MetaStrings[i];
                }
                return null;
            }
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
            public class Parameter
            {
                public EParamModifier Modifier;
                public string TypeFulleName;
                public string Name;
                [NonSerialized]
                public UL_Type Type;
            }
            public List<Parameter> Params = new List<Parameter>();

            public bool IsVirsual;

            public Sentence_List Body;

            public string UnicodeFullName
            {
                get
                {
                    string data = Owner.FullName + "."+Name+"(";
                    for (int i = 0; i < Params.Count;i++ )
                    {
                        data += Params[i].Type.FullName;

                        if(i!=Params.Count-1)
                        {
                            data += ",";
                        }
                    }

                    data += ")";

                    return data;
                }
            }

            public void PostInit()
            {
                foreach(var p in Params)
                {
                    Owner.Domain.TypeDic.TryGetValue(p.TypeFulleName, out p.Type);
                }
            }
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
            public string ParentFullName;
            public List<UL_Type> Inners = new List<UL_Type>();
            public EAccessModifier Modifier;

            [NonSerialized]
            public UL_Type Outer;
            [NonSerialized]
            public UL_Type Parent;
            [NonSerialized]
            public UL_Domain Domain;

            public void Init()
            {
                foreach (var func in Functions)
                {
                    func.Owner = this;
                }
                foreach (var var in Variables)
                {
                    var.Owner = this;
                }
                foreach (var inner in Inners)
                {
                    inner.Outer = this;
                }
                
            }

            public string FullName
            {
                get
                {
                    string ResultName = Name;
                    UL_Type TempOut = Outer;
                    while(TempOut!=null)
                    {
                        ResultName = TempOut.Name + ResultName;
                        TempOut = TempOut.Outer;
                    }

                    return ResultName;
                }
            }

            public void PostInit()
            {
                Domain.TypeDic.TryGetValue(ParentFullName, out Parent);
                foreach (var func in Functions)
                {
                    func.PostInit();
                }
            }
        }

        public class UL_Domain
        {
            [NonSerialized]
            public Dictionary<string, UL_Type> TypeDic = new Dictionary<string, UL_Type>();

            public List<UL_Type> Types = new List<UL_Type>();

            public void Init()
            {
                for(int i=0;i<Types.Count;i++)
                {
                    Types[i].Domain = this;
                    Types[i].Init();
                }
                //注册所有类型
                for (int i = 0; i < Types.Count; i++)
                {
                    RegistType(Types[i]);
                }
                //二次初始化
                for (int i = 0; i < Types.Count; i++)
                {
                    Types[i].PostInit();
                }
            }

            void RegistType(UL_Type type)
            {
                TypeDic[type.FullName] = type;
                for(int i=0;i<type.Inners.Count;i++)
                {
                    RegistType(type.Inners[i]);
                }
            }
        }


        

        public class Const
        {
            public enum ConstType
            {
                NoInit,
                Int,
                Float,
                Double,
                Char,
                String,
                Bool,
            }
            public ConstType Type;
            public string strValue;
            public int intValue;
            public float floatValue;
            public double doubleValue;
            public bool boolValue;
            public char charValue;

            public static implicit operator Const(int v){
                return new Const() { intValue = v, Type = ConstType.Int };
            }
            public static implicit operator Const(string v)
            {
                return new Const() { strValue = v, Type = ConstType.String };
            }
            public static implicit operator Const(float v)
            {
                return new Const() { floatValue = v, Type = ConstType.Float };
            }
            public static implicit operator Const(double v)
            {
                return new Const() { doubleValue = v, Type = ConstType.Double };
            }
            public static implicit operator Const(bool v)
            {
                return new Const() { boolValue = v, Type = ConstType.Bool };
            }
            public static implicit operator Const(char v)
            {
                return new Const() { charValue = v, Type = ConstType.Char };
            }

            public object Value
            {
                get
                {
                    switch(Type)
                    {
                        case ConstType.Char:
                            return charValue;
                        case ConstType.Bool:
                            return boolValue;
                        case ConstType.Double:
                            return doubleValue;
                        case ConstType.Float:
                            return floatValue;
                        case ConstType.Int:
                            return intValue;
                        case ConstType.String:
                            return strValue;
                        default:
                            return null;
                    }
                }
            }
        }
        public class Variable
        {
            public string TypeName;
            public string ObjectName;
        }

        public class Call
        {
            //调用函数的对象，或者类
            public string Object;
            //调用的函数名
            public string Function;

            //调用的参数
            public Exp[] Args;

            //返回值
            public string Ret;
        }

        public class Exp
        {
            public enum EType
            {
                Variable,
                Call,
                Const
            }

            public EType Type;
            public Const ArgConst;
            public Variable ArgVar;
            public Call ArgCall;
        }


        public class Sentence_Assign : Sentence
        {
            //调用函数的对象，或者类
            public string A;
            //调用函数的对象，或者类
            public Exp B;
            public Sentence_Assign() { Type = ESentenceType.Assign; }
        }

        public class Sentence_Return : Sentence
        {
            public Exp Object;
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
                public Const CaseValue;
                public Sentence_List Body;
            }
            public Case[] Cases;
            public Sentence_List Default;
            public Sentence_Switch() { Type = ESentenceType.Switch; }
        }

        public class Sentence_Local : Sentence
        {
            //调用函数的对象，或者类
            public Variable A;
            public Sentence_Local() { Type = ESentenceType.Local; }
        }

        public class Sentence
        {
            public enum ESentenceType
            {
                Assign,
                List,
                Loop_For,
                Loop_WhileDo,
                Loop_DoWhile,
                If,
                Switch,
                Return,
                Local
            }

            public ESentenceType Type;
        }

        public class Sentence_Args
        {
            public Sentence_Assign Assign;
            public Sentence_Return Return;
            public Sentence_List List;
            public Sentence_List_Loop_For Loop_For;
            public Sentence_Loop_WhileDo Loop_WhileDo;
            public Sentence_Loop_DoWhile Loop_DoWhile;
            public Sentence_If If;
            public Sentence_Switch Switch;
            public Sentence_Local Local;
        }
    }
    
}
