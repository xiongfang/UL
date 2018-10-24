#include "stdafx.h"
#include "System\Int64.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int16.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"


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
	return System::Byte((System::Byte::ValueType)a._v);
}


System::Int16 System::Int64::Int16(System::Int64  v)
{
	return System::Int16((System::Int16::ValueType)v._v);
}

System::Single System::Int64::Single(System::Int64  v)
{
	return System::Single((System::Single::ValueType)v._v);
}
System::Double System::Int64::Double(System::Int64  v)
{
	return System::Double((System::Double::ValueType)v._v);
}
System::Int32 System::Int64::Int32(System::Int64  v)
{
	return System::Int32((System::Int32::ValueType)v._v);
}
System::SByte System::Int64::SByte(System::Int64  v)
{
	return System::SByte((System::SByte::ValueType)v._v);
}
System::UInt16 System::Int64::UInt16(System::Int64  v)
{
	return System::UInt16((System::UInt16::ValueType)v._v);
}
System::UInt32 System::Int64::UInt32(System::Int64  v)
{
	return System::UInt32((System::UInt32::ValueType)v._v);
}
System::UInt64 System::Int64::UInt64(System::Int64  v)
{
	return System::UInt64((System::UInt64::ValueType)v._v);
}
