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
        Pen penControlLink = new Pen(new SolidBrush(Color.YellowGreen),3);
        Pen penData = new Pen(new SolidBrush(Color.Blue));
        Pen penDataLink = new Pen(new SolidBrush(Color.Yellow),3);
        public void Draw(INode node, Graphics g,GraphEditor editor)
        {
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(node.X, node.Y, node.Width, node.Height));

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(node.X, node.Y, node.Width, INode.TitleHeight));

            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(node.Title, font, titleBrush, new RectangleF(node.X, node.Y, node.Width, INode.TitleHeight), stringFormat);

            foreach(var p in node.PinIns)
            {
                if(p is ControlPinIn)
                    g.DrawEllipse(penControl, new Rectangle(p.X, p.Y, p.Width, p.Height));
                else if(p is DataPinIn)
                    g.DrawEllipse(penData, new Rectangle(p.X, p.Y, p.Width, p.Height));

            }

            foreach (var p in node.PinOuts)
            {
                if(p is ControlPinOut)
                {
                    g.DrawEllipse(penControl, new Rectangle(p.X, p.Y, p.Width, p.Height));
                    foreach (var pin in p.Ins)
                    {
                        var ptStart = p.Center;
                        var ptEnd = pin.Center;
                        var pt1 = new Point(ptStart.X + 10, ptStart.Y);
                        var pt2 = new Point(ptEnd.X - 10, ptEnd.Y);
                        g.DrawBezier(penControlLink, ptStart, pt1, pt2, ptEnd);
                    }
                }
                    
                else if(p is DataPinOut)
                {
                    g.DrawEllipse(penData, new Rectangle(p.X, p.Y, p.Width, p.Height));
                    foreach (var pin in p.Ins)
                    {
                        var ptStart = p.Center;
                        var ptEnd = pin.Center;
                        var pt1 = new Point(ptStart.X + 10, ptStart.Y);
                        var pt2 = new Point(ptEnd.X - 10, ptEnd.Y);
                        g.DrawBezier(penDataLink, ptStart, pt1, pt2, ptEnd);
                    }
                }
                    
            }
        }
    }
}
