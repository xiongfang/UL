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

        const int font_size = 7;
        int _width = 100;

        public NodeBase(ULNode n) { 
            node = n;
            _PinIns = new List<IPinIn>();
            _PinOuts = new List<IPinOut>();

            int control_input_count = 0;
            int control_output_count = 0;


            foreach (var ci in node.Inputs)
            {
                GetPinInputPos(control_input_count, out int x, out int y);
                if(string.IsNullOrEmpty( ci.TypeID))
                {
                    PinIns.Add(new ControlPinIn(this, control_input_count, ci.Name, x, y));
                }
                else
                {
                    PinIns.Add(new DataPinIn(this, ci, control_input_count, x, y));
                }
                control_input_count++;
            }

            foreach (var co in node.Outputs)
            {
                GetPinOutputPos(control_output_count, out int x, out int y);
                if (string.IsNullOrEmpty(co.TypeID))
                {
                    PinOuts.Add(new ControlPinOut(this, control_output_count, co.Name, x, y));
                }
                else
                {
                    PinOuts.Add(new DataPinOut(this, co, control_output_count, x, y));
                }
                control_output_count++;
            }

            int left_width = 0;
            int right_width = 0;
            //计算宽度
            foreach (var ci in PinIns)
            {
                left_width = Math.Max(left_width, ci.Name.Length * font_size);
            }
            foreach (var co in PinOuts)
            {
                right_width = Math.Max(right_width, co.Name.Length * font_size);
            }

            _width = left_width + right_width + 20;
            _width = Math.Max(100, _width);
            _width = Math.Max(font_size*Title.Length, _width);

            //重新计算端口位置
            for (int i=0;i<PinIns.Count;i++)
            {
                GetPinInputPos(i, out int x, out int y);
                PinIns[i].LocalX = x;
                PinIns[i].LocalY = y;
            }

            for (int i = 0; i < PinOuts.Count; i++)
            {
                GetPinOutputPos(i, out int x, out int y);
                PinOuts[i].LocalX = x;
                PinOuts[i].LocalY = y;
            }
        }

        public virtual void PostInit(System.Func<string, INode> find) { foreach (var p in PinIns) p.PostInit(find); foreach (var p in PinOuts) p.PostInit(find); }

        public ULNode Data { get { return node; } }

        public int X { get => node.X; set => node.X = value; }
        public int Y { get => node.Y; set => node.Y = value; }


        public virtual int Width => _width;

        public virtual int Height => INode.TitleHeight + (int)(MathF.Max(PinIns.Count, PinOuts.Count) * INode.LineHeight);

        public virtual string Title
        {
            get
            {
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
        public virtual string Name { get => _Name;  }

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
        public ControlPinIn(INode node, int index,string name, int x,int y) : base(node)
        {
            _Name = name;
            _LocalX = x;
            _LocalY = y;
            _index = index;
        }
        public int Index { get { return _index; } }


        public override void AddLink(IPinOut po)
        {
            base.AddLink(po);
            ControlPinOut controlPinOut = po as ControlPinOut;
            controlPinOut.Node.Data.LinkTo(Node.Data, controlPinOut.Index, Index);
        }
        public override void RemoveLink(IPinOut po)
        {
            ControlPinOut controlPinOut = po as ControlPinOut;
            controlPinOut.Node.Data.UnLinkTo(Node.Data, controlPinOut.Index, Index);
            base.RemoveLink(po);

        }
    }
    class ControlPinOut : PinOut
    {
        int _index;
        public int Index { get { return _index; } }

        public ControlPinOut(INode node,int index, string name, int x, int y) : base(node)
        {
            _Name = name;
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
            if(!string.IsNullOrEmpty(Node.Data.Outputs[Index].Link))
            {
                string[] args = Node.Data.Outputs[Index].Link.Split('.');

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
        ULPin _pin;

        public DataPinIn(INode node, ULPin a,int idx, int x, int y) : base(node)
        {
            _index = idx;
            _pin = a;
            _LocalX = x;
            _LocalY = y;
        }
        public ULTypeInfo typeInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(_pin.TypeID) && Data.types.TryGetValue(_pin.TypeID, out var t))
                {
                    return t;
                }

                return null;
            }
        }
        public Color color { 
            get
            {
                if (typeInfo!=null)
                {
                    var name = typeInfo.Name;
                    var c = Color.FromArgb((int)(name.GetHashCode()|0xff000000));
                    return c;
                }

                return Color.Black;
            } 
        }

        public override string Name { 
            get
            {
                if (typeInfo != null)
                {
                    return _pin.Name + " " + typeInfo.Name;
                }
                return _pin.Name;
            } 
        }
        public override void AddLink(IPinOut po)
        {
            base.AddLink(po);
            DataPinOut dataPinOut = po as DataPinOut;
            dataPinOut.Node.Data.LinkTo(Node.Data, dataPinOut.Index, _index);
        }

        public override void RemoveLink(IPinOut po)
        {
            base.RemoveLink(po);
            DataPinOut dataPinOut = po as DataPinOut;
            dataPinOut.Node.Data.UnLinkTo(Node.Data, dataPinOut.Index, _index);
        }


        public override void PostInit(Func<string, INode> find)
        {
            base.PostInit(find);

            if (!string.IsNullOrEmpty(Node.Data.Inputs[_index].Link))
            {
                string[] args = Node.Data.Inputs[_index].Link.Split('.');

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
        ULPin _arg;
        int _index;
        public int Index { get { return _index; } }
        public DataPinOut(INode node, ULPin a, int idx, int x, int y) : base(node)
        {
            _index = idx;
            _arg = a;
            _LocalX = x;
            _LocalY = y;
        }
        public ULTypeInfo typeInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(_arg.TypeID) && Data.types.TryGetValue(_arg.TypeID, out var t))
                {
                    return t;
                }

                return null;
            }
        }
        public Color color
        {
            get
            {
                if (typeInfo!=null)
                {
                    var name = typeInfo.Name;
                    var c = Color.FromArgb((int)(name.GetHashCode() | 0xff000000));
                    return c;
                }

                return Color.Black;
            }
        }

        public override string Name
        {
            get
            {
                if (typeInfo != null)
                {
                    return typeInfo.Name+ " " +_arg.Name;
                }
                return _arg.Name;
            }
        }

        public override bool CanLink(IPinIn pinIn)
        {
            return pinIn is DataPinIn && (pinIn as DataPinIn).typeInfo.ID == typeInfo.ID;
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
