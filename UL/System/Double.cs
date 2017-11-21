namespace System
{
    public struct Double
    {
        public const double Epsilon = 4.94065645841247e-324;
        public const double MaxValue = 1.79769313486231e308;
        public const double MinValue = -1.79769313486231e308;

        public virtual Boolean op_Small(Double b);
        public virtual Double op_PlusPlus(Double b);
    }
}
