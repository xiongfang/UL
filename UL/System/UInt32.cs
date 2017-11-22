namespace System
{
    public struct UInt32
    {
        public const UInt32 MaxValue = 0xFFFFFFFF;
        public const UInt32 MinValue = 0;


        public extern static UInt32 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out UInt32 v)
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
    }
}
