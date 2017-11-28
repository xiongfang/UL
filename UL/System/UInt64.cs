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

        public extern static UInt64 operator +(UInt64 a, UInt64 b);
        public extern static UInt64 operator -(UInt64 a, UInt64 b);
        public extern static UInt64 operator *(UInt64 a, UInt64 b);
        public extern static UInt64 operator /(UInt64 a, UInt64 b);
        public extern static UInt64 operator %(UInt64 a, UInt64 b);
        public extern static UInt64 operator &(UInt64 a, UInt64 b);
        public extern static UInt64 operator |(UInt64 a, UInt64 b);
        public extern static bool operator >(UInt64 a, UInt64 b);
        public extern static bool operator <(UInt64 a, UInt64 b);
        public extern static UInt64 operator ~(UInt64 a);
        public extern static UInt64 operator <<(UInt64 a, int b);
        public extern static UInt64 operator >>(UInt64 a, int b);
        public extern static bool operator ==(UInt64 a, UInt64 b);
        public extern static bool operator !=(UInt64 a, UInt64 b);
        public extern static UInt64 operator ++(UInt64 a);
        public extern static UInt64 operator --(UInt64 a);
        public extern static UInt64 operator +(UInt64 a);
        //public extern static UInt64 operator -(UInt64 a);

        public static explicit operator Int64(UInt64 v);
        public static explicit operator Int32(UInt64 v);
        public static explicit operator Single(UInt64 v);
        public static explicit operator Double(UInt64 v);
        public static explicit operator Byte(UInt64 v);
        public static explicit operator SByte(UInt64 v);
        public static explicit operator UInt16(UInt64 v);
        public static explicit operator UInt32(UInt64 v);
        public static explicit operator Int16(UInt64 v);
    }
}
