using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    class MethodNode : NodeBase
    {
        public MethodNode(ULNode node) :base(node)
        {
            if (node.ControlInputs.Length != 1)
            {
                node.ControlInputs = new string[1];
            }
            if (node.ControlOutputs.Length!=1)
            {
                node.ControlOutputs = new string[1];
            }
        }

        //public override int Height => INode.TitleHeight + INode.LineHeight;
    }
}
