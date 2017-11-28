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
System::UInt64 System::UInt64::op_Addition(UInt64 a, UInt64 b)
{
	return UInt64(a._v + b._v);
}
System::UInt64 System::UInt64::op_Substraction(UInt64 a, UInt64 b)
{
	return UInt64(a._v - b._v);
}
System::UInt64 System::UInt64::op_Multiply(UInt64 a, UInt64 b)
{
	return UInt64(a._v * b._v);
}
System::UInt64 System::UInt64::op_Division(UInt64 a, UInt64 b)
{
	return UInt64(a._v / b._v);
}
System::UInt64 System::UInt64::op_Modulus(UInt64 a, UInt64 b)
{
	return UInt64(a._v % b._v);
}
System::UInt64 System::UInt64::op_BitwiseAnd(UInt64 a, UInt64 b)
{
	return UInt64(a._v & b._v);
}
System::UInt64 System::UInt64::op_BitwiseOr(UInt64 a, UInt64 b)
{
	return UInt64(a._v | b._v);
}
System::Boolean System::UInt64::op_GreaterThen(UInt64 a, UInt64 b)
{
	return (a._v > b._v);
}
System::Boolean System::UInt64::op_LessThen(UInt64 a, UInt64 b)
{
	return (a._v < b._v);
}
System::UInt64 System::UInt64::op_OnesComplement(UInt64 a)
{
	return UInt64(~a._v);
}
System::UInt64 System::UInt64::op_LeftShift(UInt64 a, System::Int32 b)
{
	return UInt64(a._v << b._v);
}
System::UInt64 System::UInt64::op_RightShift(UInt64 a, System::Int32 b)
{
	return UInt64(a._v >> b._v);
}
System::Boolean System::UInt64::op_Equality(UInt64 a, UInt64 b)
{
	return (a._v == b._v);
}
System::Boolean System::UInt64::op_Inequality(UInt64 a, UInt64 b)
{
	return (a._v != b._v);
}
System::UInt64 System::UInt64::op_Increment(UInt64 a)
{
	return UInt64(a._v + 1);
}
System::UInt64 System::UInt64::op_Decrement(UInt64 a)
{
	return UInt64(a._v - 1);
}
System::UInt64 System::UInt64::op_UnaryPlus(UInt64 a)
{
	return a;
}
//System::UInt64 System::UInt64::op_UnaryNegation(UInt64 a)
//{
//	return UInt64(-a._v);
//}
//------------------类型转换-----------------------------------

System::Int64 System::UInt64::Int64(UInt64 a)
{
	return System::Int64(a._v);
}


System::Int16 System::UInt64::Int16(System::UInt64  v)
{
	return System::Int16((System::Int16::ValueType)v._v);
}

System::Single System::UInt64::Single(System::UInt64  v)
{
	return System::Single((System::Single::ValueType)v._v);
}
System::Double System::UInt64::Double(System::UInt64  v)
{
	return System::Double((System::Double::ValueType)v._v);
}
System::Int32 System::UInt64::Int32(System::UInt64  v)
{
	return System::Int32((System::Int32::ValueType)v._v);
}
System::SByte System::UInt64::SByte(System::UInt64  v)
{
	return System::SByte((System::SByte::ValueType)v._v);
}
System::UInt16 System::UInt64::UInt16(System::UInt64  v)
{
	return System::UInt16((System::UInt16::ValueType)v._v);
}
System::UInt32 System::UInt64::UInt32(System::UInt64  v)
{
	return System::UInt32((System::UInt32::ValueType)v._v);
}
System::Byte System::UInt64::Byte(System::UInt64  v)
{
	return System::Byte((System::Byte::ValueType)v._v);
}
