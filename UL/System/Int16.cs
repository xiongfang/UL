namespace System
{
    public struct Int16
    {
        public const Int16 MaxValue = 0x7FFF;
        public const Int16 MinValue = 0x8000;


        public extern static Int16 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out Int16 v)
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

        public virtual Boolean op_Equals(Int16 b);
        public virtual Boolean op_Small(Int16 b);
        public virtual Int16 op_Assign(Int16 b);
        public virtual Int16 op_PlusPlus(Int16 b);
    }
}
