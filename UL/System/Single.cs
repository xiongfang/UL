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
    }
}
