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
            if (node.ControlInputs.Count != 1)
            {
                node.ControlInputs.Clear();
                node.ControlInputs.Add("");
            }
            if (node.ControlOutputs.Count!=2)
            {
                node.ControlOutputs.Clear();
                node.ControlOutputs.AddRange(new string[2]);
            }
            if (node.Inputs.Count != 1)
            {
                node.Inputs.Clear();
                node.Inputs.AddRange(new string[1]);
            }
                

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
                PinIns.Add(new DataPinIn(this, new ULArg() { Name = "condition", TypeID = "System.Boolean" },0,x,y));
            }

        }

        public override int Width => 150;
    }
}
