using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ULEditor2
{
    interface INode
    {
        int X { get; set; }
        int Y { get; set; }
        int Width { get; }
        int Height { get; }

        string Title { get; }

        string[] Inputs { get; }

        string[] ControlInputs { get; }
        string[] ControlOutputs { get; }

        string[] InputNames { get; }
        string[] OutputNames { get; }

        const int TitleHeight = 20;
        const int LineHeight = 15;

        ULNode Node { get; }
    }
}
