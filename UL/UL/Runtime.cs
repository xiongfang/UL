using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
    public class Command
    {
        public enum ECode
        {
            NewObject,
            Call,
            List,
            Loop_For,
            Loop_WhileDo,
            Loop_DoWhile,
            If,
            Switch,
            Return
        }

        public ECode Code;
    }

    public class Command_NewObject : Command
    {
        //New的对象类型
        public string Type;
        //新变量名称
        public string Name;

        public Command_NewObject() { Code = ECode.NewObject; }
    }

    public class Command_Call : Command
    {
        //调用函数的对象，或者类
        public string Object;
        //调用的函数名
        public string Function;
        //调用的参数
        public string[] Args;

        public Command_Call() { Code = ECode.Call; }
    }

    public class Command_Return:Command
    {
        public string Object;

        public Command_Return() { Code = ECode.Return; }
    }

    public class Command_List : Command
    {
        public Command[] Body;

        public Command_List() { Code = ECode.List; }
    }

    public class Command_Loop_For:Command
    {
        public string StartObject;
        public string ConditionObject;
        public Command End;
        public Command[] LoopBody;

        public Command_Loop_For() { Code = ECode.Loop_For; }
    }

    public class Command_Loop_WhileDo:Command
    {
        public string ConditionObject;
        public Command[] Body;

        public Command_Loop_WhileDo() { Code = ECode.Loop_WhileDo; }
    }
    public class Command_Loop_DoWhile : Command
    {
        public Command[] Body;
        public string ConditionObject;
        public Command_Loop_DoWhile() { Code = ECode.Loop_DoWhile; }
    }
    public class Command_If : Command
    {
        public string ConditionObject;
        public Command[] TrueBody;
        public Command[] FalseBody;

        public Command_If() { Code = ECode.If; }
    }

    public class Command_Switch : Command
    {
        public string ConditionObject;
        public class Case
        {
            public string CaseValue;
            public Command[] Body;
        }
        public Case[] CaseList;
        public Command[] DefaultCase;

        public Command_Switch() { Code = ECode.Switch; }
    }

    public class VisualMachine
    {
        public List<Command> CommandList;
    }
}
