
namespace System
{
    public interface IComparable
    {
        int CompareTo(object obj);
    }

    public struct Int8
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct Int16
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct Int64
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct Int32
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483647;

        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct UInt16
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct UInt32
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct UInt64
    {
        public Boolean op_Equals(Int32 b);
        public Boolean op_Small(Int32 b);
        public Int32 op_Assign(Int32 b);
        public Int32 op_PlusPlus(Int32 b);
    }
    public struct Boolean
    {
    }

    public class String
    {

    }

    public class Object
    {
        public virtual string ToString();
        public virtual bool Equals(object v);
    }

    public class Array<T>
    {
        public Array(int len) { }
    }

    public class Console
    {
        public static void WriteLine(string v);
    }

    public struct IntPtr
    {

    }

    public struct Single
    {

    }

    public struct Double
    {
    }

    public class Attribute
    {

    }
}
