using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ULEditor2
{
    interface INodeDrawer
    {
        void Draw(INode node,Graphics g, GraphEditor editor);
    }
}
