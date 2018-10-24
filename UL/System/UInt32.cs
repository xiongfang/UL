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

        public extern static UInt32 operator +(UInt32 a, UInt32 b);
        public extern static UInt32 operator -(UInt32 a, UInt32 b);
        public extern static UInt32 operator *(UInt32 a, UInt32 b);
        public extern static UInt32 operator /(UInt32 a, UInt32 b);
        public extern static UInt32 operator %(UInt32 a, UInt32 b);
        public extern static UInt32 operator &(UInt32 a, UInt32 b);
        public extern static UInt32 operator |(UInt32 a, UInt32 b);
        public extern static bool operator >(UInt32 a, UInt32 b);
        public extern static bool operator <(UInt32 a, UInt32 b);
        public extern static UInt32 operator ~(UInt32 a);
        public extern static UInt32 operator <<(UInt32 a, int b);
        public extern static UInt32 operator >>(UInt32 a, int b);
        public extern static bool operator ==(UInt32 a, UInt32 b);
        public extern static bool operator !=(UInt32 a, UInt32 b);
        public extern static UInt32 operator ++(UInt32 a);
        public extern static UInt32 operator --(UInt32 a);
        public extern static UInt32 operator +(UInt32 a);
        //public extern static UInt32 operator -(UInt32 a);

        public static explicit operator Int64(UInt32 v);
        public static explicit operator Int32(UInt32 v);
        public static explicit operator Single(UInt32 v);
        public static explicit operator Double(UInt32 v);
        public static explicit operator Byte(UInt32 v);
        public static explicit operator SByte(UInt32 v);
        public static explicit operator UInt16(UInt32 v);
        public static implicit operator UInt64(UInt32 v);
        public static explicit operator Int16(UInt32 v);
    }
}
