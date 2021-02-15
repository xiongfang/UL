using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        ULNode Data { get; }

        List<IPinIn> PinIns { get; }
        List<IPinOut> PinOuts { get; }

        void PostInit(System.Func<string,INode> find);

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
        Point Center { get; }
        string Name { get; }
        void PostInit(System.Func<string, INode> find);
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
