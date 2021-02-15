using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Model;

namespace ULEditor2
{
    public partial class GraphEditor : UserControl
    {
        public GraphEditor()
        {
            InitializeComponent();
        }

        ULMemberInfo _memberInfo;
        public ULMemberInfo memberInfo
        {
            get { return _memberInfo; }
            set
            {
                _memberInfo = value;
                _selectedNode = null;
                if(_memberInfo == null || _memberInfo.Graph ==null)
                {
                    this.Enabled = false;
                }
                else
                {

                    ResetNodes();
                    this.Enabled = true;
                    OffsetX = _memberInfo.Graph.OffsetX;
                    OffsetY = _memberInfo.Graph.OffsetY;
                }
                Invalidate();
            }
        }

        public void ResetNodes()
        {
            nodes.Clear();

            foreach (var n in graph.Nodes)
            {
                nodes.Add(CreateNodeWrapper(n));
            }
            foreach (var n in nodes)
            {
                n.PostInit((v) => nodes.Find(x => x.Data.NodeID == v));
            }
        }

        ULGraph graph
        {
            get
            {
                if (_memberInfo != null)
                    return _memberInfo.Graph;
                return null;
            }
        }

        INode _selectedNode;
        IPinOut _selectedPin;

        public System.Action<ULNode> onSelectNodeChanged;

        INode selectedNode { get { return _selectedNode; }
            set
            {
                if(_selectedNode!=value)
                {
                    _selectedNode = value;
                    if(_selectedNode!=null)
                        onSelectNodeChanged?.Invoke(_selectedNode.Data);
                    else
                        onSelectNodeChanged?.Invoke(null);
                }
            }
        }

        int OffsetX = 0;
        int OffsetY = 0;
        float ScaleFactor = 1.0f;

        private void GraphEditor_Load(object sender, EventArgs e)
        {
            OffsetX = ClientRectangle.Width / 2;
            OffsetY = ClientRectangle.Height / 2;

            // Create a new MenuStrip control and add a ToolStripMenuItem.
            contextMenuStrip1.Items.Add("添加函数").Name = "AddNode";
            ToolStripMenuItem add_node = new ToolStripMenuItem("添加控制");
            contextMenuStrip1.Items.Add(add_node);

            foreach(var k in ULNode.keywords)
            {
                add_node.DropDownItems.Add(k).Name = k;
            }

            var item = contextMenuStrip1.Items.Add("删除节点");
            item.Name = "DeleteNode";

            add_node.DropDownItemClicked += contextMenuStrip1_ItemClicked;

            labelScale.Text = string.Format("视图缩放：{0:F2}", ScaleFactor);
        }


        INode CreateNodeWrapper(ULNode node)
        {
            switch (node.Name)
            {
                case "if":
                    return new IF_Node(node);
                default:
                    break;
            }
            return new MethodNode(node);
        }

        Pen penBG = new Pen(new SolidBrush(Color.White));
        Pen penSelected = new Pen(new SolidBrush(Color.Yellow),2);
        Pen penLinkNode = new Pen(new SolidBrush(Color.Yellow), 2);
        List<INode> nodes = new List<INode>();
        NodeDrawer drawer = new NodeDrawer();

        private void GraphEditor_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.TranslateTransform(OffsetX,OffsetY);
            e.Graphics.ScaleTransform(ScaleFactor, ScaleFactor);

            e.Graphics.DrawLine(penBG, new Point(-10, 0), new Point(10, 0));
            e.Graphics.DrawLine(penBG, new Point(0, -10), new Point(0, 10));

            if (graph != null)
            {
                foreach(var n in nodes)
                {
                    drawer.Draw(n, e.Graphics,this);
                }

                if(_selectedNode!=null)
                {
                    var node = _selectedNode;
                    e.Graphics.DrawRectangle(penSelected, node.X - 2, node.Y - 2, node.Width + 2, node.Height + 2);
                }
            }

