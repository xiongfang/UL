namespace ul
{
    namespace System
    {
        public struct Byte
        {
            public const byte MaxValue = 255;
            public const byte MinValue = 0;


            public extern static Byte Parse(string value);

            public extern override string ToString();

            public static bool TryParse(string value, out Byte v)
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

            public extern static Byte operator +(Byte a, Byte b);
            public extern static Byte operator -(Byte a, Byte b);
            public extern static Byte operator *(Byte a, Byte b);
            public extern static Byte operator /(Byte a, Byte b);
            public extern static Byte operator %(Byte a, Byte b);
            public extern static Byte operator &(Byte a, Byte b);
            public extern static Byte operator |(Byte a, Byte b);
            public extern static bool operator >(Byte a, Byte b);
            public extern static bool operator <(Byte a, Byte b);
            public extern static Byte operator ~(Byte a);
            public extern static Byte operator <<(Byte a, int b);
            public extern static Byte operator >>(Byte a, int b);
            public extern static bool operator ==(Byte a, Byte b);
            public extern static bool operator !=(Byte a, Byte b);
            public extern static Byte operator ++(Byte a);
            public extern static Byte operator --(Byte a);
            public extern static Byte operator +(Byte a);
            public extern static Byte operator -(Byte a);

            public extern static implicit operator Int64(Byte v);
            public extern static implicit operator Int32(Byte v);
            public extern static implicit operator Single(Byte v);
            public extern static implicit operator Double(Byte v);
            public extern static implicit operator Int16(Byte v);
            public extern static explicit operator SByte(Byte v);
            public extern static explicit operator UInt16(Byte v);
            public extern static explicit operator UInt32(Byte v);
            public extern static explicit operator UInt64(Byte v);
        }
    }
}