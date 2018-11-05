namespace ul
{
    namespace System
    {
        public struct Single
        {
            public const float Epsilon = 1.4e-45f;
            public const float MaxValue = 3.40282346e38f; //主意这里改变了值，原始值是 3.40282347e38f，因为C++常量太大不能编译;
            public const float MinValue = -3.402823e38f;


            public extern static Single Parse(string value);

            public extern override string ToString();

            public static bool TryParse(string value, out Single v)
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

            public extern static Single operator +(Single a, Single b);
            public extern static Single operator -(Single a, Single b);
            public extern static Single operator *(Single a, Single b);
            public extern static Single operator /(Single a, Single b);
            // public extern static Single operator %(Single a, Single b);
            //public extern static Single operator &(Single a, Single b);
            //public extern static Single operator |(Single a, Single b);
            public extern static bool operator >(Single a, Single b);
            public extern static bool operator <(Single a, Single b);
            public extern static Single operator ~(Single a);
            //public extern static Single operator <<(Single a, int b);
            //public extern static Single operator >>(Single a, int b);
            public extern static bool operator ==(Single a, Single b);
            public extern static bool operator !=(Single a, Single b);
            public extern static Single operator ++(Single a);
            public extern static Single operator --(Single a);
            public extern static Single operator +(Single a);
            public extern static Single operator -(Single a);

            public extern static implicit operator Int64(Single v);
            public extern static explicit operator Int32(Single v);
            public extern static explicit operator Int16(Single v);
            public extern static implicit operator Double(Single v);
            public extern static explicit operator Byte(Single v);
            public extern static explicit operator SByte(Single v);
            public extern static explicit operator UInt16(Single v);
            public extern static explicit operator UInt32(Single v);
            public extern static explicit operator UInt64(Single v);
        }
    }
}