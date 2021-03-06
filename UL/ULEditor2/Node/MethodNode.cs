using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ULEditor2
{
    class MethodNode : NodeBase
    {
        int _width;

        const int font_size = 7;

        public MethodNode(ULNode node) :base(node)
        {
            //if (node.ControlInputs.Count == 0)
            //{
            //    node.ControlInputs.Add("");
            //}
            if (node.ControlOutputs.Count==0)
            {
                node.ControlOutputs.Add("");
            }
            int control_input_count = 0;
            int control_output_count = 0;

            

            //foreach (var ci in node.ControlInputs)
            {
                GetPinInputPos(control_input_count, out int x, out int y);
                PinIns.Add(new ControlPinIn(this, control_input_count,"in", x, y));
                control_input_count++;
            }
            foreach(var co in node.ControlOutputs)
            {
                GetPinOutputPos(control_output_count, out int x, out int y);
                PinOuts.Add(new ControlPinOut(this, control_output_count,"out", x,y));
                control_output_count++;
            }
            var member = Model.Data.GetMember(node.Name);
            if (member!=null)
            {
                int data_input_count = 0;
                int data_output_count = 0;
                if(node.Inputs.Count==0)
                {
                    for (int i = 0; i < member.Graph.Args.Count; i++)
                    {
                        node.Inputs.Add("");
                    }
                }
                
                int left_width = 0;
                int right_width = 0;



                foreach (var a in member.Graph.Args)
                {
                    GetPinInputPos(data_input_count+control_input_count, out int x, out int y);
                    PinIns.Add(new DataPinIn(this, a, data_input_count, x, y));
                    left_width = Math.Max(left_width, PinIns[PinIns.Count - 1].Name.Length * font_size);
                    data_input_count++;
                }

                foreach (var a in member.Graph.Outputs)
                {
                    GetPinOutputPos(data_output_count+control_output_count, out int x, out int y);
                    PinOuts.Add(new DataPinOut(this, a, data_output_count, x, y));
                    right_width = Math.Max(right_width, PinOuts[PinOuts.Count - 1].Name.Length * font_size);
                    data_output_count++;
                }

                _width = left_width + right_width + 5;
            }

            _width = Math.Max(100, _width);
        }

        public override int Width => _width;

        public override void PostInit(Func<string, INode> find)
        {
            base.PostInit(find);
            for(int i=0;i<PinOuts.Count;i++)
            {
                GetPinOutputPos(i, out int x, out int y);
                PinOuts[i].LocalX = x;
                PinOuts[i].LocalY = y;
            }
        }
    }
}
