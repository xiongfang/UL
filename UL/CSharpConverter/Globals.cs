using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace SDILReader
{
        //public enum AssemblyType
        //{
        //    None,
        //    Console,
        //    Application,
        //    Library
        //}

        //public enum BinaryOperator
        //{
        //    Add,
        //    Subtract,
        //    Multiply,
        //    Divide,
        //    Modulus,
        //    ShiftLeft,
        //    ShiftRight,
        //    IdentityEquality,
        //    IdentityInequality,
        //    ValueEquality,
        //    ValueInequality,
        //    BitwiseOr,
        //    BitwiseAnd,
        //    BitwiseExclusiveOr,
        //    BooleanOr,
        //    BooleanAnd,
        //    LessThan,
        //    LessThanOrEqual,
        //    GreaterThan,
        //    GreaterThanOrEqual
        //}

        //public enum ExceptionHandlerType
        //{
        //    Finally,
        //    Catch,
        //    Filter,
        //    Fault
        //}

        //public enum FieldVisibility
        //{
        //    Private,
        //    Public,
        //    Internal,
        //    Protected,
        //}

        //public enum MethodVisibility
        //{
        //    Private,
        //    Public,
        //    Internal,
        //    External,
        //    Protected,
        //}
        //public enum MethodModifier
        //{
        //    Static,
        //    Override,
        //    Abstract,
        //    Virtual,
        //    Final,
        //    None,
        //}


        //public enum ResourceVisibility
        //{
        //    Public,
        //    Private
        //}

        //public enum TypeVisibility
        //{
        //    vPublic,
        //    vProtected,
        //    vInternal,
        //    vProtectedInternal,
        //    vPrivate
        //}

        //public enum ClassModifiers
        //{
        //    mAbstract,
        //    mSealed,
        //    mStatic,
        //    mNone,
        //}

        //public enum UnaryOperator
        //{
        //    Negate,
        //    BooleanNot,
        //    BitwiseNot,
        //    PreIncrement,
        //    PreDecrement,
        //    PostIncrement,
        //    PostDecrement
        //}



    public static class Globals
    {
        public static Dictionary<int, object> Cache = new Dictionary<int, object>();

        public static OpCode[] multiByteOpCodes;
        public static OpCode[] singleByteOpCodes;
        public static Module[] modules = null;

        public static void LoadOpCodes()
        {
            singleByteOpCodes = new OpCode[0x100];
            multiByteOpCodes = new OpCode[0x100];
            FieldInfo[] infoArray1 = typeof(OpCodes).GetFields();
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                FieldInfo info1 = infoArray1[num1];
                if (info1.FieldType == typeof(OpCode))
                {
                    OpCode code1 = (OpCode)info1.GetValue(null);
                    ushort num2 = (ushort)code1.Value;
                    if (num2 < 0x100)
                    {
                        singleByteOpCodes[(int)num2] = code1;
                    }
                    else
                    {
                        if ((num2 & 0xff00) != 0xfe00)
                        {
                            throw new Exception("Invalid OpCode.");
                        }
                        multiByteOpCodes[num2 & 0xff] = code1;
                    }
                }
            }
        }


        /// <summary>
        /// Retrieve the friendly name of a type
        /// </summary>
        /// <param name="typeName">
        /// The complete name to the type
        /// </param>
        /// <returns>
        /// The simplified name of the type (i.e. "int" instead f System.Int32)
        /// </returns>
        public static string ProcessSpecialTypes(string typeName)
        {
            string result = typeName;
            switch (typeName)
            {
                case "System.string":
                case "System.String":
                case "String":
                    result = "string"; break;
                case "System.Int32":
                case "Int":
                case "Int32":
                    result = "int"; break;
            }
            return result;
        }

        //public static string SpaceGenerator(int count)
        //{
        //    string result = "";
        //    for (int i = 0; i < count; i++) result += " ";
        //    return result;
        //}

        //public static string AddBeginSpaces(string source, int count)
        //{
        //    string[] elems = source.Split('\n');
        //    string result = "";
        //    for (int i = 0; i < elems.Length; i++)
        //    {
        //        result += SpaceGenerator(count) + elems[i] + "\n";
        //    }
        //    return result;
        //}
    }
}
