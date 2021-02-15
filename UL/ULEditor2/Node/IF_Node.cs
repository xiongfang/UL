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
                GetPinInputPos(input_count++, out int x, out int y);
                PinIns.Add(new ControlPinIn(this,x,y));
            }
            foreach (var co in node.ControlOutputs)
            {
                GetPinOutputPos(output_count++, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this,x,y));
            }
            {
                GetPinInputPos(input_count++, out int x, out int y);
                PinIns.Add(new DataPinIn(this, node.Inputs[0],x,y));
            }

        }

        
    }
}