            if(editMode == EEditMode.LinkNode)
            {
                var ptStart = _selectedPin.Center;
                var ptEnd = PointToContext(lastMousePoint);
                var pt1 = new Point(ptStart.X + 10, ptStart.Y);
                var pt2 = new Point(ptEnd.X - 10, ptEnd.Y);
                e.Graphics.DrawBezier(penLinkNode, ptStart, pt1, pt2, ptEnd);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            e.Cancel = false;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Name == "AddNode")
            {
                var node = new ULNode();
                node.NodeID = Guid.NewGuid().ToString();
                graph.Nodes.Add(node);
                ResetNodes();
                Invalidate();
            }
            else if(e.ClickedItem.Name == "DeleteNode")
            {
                if(selectedNode!=null)
                {
                    graph.Nodes.Remove(selectedNode.Data);
                }
                selectedNode = null;
                ResetNodes();
                Invalidate();
            }
            else if(new List<string>(ULNode.keywords).Contains(e.ClickedItem.Name))
            {
                var node = new ULNode();
                node.NodeID = Guid.NewGuid().ToString();
                node.Name = e.ClickedItem.Name;
                graph.Nodes.Add(node);
                ResetNodes();
                Invalidate();
            }
        }
        Point PointToContext(Point pointClient)
        {
            Matrix matrix = new Matrix();
            matrix.Translate(OffsetX, OffsetY);
            matrix.Scale(ScaleFactor, ScaleFactor);
            matrix.Invert();

            var pts = new PointF[] { pointClient };
            matrix.TransformPoints(pts);
            return new Point((int)pts[0].X,(int)pts[0].Y);
        }

        enum EEditMode
        {
            None,
            MoveScreen,
            MoveNode,
            LinkNode,
        }
        EEditMode editMode;
        bool mouseDown;
        Point lastMousePoint;
        private void GraphEditor_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastMousePoint = e.Location;
            editMode = EEditMode.None;
            if(e.Button == MouseButtons.Left)
            {
                var contextPoint = PointToContext(e.Location);
                foreach (var n in nodes)
                {
                    if (new Rectangle(n.X, n.Y, n.Width, n.Height).Contains(contextPoint))
                    {
                        editMode = EEditMode.MoveNode;
                        selectedNode = n;
                        Invalidate();
                        return;
                    }
                    else
                    {
                        foreach (var pinIn in n.PinOuts)
                        {
                            if (new Rectangle(pinIn.X, pinIn.Y, pinIn.Width, pinIn.Height).Contains(contextPoint))
                            {
                                editMode = EEditMode.LinkNode;
                                _selectedPin = pinIn;
                                Invalidate();
                                return;
                            }
                        }
                    }
                }
            }
            else if(e.Button == MouseButtons.Middle)
            {
                editMode = EEditMode.MoveScreen;
            }
        }

        private void GraphEditor_MouseEnter(object sender, EventArgs e)
        {
            //获得焦点，以触发滚轮事件
            trackBar1.Focus();

        }
        private void GraphEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if(editMode == EEditMode.MoveNode)
            {
                _selectedNode.X += e.Location.X - lastMousePoint.X;
                _selectedNode.Y += e.Location.Y - lastMousePoint.Y;
                lastMousePoint = e.Location;
                Invalidate();
            }
            else if(editMode == EEditMode.MoveScreen)
            {

                OffsetX += e.Location.X - lastMousePoint.X;
                OffsetY += e.Location.Y - lastMousePoint.Y;
                if(graph!=null)
                {
                    graph.OffsetX = OffsetX;
                    graph.OffsetY = OffsetY;
                }

                lastMousePoint = e.Location;

                Invalidate();
            }
            else if(editMode == EEditMode.LinkNode)
            {
                lastMousePoint = e.Location;
                Invalidate();
            }
        }

        private void GraphEditor_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if(editMode == EEditMode.LinkNode)
            {
                var contextPoint = PointToContext(e.Location);
                foreach (var n in nodes)
                {
                    foreach (var pinIn in n.PinIns)
                    {
                        if (new Rectangle(pinIn.X, pinIn.Y, pinIn.Width, pinIn.Height).Contains(contextPoint))
                        {
                            if(_selectedPin.CanLink(pinIn))
                            {
                                _selectedPin.Link(pinIn);
                            }
                            editMode = EEditMode.None;
                            Invalidate();
                            return;
                        }
                    }
                }
            }
            editMode = EEditMode.None;
            Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Value >= 50)
            {
                ScaleFactor = 1.0f + (trackBar1.Value - 50.0f) / 50*3;
            }
            else
            {
                ScaleFactor = MathF.Max(0.1f,trackBar1.Value / 50.0f);
            }
            labelScale.Text = string.Format( "视图缩放：{0:F2}" , ScaleFactor);
            Invalidate();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar1.Maximum/2;

            OffsetX = ClientRectangle.Width / 2;
            OffsetY = ClientRectangle.Height / 2;

            Invalidate();
        }
    }
}
