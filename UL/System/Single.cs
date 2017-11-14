namespace System
{
    public struct Single
    {
        public const float Epsilon = 1.4e-45f;
        public const float MaxValue = 3.40282347e38f;
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


        public virtual Boolean op_Equals(Single b);
        public virtual Boolean op_Small(Single b);
        public virtual Single op_Assign(Single b);
        public virtual Single op_PlusPlus(Single b);
    }
}
