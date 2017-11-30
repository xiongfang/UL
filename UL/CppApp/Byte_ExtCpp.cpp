#include "stdafx.h"
#include "System\Byte.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Int16.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"


//--------------操作符重载-------------------------------------------
System::Byte System::Byte::op_Addition(Byte a, Byte b)
{
	return Byte(a._v + b._v);
}
System::Byte System::Byte::op_Substraction(Byte a, Byte b)
{
	return Byte(a._v - b._v);
}
System::Byte System::Byte::op_Multiply(Byte a, Byte b)
{
	return Byte(a._v * b._v);
}
System::Byte System::Byte::op_Division(Byte a, Byte b)
{
	return Byte(a._v / b._v);
}
System::Byte System::Byte::op_Modulus(Byte a, Byte b)
{
	return Byte(a._v % b._v);
}
System::Byte System::Byte::op_BitwiseAnd(Byte a, Byte b)
{
	return Byte(a._v & b._v);
}
System::Byte System::Byte::op_BitwiseOr(Byte a, Byte b)
{
	return Byte(a._v | b._v);
}
System::Boolean System::Byte::op_GreaterThen(Byte a, Byte b)
{
	return (a._v > b._v);
}
System::Boolean System::Byte::op_LessThen(Byte a, Byte b)
{
	return (a._v < b._v);
}
System::Byte System::Byte::op_OnesComplement(Byte a)
{
	return Byte(~a._v);
}
System::Byte System::Byte::op_LeftShift(Byte a, System::Int32 b)
{
	return Byte(a._v << b._v);
}
System::Byte System::Byte::op_RightShift(Byte a, System::Int32 b)
{
	return Byte(a._v >> b._v);
}
System::Boolean System::Byte::op_Equality(Byte a, Byte b)
{
	return (a._v == b._v);
}
System::Boolean System::Byte::op_Inequality(Byte a, Byte b)
{
	return (a._v != b._v);
}
System::Byte System::Byte::op_Increment(Byte a)
{
	return Byte(a._v + 1);
}
System::Byte System::Byte::op_Decrement(Byte a)
{
	return Byte(a._v - 1);
}
System::Byte System::Byte::op_UnaryPlus(Byte a)
{
	return a;
}
System::Byte System::Byte::op_UnaryNegation(Byte a)
{
	return Byte(-a._v);
}
//------------------类型转换-----------------------------------

System::Int64 System::Byte::Int64(Byte a)
{
	return System::Int64(a._v);
}


System::Int16 System::Byte::Int16(System::Byte  v)
{
	return System::Int16(v._v);
}

System::Single System::Byte::Single(System::Byte  v)
{
	return System::Single((System::Single::ValueType)v._v);
}
System::Double System::Byte::Double(System::Byte  v)
{
	return System::Double(v._v);
}
System::Int32 System::Byte::Int32(System::Byte  v)
{
	return System::Int32(v._v);
}
System::SByte System::Byte::SByte(System::Byte  v)
{
	return System::SByte(v._v);
}
System::UInt16 System::Byte::UInt16(System::Byte  v)
{
	return System::UInt16(v._v);
}
System::UInt32 System::Byte::UInt32(System::Byte  v)
{
	return System::UInt32(v._v);
}
System::UInt64 System::Byte::UInt64(System::Byte  v)
{
	return System::UInt64(v._v);
}
