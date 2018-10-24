#include "stdafx.h"
#include "System\Single.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int64.h"
#include "System\Int32.h"
#include "System\Int16.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"

//--------------操作符重载-------------------------------------------
System::Single System::Single::op_Addition(Single a, Single b)
{
	return Single(a._v + b._v);
}
System::Single System::Single::op_Substraction(Single a, Single b)
{
	return Single(a._v - b._v);
}
System::Single System::Single::op_Multiply(Single a, Single b)
{
	return Single(a._v * b._v);
}
System::Single System::Single::op_Division(Single a, Single b)
{
	return Single(a._v / b._v);
}
//System::Single System::Single::op_Modulus(Single a, Single b)
//{
//	return Single(a._v % b._v);
//}
//System::Single System::Single::op_BitwiseOr(Single a, Single b)
//{
//	return Single(a._v | b._v);
//}
System::Boolean System::Single::op_GreaterThen(Single a, Single b)
{
	return (a._v > b._v);
}
System::Boolean System::Single::op_LessThen(Single a, Single b)
{
	return (a._v < b._v);
}
//System::Single System::Single::op_OnesComplement(Single a)
//{
//	return Single(~a._v);
//}
//System::Single System::Single::op_LeftShift(Single a, System::Int32 b)
//{
//	return Single(a._v << b._v);
//}
//System::Single System::Single::op_RightShift(Single a, System::Int32 b)
//{
//	return Single(a._v >> b._v);
//}
System::Boolean System::Single::op_Equality(Single a, Single b)
{
	return (a._v == b._v);
}
System::Boolean System::Single::op_Inequality(Single a, Single b)
{
	return (a._v != b._v);
}
System::Single System::Single::op_Increment(Single a)
{
	return Single(a._v + 1);
}
System::Single System::Single::op_Decrement(Single a)
{
	return Single(a._v - 1);
}
System::Single System::Single::op_UnaryPlus(Single a)
{
	return a;
}
System::Single System::Single::op_UnaryNegation(Single a)
{
	return Single(-a._v);
}
//------------------类型转换-----------------------------------

System::Int64 System::Single::Int64(Single a)
{
	return System::Int64((System::Int64::ValueType)a._v);
}


System::Int16 System::Single::Int16(System::Single  v)
{
	return System::Int16((System::Int16::ValueType)v._v);
}

System::Double System::Single::Double(System::Single  v)
{
	return System::Double(v._v);
}
System::Byte System::Single::Byte(System::Single  v)
{
	return System::Byte((System::Byte::ValueType)v._v);
}
System::Int32 System::Single::Int32(System::Single  v)
{
	return System::Int32((System::Int32::ValueType)v._v);
}
System::SByte System::Single::SByte(System::Single  v)
{
	return System::SByte((System::SByte::ValueType)v._v);
}
System::UInt16 System::Single::UInt16(System::Single  v)
{
	return System::UInt16((System::UInt16::ValueType)v._v);
}
System::UInt32 System::Single::UInt32(System::Single  v)
{
	return System::UInt32((System::UInt32::ValueType)v._v);
}
System::UInt64 System::Single::UInt64(System::Single  v)
{
	return System::UInt64((System::UInt64::ValueType)v._v);
}

