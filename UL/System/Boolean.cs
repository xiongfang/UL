using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct Boolean
    {
        public static readonly string FalseString = "false";
        public static readonly string TrueString = "true";

        public static bool Parse(string value)
        {
            if(value == null)
            {
                throw new ArgumentNullException();
            }
            if (value == TrueString)
                return true;
            else if(value == FalseString)
                return false;
            else
            {
                throw new FormatException(value);
            }
        } 

        public extern override string ToString();

        public static bool TryParse(string value,out bool v)
        {
            try
            {
                v =  Parse(value);
                return true;
            }
            catch(Exception e)
            {
                v = false;
                return false;
            }
        }
    }
}
