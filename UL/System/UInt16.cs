namespace System
{
    public struct UInt16
    {
        public const UInt16 MaxValue = 0xFFFF;
        public const UInt16 MinValue = 0;


        public extern static UInt16 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out UInt16 v)
        {
            try
            {
                v = Parse(value);
                return true;
            }
            catch (Exception e)
            {
                v = 0;
                return false;
            }
        }

        public virtual Boolean op_Small(UInt16 b);
        public virtual UInt16 op_PlusPlus(UInt16 b);
    }
}
