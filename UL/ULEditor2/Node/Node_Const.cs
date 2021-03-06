using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    class Node_Const : NodeBase
    {
        public Node_Const(ULNode node) :base(node)
        {

            if (node.ControlOutputs.Count!=1)
            {
                node.ControlOutputs.Clear();
                node.ControlOutputs.AddRange(new string[1]);
            }

            int output_count = 0;

            foreach (var co in node.ControlOutputs)
            {
                GetPinOutputPos(output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, output_count,"out",x, y));
                output_count++;
            }

            {
                GetPinOutputPos(output_count++, out int x, out int y);
                PinOuts.Add(new DataPinOut(this, new ULArg() { Name = "condition", TypeID = "System.Boolean" }, 0, x, y));
            }
        }

        public override int Width => 100;
    }
}
