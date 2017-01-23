using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UL.Runtime;

namespace UL
{
    //可选的对象
    //Stack.1,2,3...，从栈顶开始
    //Local.1,2,3...，从栈底开始
    //heap.Name

    namespace Compiler
    {
        public class Compiler
        {
            public  List<Command> Results = new List<Command>();
            public List<string> InvokeList = new List<string>();

            //函数索引对应的代码地址表
            List<int> FuncCodeTable = new List<int>();

            //本地变量的栈地址，包括参数，相对于EBP
            Dictionary<string, int> LocalVariables = new Dictionary<string, int>();
            //所有方法的索引
            Dictionary<string, FuncInfo> FunctionMap = new Dictionary<string, FuncInfo>();

            public class FuncInfo
            {
                public Metadata.UL_Function Func;
                public int Index;
            }

            public void Compile(UL.Metadata.UL_Domain Domain)
            {
                //为所有函数编号
                List<UL.Metadata.UL_Function> funcs = new List<Metadata.UL_Function>();

                foreach(var t in Domain.Types)
                {
                    foreach(var m in t.Functions)
                    {
                        string Invoke = m.Metadata.Find("Invoke");
                        if(Invoke!=null)
                        {
                            string Value = Invoke.Split('=')[1];
                            InvokeList.Add(Value);
                            FunctionMap[m.UnicodeFullName] = new FuncInfo() { Func = m};
                        }
                        else
                        {
                            funcs.Add(m);
                            FunctionMap[m.UnicodeFullName] = new FuncInfo() { Func = m, Index = funcs.Count };
                        }
                    }
                }

                //编译所有函数
                foreach (var m in funcs)
                {
                    LocalVariables.Clear();
                    foreach(var p in m.Params)
                    {
                        LocalVariables[p.Name] = -4 - LocalVariables.Count+1;
                    }
                    FuncCodeTable.Add(Results.Count+1);
                    Compile(m.Body);
                }

                //将所有Call命令的地址修正
                foreach(var cmd in Results)
                {
                    Command_Call cmdCall = cmd as Command_Call;
                    if(cmdCall!=null)
                    {
                        cmdCall.CodeID = FuncCodeTable[cmdCall.CodeID - 1];
                    }
                }
            }

            //编译一个方法，返回方法的起始命令索引
            void Compile(UL.Metadata.Sentence[] Body)
            {
                for (int i = 0; i < Body.Length;i++ )
                {
                    Compile(Body[i]);
                }
            }

            void Compile(UL.Metadata.Sentence sentence)
            {
                switch (sentence.Type)
                {
                    case UL.Metadata.ESentenceType.List:
                        Compile_List(sentence.List);
                        break;
                    case UL.Metadata.ESentenceType.Assign:
                        Compile_Assign(sentence.Assign);
                        break;
                    case UL.Metadata.ESentenceType.Return:
                        Compile_Return(sentence.Return);
                        break;
                    case Metadata.ESentenceType.Local:
                        Compile_Local(sentence.Local);
                        break;
                    default:
                        break;
                }
            }


            void Compile_Local( UL.Metadata.Sentence_Local sentence )
            {
                Command_Push cmd = new Command_Push();
                if(LocalVariables.ContainsKey(sentence.Var.ObjectName))
                {
                    throw new Exception("本地变量名称重复");
                }
                LocalVariables[sentence.Var.ObjectName] = LocalVariables.Count;
                Results.Add(cmd);
            }

            void Compile_List(UL.Metadata.Sentence_List sentence)
            {
                for (int i = 0; i < sentence.Body.Length; i++)
                {
                    Compile(sentence.Body[i]);
                }
            }

            void Compile_Assign(UL.Metadata.Sentence_Assign sentence)
            {

                if (sentence.B != null)
                {
                    Compile_Exp(sentence.B);
                }

                //赋值，默认返回值放在下一个本地变量处
                if(sentence.A!=null)
                {
                    int StackIndex = LocalVariables[sentence.A];
                    Command_Move Mv = new Command_Move();
                    Mv.SourceIndex = -1;
                    Mv.SourceType = Command_Move.AddressType.ESP;

                    Mv.DestType = Command_Move.AddressType.EBP;
                    Mv.DestIndex = StackIndex;
                    Results.Add(Mv);
                }
            }

            void Compile_Exp(UL.Metadata.Exp exp)
            {
                switch (exp.Type)
                {
                    case UL.Metadata.Exp.EType.Variable:
                        {
                            Command_Push cmd = new Command_Push();
                            Results.Add(cmd);

                            //将本地变量赋值到栈顶
                            int StackIndex = LocalVariables[exp.ArgVar.ObjectName];
                            Command_Move Mv = new Command_Move();
                            Mv.DestIndex = -1;
                            Mv.DestType = Command_Move.AddressType.ESP;

                            Mv.SourceType = Command_Move.AddressType.EBP;
                            Mv.SourceIndex = StackIndex;
                            Results.Add(Mv);
                        }
                        break;
                    case UL.Metadata.Exp.EType.Call:
                        {
                            Compile_Call(exp.ArgCall);
                        }
                        break;
                    case Metadata.Exp.EType.Const:
                        {
                            Command_Push cmd = new Command_Push();
                            cmd.Value = exp.ArgConst;
                            Results.Add(cmd);
                        }
                        break;
                    default:
                        break;
                }
            }

            void Compile_Call(UL.Metadata.Call call)
            {
                for(int i=0;i<call.Args.Length;i++)
                {
                    Compile_Exp(call.Args[i]);
                }

                string CallFullName = call.Object+"."+call.Function;
                string Invoke = FunctionMap[CallFullName].Func.Metadata.Find("Invoke");
                if(Invoke == null)
                {
                    Command_Call cmd = new Command_Call();
                    cmd.CodeID = FunctionMap[CallFullName].Index;
                    Results.Add(cmd);
                }
                else
                {
                    string Value = Invoke.Split('=')[1];
                    Command_Invoke cmd = new Command_Invoke();
                    cmd.FuncID = InvokeList.FindIndex((a) => { return a == Value; }) +1;
                    Results.Add(cmd);
                }
                Command_Pop cmdPopParams = new Command_Pop();
                cmdPopParams.Count = call.Args.Length;
                Results.Add(cmdPopParams);

                Command cmdPushRet = new Command();
                cmdPushRet.Code = ECode.PushRet;
                Results.Add(cmdPushRet);
            }

            void Compile_Return(UL.Metadata.Sentence_Return sentence)
            {
                if(sentence.Value!=null)
                {
                    Compile_Exp(sentence.Value);
                }

                {
                    Command cmd = new Command();
                    cmd.Code = ECode.Ret;
                    Results.Add(cmd);
                }
            }
        }
    }
}
