namespace System
{
    public struct SByte
    {
        public const SByte MaxValue = 127;
        public const SByte MinValue = -128;


        public extern static SByte Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out SByte v)
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
