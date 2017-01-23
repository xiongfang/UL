using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL
{
    namespace Runtime
    {
        public enum ECode
        {
            Invoke, //调用一个注册函数
            Call,   //调用一个函数
            Ret, //返回函数
            Push,       //将一个值压栈
            Pop,        //将一个值Pop出栈
            PushRet,    //将返回值压栈
            //Move,       //将源数据传送到目的地址
            Jump,  //无条件跳转指令
            JZ,    //标记为真则跳转
            Move    //栈上的值拷贝
        }

        public class Command
        {
            public ECode Code;
        }
        public class Command_Invoke : Command
        {
            //注册函数地址
            public int FuncID;
            public Command_Invoke() { Code = ECode.Invoke; }
        }
        public class Command_Call : Command
        {
            //代码地址
            public int CodeID;   
            public Command_Call() { Code = ECode.Call; }
        }
        public class Command_Ret : Command
        {
            public Command_Ret() { Code = ECode.Ret; }
        }
        public class Command_Push : Command
        {
            //public int MemoryID; //内存地址(为0则push一个空对象到栈顶)
            public Metadata.Const Value;
            public Command_Push() { Code = ECode.Push; }
        }
        public class Command_Pop : Command
        {
            //public int MemoryID; //内存地址(为0则pop一个对象，不存放到内存)
            public int Count;   //Pop数量
            public Command_Pop() { Code = ECode.Pop; }
        }

        public class Command_Move : Command
        {
            public enum AddressType
            {
                EBP,
                ESP
            }
            public AddressType SourceType;
            public int SourceIndex;  //相对于EBP
            public AddressType DestType;
            public int DestIndex; //相对于EBP
            public Command_Move() { Code = ECode.Move; }
        }

        public class Command_Jump : Command
        {
            //代码地址
            public int CodeID; 
            public Command_Jump() { Code = ECode.Jump; }
        }
        public class Command_JZ : Command
        {
            //代码地址
            public int CodeID;
            public Command_JZ() { Code = ECode.JZ; }
        }

        public class VisualMachine
        {
            //内存
            List<object> Memory = new List<object>();

            //指令
            List<Command> Commands = new List<Command>();

            VM_State _State;

            public class VM_State
            {
                VisualMachine This;
                public VM_State(VisualMachine vm) { This = vm; }

                //public void Push(object o)
                //{
                //    This.RuntimeStack.Push(o);
                //}

                //public object Pop()
                //{
                //    return This.RuntimeStack.Pop();
                //}

                public object Get(int Index)
                {
                    return This.RuntimeStack.ElementAt((This.RuntimeStack.Count - 1) - ((This.EBP - 2) - Index));
                }

                public void Ret(object o)
                {
                    This.PopFrame();
                    This.RetValue = o;
                }

                public void Ret()
                {
                    This.PopFrame();
                    This.RetValue = null;
                }
            }
            public delegate void RefistFunc(VM_State vm);

            List<RefistFunc> RegistFunctions = new List<RefistFunc>();

            //指令指针
            int EIP;
            ////栈顶指针
            //int ESP;
            //栈基指针
            int EBP;
            //跳转标记 寄存器
            bool Z;

            object RetValue;

            //运行时栈
            Stack<object> RuntimeStack = new Stack<object>();


            void PushFrame()
            {
                //保存上一个位置
                RuntimeStack.Push(EBP);
                RuntimeStack.Push(EIP);
                EBP = RuntimeStack.Count;
            }

            void PopFrame()
            {
                while (RuntimeStack.Count>EBP)
                {
                    RuntimeStack.Pop();
                }
                EIP = (int)RuntimeStack.Pop();
                EBP = (int)RuntimeStack.Pop();
            }

            

            void Do(Command cmd)
            {
                switch (cmd.Code)
                {
                    case ECode.Call:
                        Do_Call(cmd as Command_Call);
                        break;
                    case ECode.Invoke:
                        Do_Invoke(cmd as Command_Invoke);
                        break;
                    case ECode.Push:
                        Do_Push(cmd as Command_Push);
                        break;
                    case ECode.Ret:
                        Do_Return(cmd);
                        break;
                    case ECode.PushRet:
                        Do_PushRet(cmd);
                        break;
                    case ECode.Pop:
                        Do_Pop(cmd as Command_Pop);
                        break;
                    default:
                        break;
                }
            }


            void Do_Pop(Command_Pop cmd)
            {
                for (int i = 0; i < cmd.Count;i++ )
                {
                    RuntimeStack.Pop();
                }
                EIP++;
            }

            void Do_PushRet(Command cmd)
            {
                RuntimeStack.Push(RetValue);
                EIP++;
            }

            void Do_Invoke(Command_Invoke cmd)
            {
                PushFrame();
                RegistFunctions[cmd.FuncID - 1](_State);
                EIP++;
            }

            void Do_Push(Command_Push cmd)
            {
                RuntimeStack.Push(cmd.Value.Value);
                EIP++;
            }

            void Do_Call(Command_Call cmd)
            {
                PushFrame();
                EIP = cmd.CodeID;
            }

            //void Do_Move(Command cmd)
            //{

            //}

            void Do_Return(Command cmd)
            {
                PopFrame();
                EIP++;
            }

            /*
            int AddTest(int a, int b)
            {
                return a + 5 + b;
            }
            */

            static void Add(VM_State state)
            {
                int a= (int)state.Get(1);
                int b = (int)state.Get(2);
                state.Ret(a + b);
            }
            static void Printf(VM_State state)
            {
                Console.Out.WriteLine("打印 " + state.Get(1));
                state.Ret();
            }

            public void RunTest()
            {
                RegistFunctions.Add(Add);
                RegistFunctions.Add(Printf);

                Commands = new List<Command>();

                //起始函数，调用main函数
                Commands.Add(new Command_Call() { CodeID = 3 });
                Commands.Add(new Command_Ret());

                //main函数体
                Commands.Add(new Command_Push() { Value = 5 });
                Commands.Add(new Command_Push() { Value = 6 });
                Commands.Add(new Command_Push() { Value = 7 });

                Commands.Add(new Command_Invoke() { FuncID = 1 });
                Commands.Add(new Command_Pop() { Count = 2 });
                Commands.Add(new Command() { Code = ECode.PushRet  });
                Commands.Add(new Command_Invoke() { FuncID = 1 });
                Commands.Add(new Command_Pop() { Count = 2 });
                

                //打印返回值
                Commands.Add(new Command() { Code = ECode.PushRet });
                Commands.Add(new Command_Invoke() { FuncID = 2 });

                //主函数返回
                Commands.Add(new Command_Ret());
                Run(1);
            }

            public void RunTestCompiler(string json)
            {
                UL.Metadata.UL_Domain domain = Newtonsoft.Json.JsonConvert.DeserializeObject<Metadata.UL_Domain>(json);
                Compiler.Compiler cp = new Compiler.Compiler();
                cp.Compile(domain);

                //起始函数，调用main函数
                Commands.Add(new Command_Call() { CodeID = 3 });
                Commands.Add(new Command_Ret());
                Commands.AddRange(cp.Results);


                RegistFunctions.Add(Add);
                RegistFunctions.Add(Printf);


                for (int i = 0; i < RegistFunctions.Count;i++ )
                {
                    RefistFunc temp = RegistFunctions[i];
                    int index = cp.InvokeList.IndexOf(temp.Method.Name);

                    if (index != -1)
                    {
                        if(index < RegistFunctions.Count)
                        {
                            RegistFunctions[i] = RegistFunctions[index];
                            RegistFunctions[index] = temp;
                        }
                        else
                        {
                            throw new Exception("无效的调用");
                        }
                    }
                    else
                    {
                        throw new Exception("无效的调用");
                    }
                }
            }

            void Run(int EIP_POS)
            {
                EIP = EIP_POS;
                EBP = 0;
                _State = new VM_State(this);

                if(!(Commands[EIP-1] is Command_Call))
                {
                    throw new Exception("起始方法必须是函数");
                }


                do
                {
                    Do(Commands[EIP - 1]);
                } while (EIP <= Commands.Count && EBP != 0);
            }
        }
    }
}
