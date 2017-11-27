namespace System
{
    public struct Double
    {
        public const double Epsilon = 4.94065645841247e-324;
        public const double MaxValue = 1.79769313486231e308;
        public const double MinValue = -1.79769313486231e308;

        public extern static Double Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out Double v)
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

        public extern static Double operator +(Double a, Double b);
        public extern static Double operator -(Double a, Double b);
        public extern static Double operator *(Double a, Double b);
        public extern static Double operator /(Double a, Double b);
        public extern static Double operator %(Double a, Double b);
        public extern static Double operator &(Double a, Double b);
        public extern static Double operator |(Double a, Double b);
        public extern static bool operator >(Double a, Double b);
        public extern static bool operator <(Double a, Double b);
        public extern static Double operator ~(Double a);
        public extern static Double operator <<(Double a, int b);
        public extern static Double operator >>(Double a, int b);
        public extern static bool operator ==(Double a, Double b);
        public extern static bool operator !=(Double a, Double b);
        public extern static Double operator ++(Double a);
        public extern static Double operator --(Double a);

        public static explicit operator Int64(Double v);
        public static explicit operator Int32(Double v);
        public static explicit operator Single(Double v);
        public static explicit operator Int16(Double v);
        public static explicit operator Byte(Double v);
        public static explicit operator SByte(Double v);
        public static explicit operator UInt16(Double v);
        public static explicit operator UInt32(Double v);
        public static explicit operator UInt64(Double v);
    }
}
