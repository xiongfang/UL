using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ULEditor2
{
    class NodeDrawer : INodeDrawer
    {
        Font font = SystemFonts.DefaultFont;
        Brush titleBrush = new SolidBrush(Color.Black);
        Pen penControl = new Pen(new SolidBrush(Color.Red));
        public void Draw(INode node, Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(node.X, node.Y, node.Width, node.Height));

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(node.X, node.Y, node.Width, INode.TitleHeight));

            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(node.Title, font, titleBrush, new RectangleF(node.X, node.Y, node.Width, INode.TitleHeight), stringFormat);

            for(int i=0;i<node.ControlOutputs.Length;i++)
            {
                g.DrawEllipse(penControl, new Rectangle(node.X + node.Width, node.Y + INode.TitleHeight + i * INode.LineHeight+2, INode.LineHeight-4, INode.LineHeight-4));
            }
        }
    }
}
