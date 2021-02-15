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
            int control_input_count = 0;
            int control_output_count = 0;

            foreach(var ci in node.ControlInputs)
            {
                GetPinInputPos(control_input_count, out int x, out int y);
                PinIns.Add(new ControlPinIn(this, control_input_count, x, y));
                control_input_count++;
            }
            foreach(var co in node.ControlOutputs)
            {
                GetPinOutputPos(control_output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, control_output_count, x,y));
                control_output_count++;
            }

            if(Model.Data.members.TryGetValue(node.Name,out var member))
            {
                int data_input_count = 0;
                int data_output_count = 0;

                foreach (var a in member.Graph.Args)
                {
                    GetPinInputPos(data_input_count, out int x, out int y);
                    PinIns.Add(new DataPinIn(this, a[1], data_input_count, x, y));
                    data_input_count++;
                }

                foreach (var a in member.Graph.Outputs)
                {
                    GetPinOutputPos(data_output_count, out int x, out int y);
                    PinOuts.Add(new DataPinOut(this, a[1], data_output_count, x, y));
                    data_output_count++;
                }
            }

        }

        //public override int Height => INode.TitleHeight + INode.LineHeight;
    }
}
