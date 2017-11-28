#include "stdafx.h"
#include "UInt32.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt64.h"
#include "Int16.h"
#include "Exception.h"


//--------------操作符重载-------------------------------------------
System::UInt32 System::UInt32::op_Addition(UInt32 a, UInt32 b)
{
	return UInt32(a._v + b._v);
}
System::UInt32 System::UInt32::op_Substraction(UInt32 a, UInt32 b)
{
	return UInt32(a._v - b._v);
}
System::UInt32 System::UInt32::op_Multiply(UInt32 a, UInt32 b)
{
	return UInt32(a._v * b._v);
}
System::UInt32 System::UInt32::op_Division(UInt32 a, UInt32 b)
{
	return UInt32(a._v / b._v);
}
System::UInt32 System::UInt32::op_Modulus(UInt32 a, UInt32 b)
{
	return UInt32(a._v % b._v);
}
System::UInt32 System::UInt32::op_BitwiseAnd(UInt32 a, UInt32 b)
{
	return UInt32(a._v & b._v);
}
System::UInt32 System::UInt32::op_BitwiseOr(UInt32 a, UInt32 b)
{
	return UInt32(a._v | b._v);
}
System::Boolean System::UInt32::op_GreaterThen(UInt32 a, UInt32 b)
{
	return (a._v > b._v);
}
System::Boolean System::UInt32::op_LessThen(UInt32 a, UInt32 b)
{
	return (a._v < b._v);
}
System::UInt32 System::UInt32::op_OnesComplement(UInt32 a)
{
	return UInt32(~a._v);
}
System::UInt32 System::UInt32::op_LeftShift(UInt32 a, System::Int32 b)
{
	return UInt32(a._v << b._v);
}
System::UInt32 System::UInt32::op_RightShift(UInt32 a, System::Int32 b)
{
	return UInt32(a._v >> b._v);
}
System::Boolean System::UInt32::op_Equality(UInt32 a, UInt32 b)
{
	return (a._v == b._v);
}
System::Boolean System::UInt32::op_Inequality(UInt32 a, UInt32 b)
{
	return (a._v != b._v);
}
System::UInt32 System::UInt32::op_Increment(UInt32 a)
{
	return UInt32(a._v + 1);
}
System::UInt32 System::UInt32::op_Decrement(UInt32 a)
{
	return UInt32(a._v - 1);
}
System::UInt32 System::UInt32::op_UnaryPlus(UInt32 a)
{
	return a;
}
//System::UInt32 System::UInt32::op_UnaryNegation(UInt32 a)
//{
//	return UInt32(-a._v);
//}
//------------------类型转换-----------------------------------

System::Int64 System::UInt32::Int64(UInt32 a)
{
	return System::Int64(a._v);
}


System::Int16 System::UInt32::Int16(System::UInt32  v)
{
	return System::Int16(v._v);
}

System::Single System::UInt32::Single(System::UInt32  v)
{
	return System::Single(v._v);
}
System::Double System::UInt32::Double(System::UInt32  v)
{
	return System::Double(v._v);
}
System::Int32 System::UInt32::Int32(System::UInt32  v)
{
	return System::Int32(v._v);
}
System::SByte System::UInt32::SByte(System::UInt32  v)
{
	return System::SByte(v._v);
}
System::UInt16 System::UInt32::UInt16(System::UInt32  v)
{
	return System::UInt16(v._v);
}
System::Byte System::UInt32::Byte(System::UInt32  v)
{
	return System::Byte(v._v);
}
System::UInt64 System::UInt32::UInt64(System::UInt32  v)
{
	return System::UInt64(v._v);
}
