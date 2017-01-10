using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
    public enum ECode
    {
        Call,   //调用一个函数
        BeginScope, //开始子域
        EndScope,   //结束子域
        Move,       //赋值
        Local,      //新建本地变量
        Return, //返回函数
        Jump,  //无条件跳转指令
        JZ,    //标记为真则跳转
    }

    public class Command
    {
        public ECode Code;
    }

    public class Command_Call:Command
    {
        public Call args;
        public Command_Call() { Code = ECode.Call; }
    }
    public class Command_Jump : Command
    {
        public int Index;
        public Command_Jump() { Code = ECode.Jump; }
    }

    public class Command_Local : Command
    {
        public UL_Variable vb;
    }

    public class Compiler
    {
        List<Command> Results = new List<Command>();

        void Compile(UL_Type type)
        {

        }

        //编译一个方法，返回方法的起始命令索引
        List<int> Compile(List<FunctionBody> Body)
        {
            List<int> Ret = new List<int>();
            for (int i = 0; i < Body.Count;i++ )
            {
                Ret.Add(Results.Count);
                Compile(Body[i].Body);
            }
            return Ret;
        }

        void Compile(Sentence sentence)
        {
            switch(sentence.Type)
            {
                case Sentence.ESentenceType.List:
                    Compile_List(sentence as Sentence_List);
                    break;
                case Sentence.ESentenceType.Move:
                    Compile_Move(sentence as Sentence_Assign);
                    break;
                case Sentence.ESentenceType.Return:
                    Compile_Return(sentence as Sentence_Return);
                    break;
                default:
                    break;
            }
        }

        void Compile_List(Sentence_List sentence)
        {
            {
                Command cmd = new Command();
                cmd.Code = ECode.BeginScope;
                Results.Add(cmd);
            }
            for(int i=0;i<sentence.Body.Length;i++)
            {
                Compile(sentence.Body[i]);
            }
            {
                Command cmd = new Command();
                cmd.Code = ECode.EndScope;
                Results.Add(cmd);
            }
        }

        void Compile_Move(Sentence_Assign sentence)
        {
            
        }

        void Compile_Return(Sentence_Return sentence)
        {
            {
                Command cmd = new Command();
                cmd.Code = ECode.Return;
                Results.Add(cmd);
            }
        }
    }
}
