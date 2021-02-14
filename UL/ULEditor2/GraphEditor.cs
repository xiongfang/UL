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
                Invalidate();
            }
        }
        ULNode _selectedNode;
        int NodeWidth = 100;
        int NodeHeight = 50;

        int OffsetX = 0;
        int OffsetY = 0;


        Pen penBG = new Pen(new SolidBrush(Color.White));
        Pen penSelected = new Pen(new SolidBrush(Color.Yellow));
        private void GraphEditor_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.TranslateTransform(OffsetX,OffsetY);
            e.Graphics.DrawLine(penBG, new Point(-10, 0), new Point(10, 0));
            e.Graphics.DrawLine(penBG, new Point(0, -10), new Point(0, 10));

            if (_memberInfo!=null)
            {
                foreach(var n in _memberInfo.Nodes)
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
            g.FillRectangle(SystemBrushes.Control, new Rectangle(node.X, node.Y, 100, 50));
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var item = contextMenuStrip1.Items.Add("AddNode");


            e.Cancel = false;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Text == "AddNode")
            {
                var node = new ULNode();
                node.MethodID = memberInfo.ID;
                node.NodeID = Guid.NewGuid().ToString();
                Data.nodes.Add(node);
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
            if (memberInfo == null)
                return;
            foreach (var n in memberInfo.Nodes)
            {
                if(new Rectangle(n.X,n.Y,NodeWidth,NodeHeight).Contains(contextPoint))
                {
                    _selectedNode = n;
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
                lastMousePoint = e.Location;

                Invalidate();
            }

        }

        private void GraphEditor_Load(object sender, EventArgs e)
        {
            OffsetX = ClientRectangle.Width / 2;
            OffsetY = ClientRectangle.Height / 2;
        }

        private void GraphEditor_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
