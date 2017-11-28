#include "stdafx.h"
#include "UInt16.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "Int16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"

//--------------操作符重载-------------------------------------------
System::UInt16 System::UInt16::op_Addition(UInt16 a, UInt16 b)
{
	return UInt16(a._v + b._v);
}
System::UInt16 System::UInt16::op_Substraction(UInt16 a, UInt16 b)
{
	return UInt16(a._v - b._v);
}
System::UInt16 System::UInt16::op_Multiply(UInt16 a, UInt16 b)
{
	return UInt16(a._v * b._v);
}
System::UInt16 System::UInt16::op_Division(UInt16 a, UInt16 b)
{
	return UInt16(a._v / b._v);
}
System::UInt16 System::UInt16::op_Modulus(UInt16 a, UInt16 b)
{
	return UInt16(a._v % b._v);
}
System::UInt16 System::UInt16::op_BitwiseAnd(UInt16 a, UInt16 b)
{
	return UInt16(a._v & b._v);
}
System::UInt16 System::UInt16::op_BitwiseOr(UInt16 a, UInt16 b)
{
	return UInt16(a._v | b._v);
}
System::Boolean System::UInt16::op_GreaterThen(UInt16 a, UInt16 b)
{
	return (a._v > b._v);
}
System::Boolean System::UInt16::op_LessThen(UInt16 a, UInt16 b)
{
	return (a._v < b._v);
}
System::UInt16 System::UInt16::op_OnesComplement(UInt16 a)
{
	return UInt16(~a._v);
}
System::UInt16 System::UInt16::op_LeftShift(UInt16 a, System::Int32 b)
{
	return UInt16(a._v << b._v);
}
System::UInt16 System::UInt16::op_RightShift(UInt16 a, System::Int32 b)
{
	return UInt16(a._v >> b._v);
}
System::Boolean System::UInt16::op_Equality(UInt16 a, UInt16 b)
{
	return (a._v == b._v);
}
System::Boolean System::UInt16::op_Inequality(UInt16 a, UInt16 b)
{
	return (a._v != b._v);
}
System::UInt16 System::UInt16::op_Increment(UInt16 a)
{
	return UInt16(a._v + 1);
}
System::UInt16 System::UInt16::op_Decrement(UInt16 a)
{
	return UInt16(a._v - 1);
}
System::UInt16 System::UInt16::op_UnaryPlus(UInt16 a)
{
	return a;
}
//System::UInt16 System::UInt16::op_UnaryNegation(UInt16 a)
//{
//	return UInt16(-a._v);
//}
//------------------类型转换-----------------------------------

System::Int64 System::UInt16::Int64(UInt16 a)
{
	return System::Int64(a._v);
}


System::Int16 System::UInt16::Int16(System::UInt16  v)
{
	return System::Int16(v._v);
}

System::Single System::UInt16::Single(System::UInt16  v)
{
	return System::Single(v._v);
}
System::Double System::UInt16::Double(System::UInt16  v)
{
	return System::Double(v._v);
}
System::Int32 System::UInt16::Int32(System::UInt16  v)
{
	return System::Int32(v._v);
}
System::SByte System::UInt16::SByte(System::UInt16  v)
{
	return System::SByte(v._v);
}
System::Byte System::UInt16::Byte(System::UInt16  v)
{
	return System::Byte(v._v);
}
System::UInt32 System::UInt16::UInt32(System::UInt16  v)
{
	return System::UInt32(v._v);
}
System::UInt64 System::UInt16::UInt64(System::UInt16  v)
{
	return System::UInt64(v._v);
}
