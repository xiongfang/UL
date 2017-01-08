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
            Statement,
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


    public abstract class Exp
    {
        public abstract string Type { get; }
        public abstract string Value { get; }
    }

    public class Variable : Exp
    {
        public string TypeName;
        public string ObjectName;

        public override string Type { get { return TypeName; } }
        public override string Value { get { return ObjectName; } }

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

        public override string Type { get { return Ret.Type; } }
        public override string Value { get { return Ret.Value; } }
    }

    public class Command_Statement : Command
    {
        //调用函数的对象，或者类
        public Variable A;
        //调用函数的对象，或者类
        public Exp B;
    }

    public class Command_Return:Command
    {
        public Variable Object;

        public Command_Return() { Code = ECode.Return; }
    }

    public class Command_List : Command
    {
        public Command[] Body;

        public Command_List() { Code = ECode.List; }
    }

    public class Command_Loop_For:Command
    {
        public Command_Statement Start;
        public Exp Condition;
        public Command_Statement End;
        public Command_List Body;

        public Command_Loop_For() { Code = ECode.Loop_For; }
    }

    public class Command_Loop_WhileDo:Command
    {
        public Exp ConditionObject;
        public Command_List Body;

        public Command_Loop_WhileDo() { Code = ECode.Loop_WhileDo; }
    }
    public class Command_Loop_DoWhile : Command
    {
        public Command_List Body;
        public Exp ConditionObject;
        public Command_Loop_DoWhile() { Code = ECode.Loop_DoWhile; }
    }
    public class Command_If : Command
    {
        public Exp Condition;
        public Command_List TrueBody;
        public Command_List FalseBody;

        public Command_If() { Code = ECode.If; }
    }

    public class Command_Switch : Command
    {
        public Exp Condition;
        public class Case
        {
            public Variable CaseValue;
            public Command_List Body;
        }
        public Case[] Cases;
        public Command_List Default;

        public Command_Switch() { Code = ECode.Switch; }
    }

    public class FunctionBody
    {
        public Command_List Body;
    }

    public class VisualMachine
    {

    }
}
