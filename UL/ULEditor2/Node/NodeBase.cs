using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public virtual void PostInit(System.Func<string, INode> find) { foreach (var p in PinOuts) p.PostInit(find); }

        public ULNode Data { get { return node; } }

        public int X { get => node.X; set => node.X = value; }
        public int Y { get => node.Y; set => node.Y = value; }


        public virtual int Width => 100;

        public virtual int Height => INode.TitleHeight + (int)(MathF.Max(PinIns.Count, PinOuts.Count) * INode.LineHeight);

        public virtual string Title
        {
            get
            {
                if(Model.Data.members.TryGetValue(node.Name, out var m))
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

        public Point Center => new Point(X + Width / 2, Y + Height / 2);

        public virtual void PostInit(Func<string, INode> find)
        {
        }
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
        int _index;
        public ControlPinIn(INode node, int index, int x,int y) : base(node)
        {
            _LocalX = x;
            _LocalY = y;
            _index = index;
        }
        public int Index { get { return _index; } }

        public string RefString { get { return Node.Data.NodeID + "." + Index; } }

        public override void AddLink(IPinOut po)
        {
            base.AddLink(po);
            ControlPinOut controlPinOut = po as ControlPinOut;
            controlPinOut.Node.Data.ControlOutputs[controlPinOut.Index] = RefString;
            Node.Data.ControlInputs[Index] = controlPinOut.RefString;
        }
        public override void RemoveLink(IPinOut po)
        {
            ControlPinOut controlPinOut = po as ControlPinOut;
            controlPinOut.Node.Data.ControlOutputs[controlPinOut.Index] = "";
            Node.Data.ControlInputs[Index] = "";
            base.RemoveLink(po);

        }
    }
    class ControlPinOut : PinOut
    {
        int _index;
        public int Index { get { return _index; } }
        public string RefString { get { return Node.Data.NodeID + "." + Index; } }
        public ControlPinOut(INode node,int index, int x, int y) : base(node)
        {
            _index = index;
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

        public override void PostInit(Func<string, INode> find)
        {
            base.PostInit(find);
            if(!string.IsNullOrEmpty(Node.Data.ControlOutputs[Index]))
            {
                string[] args = Node.Data.ControlOutputs[Index].Split('.');

                var node = find(args[0]);
                if(node!=null)
                {
                    var pinIn = node.PinIns[Int32.Parse(args[1])];
                    Ins.Add(pinIn);
                    pinIn.Outs.Add(this);
                }
            }
        }
    }

    class DataPinIn : PinIn
    {
        int _index;

        public DataPinIn(INode node,string name,int idx, int x, int y) : base(node)
        {
            _index = idx;
            _Name = name;
            _LocalX = x;
            _LocalY = y;
        }

        public override void AddLink(IPinOut po)
        {
            base.AddLink(po);
            DataPinOut dataPinOut = po as DataPinOut;
            Node.Data.Inputs[_index] = dataPinOut.Node.Data.NodeID+"."+ dataPinOut.Index;
        }

        public override void RemoveLink(IPinOut po)
        {
            base.RemoveLink(po);
            Node.Data.Inputs[_index] = "";
        }


        public override void PostInit(Func<string, INode> find)
        {
            base.PostInit(find);

            if (!string.IsNullOrEmpty(Node.Data.Inputs[_index]))
            {
                string[] args = Node.Data.Inputs[_index].Split('.');

                var node = find(args[0]);
                if (node != null)
                {
                    var dataPins = node.PinOuts.Select((v)=>v as DataPinOut).Where((v)=>v is DataPinOut).ToArray();
                    var pinOut = dataPins[Int32.Parse(args[1])];
                    Outs.Add(pinOut);
                    pinOut.Ins.Add(this);
                }
            }
        }
    }

    class DataPinOut : PinOut
    {
        int _index;
        public int Index { get { return _index; } }
        public DataPinOut(INode node, string name, int idx, int x, int y) : base(node)
        {
            _index = idx;
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
