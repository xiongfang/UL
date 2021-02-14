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
                Invalidate();
            }
        }

        Pen penBG = new Pen(new SolidBrush(Color.White));

        private void GraphEditor_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.TranslateTransform(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2);
            e.Graphics.DrawLine(penBG, new Point(-10, 0), new Point(10, 0));
            e.Graphics.DrawLine(penBG, new Point(0, -10), new Point(0, 10));

            if (_memberInfo!=null)
            {
                foreach(var n in _memberInfo.Nodes)
                {
                    DrawNode(n,e.Graphics);
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
    }
}
