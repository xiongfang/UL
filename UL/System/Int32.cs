namespace System
{
    public struct Int32
    {
        public const Int32 MaxValue = 0x7FFFFFFF;
        public const Int32 MinValue = 0x80000000;


        public extern static Int32 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out Int32 v)
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
