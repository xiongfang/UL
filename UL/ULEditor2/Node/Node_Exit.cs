using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    class Node_Exit : NodeBase
    {
        public Node_Exit(ULNode node) :base(node)
        {
            int control_input_count = 0;

            {
                GetPinInputPos(control_input_count, out int x, out int y);
                PinIns.Add(new ControlPinIn(this, control_input_count, "in", x, y));
                control_input_count++;
            }

        }

        public override int Width => 100;
    }
}
