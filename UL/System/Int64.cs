namespace System
{
    public struct Int64
    {
        public const Int64 MaxValue = 0x7FFFFFFFFFFFFFFF;
        public const Int64 MinValue = 0x8000000000000000;


        public extern static Int64 Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out Int64 v)
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

        public extern static Int64 operator +(Int64 a, Int64 b);
        public extern static Int64 operator -(Int64 a, Int64 b);
        public extern static Int64 operator *(Int64 a, Int64 b);
        public extern static Int64 operator /(Int64 a, Int64 b);
        public extern static Int64 operator %(Int64 a, Int64 b);
        public extern static Int64 operator &(Int64 a, Int64 b);
        public extern static Int64 operator |(Int64 a, Int64 b);
        public extern static Int64 operator ~(Int64 a);
        public extern static Int64 operator <<(Int64 a, int b);
        public extern static Int64 operator >>(Int64 a, int b);
        public extern static Int64 operator ==(Int64 a, Int64 b);
        public extern static Int64 operator !=(Int64 a, Int64 b);
    }
}
