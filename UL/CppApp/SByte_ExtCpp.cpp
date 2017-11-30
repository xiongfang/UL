#include "stdafx.h"
#include "System\SByte.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\Int16.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"


//--------------操作符重载-------------------------------------------
System::SByte System::SByte::op_Addition(SByte a, SByte b)
{
	return SByte(a._v + b._v);
}
System::SByte System::SByte::op_Substraction(SByte a, SByte b)
{
	return SByte(a._v - b._v);
}
System::SByte System::SByte::op_Multiply(SByte a, SByte b)
{
	return SByte(a._v * b._v);
}
System::SByte System::SByte::op_Division(SByte a, SByte b)
{
	return SByte(a._v / b._v);
}
System::SByte System::SByte::op_Modulus(SByte a, SByte b)
{
	return SByte(a._v % b._v);
}
System::SByte System::SByte::op_BitwiseAnd(SByte a, SByte b)
{
	return SByte(a._v & b._v);
}
System::SByte System::SByte::op_BitwiseOr(SByte a, SByte b)
{
	return SByte(a._v | b._v);
}
System::Boolean System::SByte::op_GreaterThen(SByte a, SByte b)
{
	return (a._v > b._v);
}
System::Boolean System::SByte::op_LessThen(SByte a, SByte b)
{
	return (a._v < b._v);
}
System::SByte System::SByte::op_OnesComplement(SByte a)
{
	return SByte(~a._v);
}
System::SByte System::SByte::op_LeftShift(SByte a, System::Int32 b)
{
	return SByte(a._v << b._v);
}
System::SByte System::SByte::op_RightShift(SByte a, System::Int32 b)
{
	return SByte(a._v >> b._v);
}
System::Boolean System::SByte::op_Equality(SByte a, SByte b)
{
	return (a._v == b._v);
}
System::Boolean System::SByte::op_Inequality(SByte a, SByte b)
{
	return (a._v != b._v);
}
System::SByte System::SByte::op_Increment(SByte a)
{
	return SByte(a._v + 1);
}
System::SByte System::SByte::op_Decrement(SByte a)
{
	return SByte(a._v - 1);
}
System::SByte System::SByte::op_UnaryPlus(SByte a)
{
	return a;
}
System::SByte System::SByte::op_UnaryNegation(SByte a)
{
	return SByte(-a._v);
}
//------------------类型转换-----------------------------------

System::Int64 System::SByte::Int64(SByte a)
{
	return System::Int64(a._v);
}


System::Int16 System::SByte::Int16(System::SByte  v)
{
	return System::Int16(v._v);
}

System::Single System::SByte::Single(System::SByte  v)
{
	return System::Single(v._v);
}
System::Double System::SByte::Double(System::SByte  v)
{
	return System::Double(v._v);
}
System::Int32 System::SByte::Int32(System::SByte  v)
{
	return System::Int32(v._v);
}
System::Byte System::SByte::Byte(System::SByte  v)
{
	return System::Byte(v._v);
}
System::UInt16 System::SByte::UInt16(System::SByte  v)
{
	return System::UInt16(v._v);
}
System::UInt32 System::SByte::UInt32(System::SByte  v)
{
	return System::UInt32(v._v);
}
System::UInt64 System::SByte::UInt64(System::SByte  v)
{
	return System::UInt64(v._v);
}
