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

        public extern static UInt16 operator +(UInt16 a, UInt16 b);
        public extern static UInt16 operator -(UInt16 a, UInt16 b);
        public extern static UInt16 operator *(UInt16 a, UInt16 b);
        public extern static UInt16 operator /(UInt16 a, UInt16 b);
        public extern static UInt16 operator %(UInt16 a, UInt16 b);
        public extern static UInt16 operator &(UInt16 a, UInt16 b);
        public extern static UInt16 operator |(UInt16 a, UInt16 b);
        public extern static bool operator >(UInt16 a, UInt16 b);
        public extern static bool operator <(UInt16 a, UInt16 b);
        public extern static UInt16 operator ~(UInt16 a);
        public extern static UInt16 operator <<(UInt16 a, int b);
        public extern static UInt16 operator >>(UInt16 a, int b);
        public extern static bool operator ==(UInt16 a, UInt16 b);
        public extern static bool operator !=(UInt16 a, UInt16 b);
        public extern static UInt16 operator ++(UInt16 a);
        public extern static UInt16 operator --(UInt16 a);

        public static explicit operator Int64(UInt16 v);
        public static implicit operator Int32(UInt16 v);
        public static implicit operator Single(UInt16 v);
        public static implicit operator Double(UInt16 v);
        public static explicit operator Byte(UInt16 v);
        public static explicit operator SByte(UInt16 v);
        public static explicit operator Int16(UInt16 v);
        public static implicit operator UInt32(UInt16 v);
        public static implicit operator UInt64(UInt16 v);
    }
}
