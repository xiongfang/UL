#include "stdafx.h"
#include "Int64.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int16.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"


//--------------操作符重载-------------------------------------------
System::Int64 System::Int64::op_Addition(Int64 a, Int64 b)
{
	return Int64(a._v + b._v);
}
System::Int64 System::Int64::op_Substraction(Int64 a, Int64 b)
{
	return Int64(a._v - b._v);
}
System::Int64 System::Int64::op_Multiply(Int64 a, Int64 b)
{
	return Int64(a._v * b._v);
}
System::Int64 System::Int64::op_Division(Int64 a, Int64 b)
{
	return Int64(a._v / b._v);
}
System::Int64 System::Int64::op_Modulus(Int64 a, Int64 b)
{
	return Int64(a._v % b._v);
}
System::Int64 System::Int64::op_BitwiseAnd(Int64 a, Int64 b)
{
	return Int64(a._v & b._v);
}
System::Int64 System::Int64::op_BitwiseOr(Int64 a, Int64 b)
{
	return Int64(a._v | b._v);
}
System::Boolean System::Int64::op_GreaterThen(Int64 a, Int64 b)
{
	return (a._v > b._v);
}
System::Boolean System::Int64::op_LessThen(Int64 a, Int64 b)
{
	return (a._v < b._v);
}
System::Int64 System::Int64::op_OnesComplement(Int64 a)
{
	return Int64(~a._v);
}
System::Int64 System::Int64::op_LeftShift(Int64 a, System::Int32 b)
{
	return Int64(a._v << b._v);
}
System::Int64 System::Int64::op_RightShift(Int64 a, System::Int32 b)
{
	return Int64(a._v >> b._v);
}
System::Boolean System::Int64::op_Equality(Int64 a, Int64 b)
{
	return (a._v == b._v);
}
System::Boolean System::Int64::op_Inequality(Int64 a, Int64 b)
{
	return (a._v != b._v);
}
System::Int64 System::Int64::op_Increment(Int64 a)
{
	return Int64(a._v + 1);
}
System::Int64 System::Int64::op_Decrement(Int64 a)
{
	return Int64(a._v - 1);
}
System::Int64 System::Int64::op_UnaryPlus(Int64 a)
{
	return a;
}
System::Int64 System::Int64::op_UnaryNegation(Int64 a)
{
	return Int64(-a._v);
}
//------------------类型转换-----------------------------------

System::Byte System::Int64::Byte(Int64 a)
{
	return System::Byte(a._v);
}


System::Int16 System::Int64::Int16(System::Int64  v)
{
	return System::Int16(v._v);
}

System::Single System::Int64::Single(System::Int64  v)
{
	return System::Single(v._v);
}
System::Double System::Int64::Double(System::Int64  v)
{
	return System::Double(v._v);
}
System::Int32 System::Int64::Int32(System::Int64  v)
{
	return System::Int32(v._v);
}
System::SByte System::Int64::SByte(System::Int64  v)
{
	return System::SByte(v._v);
}
System::UInt16 System::Int64::UInt16(System::Int64  v)
{
	return System::UInt16(v._v);
}
System::UInt32 System::Int64::UInt32(System::Int64  v)
{
	return System::UInt32(v._v);
}
System::UInt64 System::Int64::UInt64(System::Int64  v)
{
	return System::UInt64(v._v);
}
