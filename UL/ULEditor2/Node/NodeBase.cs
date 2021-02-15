using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ULEditor2
{
    class NodeBase : INode
    {
        ULNode node;
        List<IPinIn> _PinIns;
        List<IPinOut> _PinOuts;

        public NodeBase(ULNode n) { 
            node = n;
            _PinIns = new List<IPinIn>();
            _PinOuts = new List<IPinOut>();
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

        public virtual int Height => INode.TitleHeight + (int)(MathF.Max(PinIns.Count, PinOuts.Count) * INode.LineHeight);

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

        public List<IPinIn> PinIns => _PinIns;

        public List<IPinOut> PinOuts => _PinOuts;

        protected void GetPinInputPos(int index, out int x, out int y)
        {
            x = -INode.PinSize - 5;
            y = INode.TitleHeight + INode.LineHeight * index + (INode.LineHeight - INode.PinSize) / 2;
        }

        protected void GetPinOutputPos(int index, out int x, out int y)
        {
            x = Width + 5;
            y = INode.TitleHeight + INode.LineHeight * index + (INode.LineHeight - INode.PinSize) / 2;
        }

    }



    class PinBase : IPin
    {
        protected INode _Node;
        protected int _LocalX;
        protected int _LocalY;
        protected int _Height;
        protected int _Width;
        protected string _Name;

        public PinBase(INode node) { 
            _Node = node;
            _Width = INode.PinSize;
            _Height = INode.PinSize;
        }

        public INode Node => _Node;

        public int X => _Node.X+ _LocalX;

        public int Y => _Node.Y+ _LocalY;

        public int Width => _Width;

        public int Height => _Height;

        public int LocalX { get => _LocalX; set => _LocalX=value; }
        public int LocalY { get => _LocalY; set => _LocalY=value; }
        public string Name { get => _Name; set => _Name = value; }
    }

    class PinIn : PinBase, IPinIn
    {
        List<IPinOut> _Outs;
        public PinIn(INode node) : base(node)
        {
            _Outs = new List<IPinOut>();
        }

        public List<IPinOut> Outs => _Outs;

        public virtual void AddLink(IPinOut po)
        {
            Outs.Add(po);
        }

        public virtual void RemoveLink(IPinOut po)
        {
            Outs.Remove(po);
        }
    }

    class PinOut : PinBase, IPinOut
    {
        List<IPinIn> _Ins;
        public PinOut(INode node) : base(node)
        {
            _Ins = new List<IPinIn>();
        }

        public List<IPinIn> Ins => _Ins;

        public virtual bool CanLink(IPinIn pinIn)
        {
            return true;
        }

        public virtual void Link(IPinIn pinIn)
        {
            if (_Ins.Contains(pinIn))
                return;

            _Ins.Add(pinIn);
            pinIn.AddLink(this);
        }

        public virtual void UnLink(IPinIn pinIn)
        {
            if(_Ins.Contains(pinIn))
            {
                _Ins.Remove(pinIn);
                pinIn.RemoveLink(this);
            }
        }
    }

    class ControlPinIn : PinIn
    {
        public ControlPinIn(INode node,int x,int y) : base(node)
        {
            _LocalX = x;
            _LocalY = y;
        }
    }
    class ControlPinOut : PinOut
    {
        public ControlPinOut(INode node, int x, int y) : base(node)
        {
            _LocalX = x;
            _LocalY = y;
        }
        public override bool CanLink(IPinIn pinIn)
        {
            return pinIn is ControlPinIn;
        }
        public override void Link(IPinIn pinIn)
        {
            if (Ins.Count > 0)
                UnLink(Ins[0]);
            if(pinIn.Outs.Count>0)
            {
                pinIn.Outs[0].UnLink(pinIn);
            }
            base.Link(pinIn);
        }
    }

    class DataPinIn : PinIn
    {
        public DataPinIn(INode node,string name, int x, int y) : base(node)
        {
            _Name = name;
            _LocalX = x;
            _LocalY = y;
        }
    }

    class DataPinOut : PinOut
    {
        public DataPinOut(INode node, string name, int x, int y) : base(node)
        {
            _Name = name;
            _LocalX = x;
            _LocalY = y;
        }
        public override bool CanLink(IPinIn pinIn)
        {
            return pinIn is DataPinIn;
        }
        public override void Link(IPinIn pinIn)
        {
            if (pinIn.Outs.Count > 0)
            {
                pinIn.Outs[0].UnLink(pinIn);
            }
            base.Link(pinIn);
        }
    }
}
