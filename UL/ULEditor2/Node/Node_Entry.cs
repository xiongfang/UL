using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    class Node_Entry : NodeBase
    {
        public Node_Entry(ULNode node) :base(node)
        {

            if (node.ControlOutputs.Length!=1)
            {
                node.ControlOutputs = new string[1];
            }

            int output_count = 0;

            foreach (var co in node.ControlOutputs)
            {
                GetPinOutputPos(output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, output_count,x, y));
                output_count++;
            }

        }

        public override int Width => 100;
    }
}
