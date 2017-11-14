namespace System
{
    public struct UInt64
    {
        public const UInt64 MaxValue = 0xFFFFFFFFFFFFFFFF;
        public const UInt64 MinValue = 0;


        public extern static UInt64 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out UInt64 v)
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


        public virtual Boolean op_Equals(UInt64 b);
        public virtual Boolean op_Small(UInt64 b);
        public virtual UInt64 op_Assign(UInt64 b);
        public virtual UInt64 op_PlusPlus(UInt64 b);
    }
}
