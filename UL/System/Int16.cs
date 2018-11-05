namespace ul
{
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

            public extern static Int16 operator +(Int16 a, Int16 b);
            public extern static Int16 operator -(Int16 a, Int16 b);
            public extern static Int16 operator *(Int16 a, Int16 b);
            public extern static Int16 operator /(Int16 a, Int16 b);
            public extern static Int16 operator %(Int16 a, Int16 b);
            public extern static Int16 operator &(Int16 a, Int16 b);
            public extern static Int16 operator |(Int16 a, Int16 b);
            public extern static bool operator >(Int16 a, Int16 b);
            public extern static bool operator <(Int16 a, Int16 b);
            public extern static Int16 operator ~(Int16 a);
            public extern static Int16 operator <<(Int16 a, int b);
            public extern static Int16 operator >>(Int16 a, int b);
            public extern static bool operator ==(Int16 a, Int16 b);
            public extern static bool operator !=(Int16 a, Int16 b);
            public extern static Int16 operator ++(Int16 a);
            public extern static Int16 operator --(Int16 a);

            public extern static implicit operator Int64(Int16 v);
            public extern static implicit operator Int32(Int16 v);
            public extern static implicit operator Single(Int16 v);
            public extern static implicit operator Double(Int16 v);
            public extern static explicit operator Byte(Int16 v);
            public extern static explicit operator SByte(Int16 v);
            public extern static explicit operator UInt16(Int16 v);
            public extern static explicit operator UInt32(Int16 v);
            public extern static explicit operator UInt64(Int16 v);
        }
    }
}