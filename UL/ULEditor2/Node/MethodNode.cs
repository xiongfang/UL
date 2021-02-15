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
            int input_count = 0;
            int output_count = 0;

            foreach(var ci in node.ControlInputs)
            {
                GetPinInputPos(input_count, out int x, out int y);
                PinIns.Add(new ControlPinIn(this, input_count,x, y));
                input_count++;
            }
            foreach(var co in node.ControlOutputs)
            {
                GetPinOutputPos(output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, output_count, x,y));
                output_count++;
            }

            if(Model.Data.members.TryGetValue(node.Name,out var member))
            {
                foreach(var a in member.Graph.Args)
                {
                    GetPinInputPos(input_count++, out int x, out int y);
                    PinIns.Add(new DataPinIn(this, a[1], x, y));
                }

                foreach (var a in member.Graph.Outputs)
                {
                    GetPinOutputPos(output_count++, out int x, out int y);
                    PinOuts.Add(new DataPinOut(this, a[1], x, y));
                }
            }

        }

        //public override int Height => INode.TitleHeight + INode.LineHeight;
    }
}
