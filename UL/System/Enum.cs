namespace ul
{
    namespace System
    {
        public class Enum : ValueType
        {
            public extern static Enum operator &(Enum a, Enum b);
            public extern static Enum operator |(Enum a, Enum b);
            public extern static Enum operator ~(Enum a);
            public extern static bool operator ==(Enum a, Enum b);
            public extern static bool operator !=(Enum a, Enum b);
            public extern override Boolean Equals(Object obj);

            public extern static Array GetValues(
                Type enumType
            );
        }
    }
}