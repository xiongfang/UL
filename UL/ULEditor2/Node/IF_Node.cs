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
            if (node.ControlInputs.Length != 1)
            {
                node.ControlInputs = new string[1];
            }
            if (node.ControlOutputs.Length!=2)
            {
                node.ControlOutputs = new string[2];
            }
            if (node.Inputs.Length != 1)
                node.Inputs = new string[1];

            int input_count = 0;
            int output_count = 0;

            foreach (var ci in node.ControlInputs)
            {
                GetPinInputPos(input_count, out int x, out int y);
                PinIns.Add(new ControlPinIn(this, input_count,"in",x, y));
                input_count++;
            }
            foreach (var co in node.ControlOutputs)
            {
                GetPinOutputPos(output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, output_count, output_count==0?"true":"false",x, y));
                output_count++;
            }
            {
                GetPinInputPos(input_count++, out int x, out int y);
                PinIns.Add(new DataPinIn(this, new ULArg() { Name = "condition", TypeID = "00000009" },0,x,y));
            }

        }

        public override int Width => 150;
    }
}
