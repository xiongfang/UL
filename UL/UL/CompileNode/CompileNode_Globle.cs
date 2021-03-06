using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace UL.CompileNode
{
    public class CompileNode_Globle : CompileNode
    {
        public List<string> usingList = new List<string>();

        public List<ULTypeInfo> GetChildrenTypes(CompileNode baseNode)
        {
            List<ULTypeInfo> result = new List<ULTypeInfo>();

            foreach (var c in baseNode.Children)
            {
                if (c is CompileNode_Class)
                {
                    result.Add((c as CompileNode_Class).type);
                    GetChildrenTypes(c);
                }
            }
            return result;
        }

        //public List<ULMemberInfo> GetChildrenMembers(CompileNode baseNode)
        //{
        //    List<ULMemberInfo> result = new List<ULMemberInfo>();

        //    foreach (var c in baseNode.Children)
        //    {
        //        if (c is CompileNode_Class)
        //        {
        //            result.AddRange((c as CompileNode_Class).memberInfos);
        //            GetChildrenTypes(c);
        //        }
        //    }
        //    return result;
        //}
    }

}
