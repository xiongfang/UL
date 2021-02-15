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
        ULNode Node { get; }

        List<IPinIn> PinIns { get; }
        List<IPinOut> PinOuts { get; }

        const int TitleHeight = 20;
        const int LineHeight = 15;
        const int PinSize = 10;
        
    }

    interface IPin
    {
        INode Node { get; }
        int X { get; }
        int Y { get; }
        int LocalX { get; set; }
        int LocalY { get; set; }
        int Width { get; }
        int Height { get; }
        string Name { get; set; }
    }

    interface IPinOut: IPin
    {
        List<IPinIn> Ins { get; }
        void Link(IPinIn pinIn);
        void UnLink(IPinIn pinIn);
        bool CanLink(IPinIn pinIn);
    }

    interface IPinIn : IPin
    {
        List<IPinOut> Outs { get; }
        void AddLink(IPinOut po);
        void RemoveLink(IPinOut po);
    }
}
