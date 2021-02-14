using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    class IF_Node:NodeBase
    {
        public IF_Node(ULNode node) :base(node)
        {
            if(node.ControlOutputs==null || node.ControlOutputs.Length!=2)
            {
                node.ControlOutputs = new string[2];
            }
        }

        public override int Height => INode.TitleHeight + 2 * INode.LineHeight;
    }
}
