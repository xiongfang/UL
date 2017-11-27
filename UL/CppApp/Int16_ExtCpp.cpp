#include "stdafx.h"
#include "Int16.h"
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
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"

//--------------操作符重载-------------------------------------------
System::Int16 System::Int16::op_Addition(Int16 a, Int16 b)
{
	return Int16(a._v + b._v);
}

System::Int16 System::Int16::op_Substraction(Int16 a, Int16 b)
{
	return Int16(a._v - b._v);
}
System::Int16 System::Int16::op_Multiply(Int16 a, Int16 b)
{
	return Int16(a._v * b._v);
}
System::Int16 System::Int16::op_Division(Int16 a, Int16 b)
{
	return Int16(a._v / b._v);
}
System::Int16 System::Int16::op_Modulus(Int16 a, Int16 b)
{
	return Int16(a._v % b._v);
}
System::Int16 System::Int16::op_BitwiseAnd(Int16 a, Int16 b)
{
	return Int16(a._v & b._v);
}
System::Int16 System::Int16::op_BitwiseOr(Int16 a, Int16 b)
{
	return Int16(a._v | b._v);
}
System::Boolean System::Int16::op_GreaterThen(Int16 a, Int16 b)
{
	return (a._v > b._v);
}
System::Boolean System::Int16::op_LessThen(Int16 a, Int16 b)
{
	return (a._v < b._v);
}
System::Int16 System::Int16::op_OnesComplement(Int16 a)
{
	return Int16(~a._v);
}
System::Int16 System::Int16::op_LeftShift(Int16 a, System::Int32 b)
{
	return Int16(a._v << b._v);
}
System::Int16 System::Int16::op_RightShift(Int16 a, System::Int32 b)
{
	return Int16(a._v >> b._v);
}
System::Boolean System::Int16::op_Equality(Int16 a, Int16 b)
{
	return (a._v == b._v);
}
System::Boolean System::Int16::op_Inequality(Int16 a, Int16 b)
{
	return (a._v != b._v);
}

//------------------类型转换-----------------------------------
System::Int64 System::Int16::Int64(System::Int16  v)
{
	return System::Int64(v._v);
}


System::Int32 System::Int16::Int32(System::Int16  v)
{
	return System::Int32(v._v);
}

System::Single System::Int16::Single(System::Int16  v)
{
	return System::Single(v._v);
}

System::Double System::Int16::Double(System::Int16  v)
{
	return System::Double(v._v);
}

System::Byte System::Int16::Byte(System::Int16  v)
{
	return System::Byte(v._v);
}

System::SByte System::Int16::SByte(System::Int16  v)
{
	return System::SByte((signed char)v._v);
}

System::UInt16 System::Int16::UInt16(System::Int16  v)
{
	return System::UInt16(v._v);
}

System::UInt32 System::Int16::UInt32(System::Int16  v)
{
	return System::UInt32(v._v);
}

System::UInt64 System::Int16::UInt64(System::Int16  v)
{
	return System::UInt64(v._v);
}
