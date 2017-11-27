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

        public extern static SByte operator +(SByte a, SByte b);
        public extern static SByte operator -(SByte a, SByte b);
        public extern static SByte operator *(SByte a, SByte b);
        public extern static SByte operator /(SByte a, SByte b);
        public extern static SByte operator %(SByte a, SByte b);
        public extern static SByte operator &(SByte a, SByte b);
        public extern static SByte operator |(SByte a, SByte b);
        public extern static bool operator >(SByte a, SByte b);
        public extern static bool operator <(SByte a, SByte b);
        public extern static SByte operator ~(SByte a);
        public extern static SByte operator <<(SByte a, int b);
        public extern static SByte operator >>(SByte a, int b);
        public extern static bool operator ==(SByte a, SByte b);
        public extern static bool operator !=(SByte a, SByte b);
        public extern static SByte operator ++(SByte a);
        public extern static SByte operator --(SByte a);

        public static implicit operator Int64(SByte v);
        public static implicit operator Int32(SByte v);
        public static implicit operator Single(SByte v);
        public static implicit operator Double(SByte v);
        public static explicit operator Byte(SByte v);
        public static implicit operator Int16(SByte v);
        public static implicit operator UInt16(SByte v);
        public static implicit operator UInt32(SByte v);
        public static implicit operator UInt64(SByte v);
    }
}
