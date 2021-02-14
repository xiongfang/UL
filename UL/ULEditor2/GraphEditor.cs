using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                    this.Enabled = true;
                    OffsetX = _memberInfo.Graph.OffsetX;
                    OffsetY = _memberInfo.Graph.OffsetY;
                }
                Invalidate();
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

        ULNode _selectedNode;

        public System.Action<ULNode> onSelectNodeChanged;

        ULNode selectedNode { get { return _selectedNode; }
            set
            {
                if(_selectedNode!=value)
                {
                    _selectedNode = value;
                    onSelectNodeChanged?.Invoke(_selectedNode);
                }
            }
        }

        int NodeWidth = 100;
        int NodeHeight = 50;

        int OffsetX = 0;
        int OffsetY = 0;



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
        }


        Pen penBG = new Pen(new SolidBrush(Color.White));
        Pen penSelected = new Pen(new SolidBrush(Color.Yellow));

        private void GraphEditor_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.TranslateTransform(OffsetX,OffsetY);
            e.Graphics.DrawLine(penBG, new Point(-10, 0), new Point(10, 0));
            e.Graphics.DrawLine(penBG, new Point(0, -10), new Point(0, 10));

            if (graph != null)
            {
                foreach(var n in graph.Nodes)
                {
                    DrawNode(n,e.Graphics);
                }

                if(_selectedNode!=null)
                {
                    e.Graphics.DrawRectangle(penSelected, _selectedNode.X - 1, _selectedNode.Y - 1, NodeWidth + 2, NodeHeight + 2);
                }
            }
        }

        void DrawNode(ULNode node,Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(node.X, node.Y, 100, 50));
            if (node.Name == null)
                node.Name = "";

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(node.X, node.Y, 100, 20));

            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Alignment = StringAlignment.Center;
            if (Data.members.TryGetValue(node.Name,out var m))
            {
                g.DrawString(m.Name, Font, new SolidBrush(BackColor),new RectangleF(node.X, node.Y,NodeWidth,20), stringFormat);
            }
            else
            {
                g.DrawString(node.Name, Font, new SolidBrush(BackColor), new RectangleF(node.X, node.Y, NodeWidth, 20), stringFormat);
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
                Invalidate();
            }
            else if(e.ClickedItem.Name == "DeleteNode")
            {
                if(selectedNode!=null)
                {
                    graph.Nodes.Remove(selectedNode);
                }
                selectedNode = null;
                Invalidate();
            }
            else if(new List<string>(ULNode.keywords).Contains(e.ClickedItem.Name))
            {
                var node = new ULNode();
                node.NodeID = Guid.NewGuid().ToString();
                node.Name = e.ClickedItem.Name;
                graph.Nodes.Add(node);
                Invalidate();
            }
        }
        Point PointToContext(Point pointClient)
        {
            return new Point(pointClient.X - OffsetX, pointClient.Y- OffsetY);
        }

        bool mouseDown;
        Point lastMousePoint;
        private void GraphEditor_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastMousePoint = e.Location;

            var contextPoint = PointToContext(e.Location);
            foreach (var n in graph.Nodes)
            {
                if(new Rectangle(n.X,n.Y,NodeWidth,NodeHeight).Contains(contextPoint))
                {
                    selectedNode = n;
                    Invalidate();
                    return;
                }
            }
        }

        private void GraphEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown && _selectedNode != null && e.Button == MouseButtons.Left)
            {
                _selectedNode.X += e.Location.X - lastMousePoint.X;
                _selectedNode.Y += e.Location.Y - lastMousePoint.Y;
                lastMousePoint = e.Location;
                Invalidate();
            }
            else if(mouseDown && e.Button == MouseButtons.Middle)
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

        }

        private void GraphEditor_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
