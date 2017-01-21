using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //List<Command> Results = new List<Command>();


            ////编译一个方法，返回方法的起始命令索引
            //List<int> Compile(List<UL.Metadata.FunctionBody> Body)
            //{
            //    List<int> Ret = new List<int>();
            //    for (int i = 0; i < Body.Count; i++)
            //    {
            //        Ret.Add(Results.Count);
            //        Compile(Body[i].Body);
            //    }
            //    return Ret;
            //}

            //void Compile(UL.Metadata.Sentence sentence)
            //{
            //    switch (sentence.Type)
            //    {
            //        case UL.Metadata.Sentence.ESentenceType.List:
            //            Compile_List(sentence as UL.Metadata.Sentence_List);
            //            break;
            //        case UL.Metadata.Sentence.ESentenceType.Move:
            //            Compile_Move(sentence as UL.Metadata.Sentence_Assign);
            //            break;
            //        case UL.Metadata.Sentence.ESentenceType.Return:
            //            Compile_Return(sentence as UL.Metadata.Sentence_Return);
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //void Compile_List(UL.Metadata.Sentence_List sentence)
            //{
            //    {
            //        Command cmd = new Command();
            //        cmd.Code = ECode.BeginScope;
            //        Results.Add(cmd);
            //    }
            //    for (int i = 0; i < sentence.Body.Length; i++)
            //    {
            //        Compile(sentence.Body[i]);
            //    }
            //    {
            //        Command cmd = new Command();
            //        cmd.Code = ECode.EndScope;
            //        Results.Add(cmd);
            //    }
            //}

            //void Compile_Move(UL.Metadata.Sentence_Assign sentence)
            //{
            //    {
            //        Command_Local cmd = new Command_Local();
            //        cmd.Code = ECode.Local;
            //        cmd.vb = sentence.A;
            //        Results.Add(cmd);
            //    }

            //    if (sentence.B != null)
            //    {
            //        switch (sentence.B.Type)
            //        {
            //            case Exp.EType.Variable:
            //                {

            //                }
            //                break;
            //            case Exp.EType.Call:
            //                {

            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    {
            //        Command_Move cmd = new Command_Move();
            //        cmd.Code = ECode.Local;
            //        cmd.B = sentence.A;
            //        Results.Add(cmd);
            //    }
            //}

            //void Compile_Exp(UL.Metadata.Sentence_Return sentence)
            //{

            //}


            //void Compile_Return(UL.Metadata.Sentence_Return sentence)
            //{
            //    {
            //        Command cmd = new Command();
            //        cmd.Code = ECode.Return;
            //        Results.Add(cmd);
            //    }
            //}
        }
    }
}
