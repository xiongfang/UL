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
        Font fontData = SystemFonts.DefaultFont;
        Brush titleBrush = new SolidBrush(Color.Black);
        Pen penControl = new Pen(new SolidBrush(Color.Red));
        Pen penControlLink = new Pen(new SolidBrush(Color.YellowGreen),3);
        Pen penData = new Pen(new SolidBrush(Color.Blue));
        Pen penDataLink = new Pen(new SolidBrush(Color.Yellow),3);
        public void Draw(INode node, Graphics g,GraphEditor editor)
        {
            g.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(node.X, node.Y, node.Width, node.Height));

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(node.X, node.Y, node.Width, INode.TitleHeight));

            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip|StringFormatFlags.NoWrap);
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Trimming = StringTrimming.None;
            g.DrawString(node.Title, font, titleBrush, new RectangleF(node.X, node.Y, node.Width, INode.TitleHeight), stringFormat);

            StringFormat stringFormatLeft = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
            stringFormatLeft.Alignment = StringAlignment.Near;
            stringFormatLeft.LineAlignment = StringAlignment.Center;
            stringFormatLeft.Trimming = StringTrimming.None;

            StringFormat stringFormatRight = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap);
            stringFormatRight.Alignment = StringAlignment.Far;
            stringFormatRight.LineAlignment = StringAlignment.Center;
            stringFormatRight.Trimming = StringTrimming.None;

            foreach (var p in node.PinIns)
            {
                if(p is ControlPinIn)
                {
                    g.DrawEllipse(penControl, new Rectangle(p.X, p.Y, p.Width, p.Height));
                    g.DrawString(p.Name, fontData, titleBrush, new RectangleF(node.X, p.Y, node.Width / 2, INode.LineHeight), stringFormatLeft);
                }
                    
                else if(p is DataPinIn)
                {
                    penData.Color = (p as DataPinIn).color;
                    g.FillEllipse(penData.Brush, new Rectangle(p.X, p.Y, p.Width, p.Height));

                    g.DrawString(p.Name, fontData, titleBrush, new RectangleF(node.X, p.Y, node.Width / 2, INode.LineHeight), stringFormatLeft);
                }
            }

            foreach (var p in node.PinOuts)
            {
                if(p is ControlPinOut)
                {
                    g.DrawEllipse(penControl, new Rectangle(p.X, p.Y, p.Width, p.Height));
                    g.DrawString(p.Name, fontData, titleBrush, new RectangleF(node.X + node.Width / 2, p.Y, node.Width / 2, INode.LineHeight), stringFormatRight);
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
                    penData.Color = (p as DataPinOut).color;
                    g.FillEllipse(penData.Brush, new Rectangle(p.X, p.Y, p.Width, p.Height));
                    g.DrawString(p.Name, fontData, titleBrush, new RectangleF(node.X+ node.Width/2, p.Y, node.Width / 2, INode.LineHeight), stringFormatRight);
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
