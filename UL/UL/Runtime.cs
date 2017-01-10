using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
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

    public class Sentence_Return:Sentence
    {
        public Variable Object;

        public Sentence_Return() { Type = ESentenceType.Return; }
    }

    public class Sentence_List : Sentence
    {
        public Sentence[] Body;

        public Sentence_List() { Type = ESentenceType.List; }
    }

    public class Sentence_List_Loop_For:Sentence
    {
        public Sentence_Assign Start;
        public Exp Condition;
        public Sentence_Assign End;
        public Sentence_List Body;

        public Sentence_List_Loop_For() { Type = ESentenceType.Loop_For; }
    }

    public class Sentence_Loop_WhileDo:Sentence
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

    public class VisualMachine
    {
        public Dictionary<string, UL_Type> NameTypeMap = new Dictionary<string, UL_Type>();

        //指令
        List<Command> Commands = new List<Command>();
        int EIP;    //指令指针
        bool Z;     //跳转标记 寄存器

        Scope GlobalScope;  //全局域

        //运行时栈
        Stack<Frame> RuntimeStack = new Stack<Frame>();

        public class Scope
        {
            public Scope Outer;
            public Dictionary<string, object> Variables = new Dictionary<string, object>();

            public int Index;
        }

        public class Frame
        {
            public VisualMachine Machine;
            public int EIP;
            
            public Stack<Scope> ScopeStack = new Stack<Scope>();

            public void Push()
            {
                Scope sp = new Scope();

                if (ScopeStack.Count > 0)
                    sp.Outer = Top;
                else
                    sp.Outer = Machine.GlobalScope;
                ScopeStack.Push(sp);

            }

            public void Pop()
            {
                ScopeStack.Pop();
            }

            public Scope Top
            {
                get
                {
                    return ScopeStack.Last();
                }
            }
        }

        


        void PushFrame()
        {
            Frame f = new Frame();
            f.Push();
            RuntimeStack.Push(f);
        }

        Frame TopFrame
        {
            get { return RuntimeStack.Last(); }
        }

        void PopFrame()
        {
            RuntimeStack.Pop();
        }

        public void DoString(string code)
        {

        }

        void Do(Command cmd)
        {
            switch (cmd.Code)
            {
                case ECode.BeginScope:
                    {
                        TopFrame.Push();
                    }
                    break;
                case ECode.EndScope:
                    {
                        TopFrame.Pop();
                    }
                    break;
                case ECode.Call:
                    Do_Call(cmd as Command_Call);
                    break;
                case ECode.Move:
                    Do_Move(cmd);
                    break;
                case ECode.Return:
                    Do_Return(cmd);
                    break;
                default:
                    break;
            }
        }


        void Do_Call(Command_Call cmd)
        {
            PushFrame();

        }

        void Do_Move(Command cmd)
        {

        }

        void Do_Return(Command cmd)
        {
            EIP = TopFrame.EIP + 1;
            PopFrame();
        }
    }
}
