using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ULEditor2
{
    class NodeBase : INode
    {
        ULNode node;
        public NodeBase(ULNode n) { 
            node = n;
            if (node.ControlInputs == null)
                node.ControlInputs = new string[0];
            if (node.ControlOutputs == null)
                node.ControlOutputs = new string[0];
            if (node.Inputs == null)
                node.Inputs = new string[0];
        }

        public ULNode Node { get { return node; } }

        public int X { get => node.X; set => node.X = value; }
        public int Y { get => node.Y; set => node.Y = value; }


        public virtual int Width => 100;

        public virtual int Height => INode.TitleHeight + (int)(MathF.Max(ControlOutputs.Length+ OutputNames.Length,InputNames.Length+ControlInputs.Length) * INode.LineHeight);

        public virtual string Title
        {
            get
            {
                if(Data.members.TryGetValue(node.Name, out var m))
                {
                    return m.Name;
                }
                return node.Name;
            }
        }

        public string[] Inputs => node.Inputs;

        public string[] ControlInputs => node.ControlInputs;

        public string[] ControlOutputs => node.ControlOutputs;

        public virtual string[] InputNames => new string[0];

        public virtual string[] OutputNames => new string[0];
    }
}
