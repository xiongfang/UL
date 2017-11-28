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

        public extern static Int32 operator +(Int32 a, Int32 b);
        public extern static Int32 operator -(Int32 a, Int32 b);
        public extern static Int32 operator *(Int32 a, Int32 b);
        public extern static Int32 operator /(Int32 a, Int32 b);
        public extern static Int32 operator %(Int32 a, Int32 b);
        public extern static Int32 operator &(Int32 a, Int32 b);
        public extern static Int32 operator |(Int32 a, Int32 b);
        public extern static bool operator >(Int32 a, Int32 b);
        public extern static bool operator <(Int32 a, Int32 b);
        public extern static bool operator ==(Int32 a, Int32 b);
        public extern static bool operator !=(Int32 a, Int32 b);

        public extern static Int32 operator <<(Int32 a, int b);
        public extern static Int32 operator >>(Int32 a, int b);

        public extern static Int32 operator ++(Int32 a);
        public extern static Int32 operator --(Int32 a);
        public extern static Int32 operator ~(Int32 a);

        public extern static Int32 operator +(Int32 a);
        public extern static Int32 operator -(Int32 a);

        //public extern static Int32 operator !(Int32 a);

        public static implicit operator Int64(Int32 v);
        public static explicit operator Int16(Int32 v);
        public static implicit operator Single(Int32 v);
        public static implicit operator Double(Int32 v);
        public static explicit operator Byte(Int32 v);
        public static explicit operator SByte(Int32 v);
        public static explicit operator UInt16(Int32 v);
        public static explicit operator UInt32(Int32 v);
        public static explicit operator UInt64(Int32 v);
    }
}
