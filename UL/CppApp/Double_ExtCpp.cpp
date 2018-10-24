#include "stdafx.h"
#include "System\Double.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int64.h"
#include "System\Int32.h"
#include "System\Single.h"
#include "System\Int16.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"

//--------------操作符重载-------------------------------------------
System::Double System::Double::op_Addition(Double a, Double b)
{
	return Double(a._v + b._v);
}
System::Double System::Double::op_Substraction(Double a, Double b)
{
	return Double(a._v - b._v);
}
System::Double System::Double::op_Multiply(Double a, Double b)
{
	return Double(a._v * b._v);
}
System::Double System::Double::op_Division(Double a, Double b)
{
	return Double(a._v / b._v);
}
//System::Double System::Double::op_Modulus(Double a, Double b)
//{
//	return Double(a._v % b._v);
//}
//System::Double System::Double::op_BitwiseOr(Double a, Double b)
//{
//	return Double(a._v | b._v);
//}
System::Boolean System::Double::op_GreaterThen(Double a, Double b)
{
	return (a._v > b._v);
}
System::Boolean System::Double::op_LessThen(Double a, Double b)
{
	return (a._v < b._v);
}
//System::Double System::Double::op_OnesComplement(Double a)
//{
//	return Double(~a._v);
//}
//System::Double System::Double::op_LeftShift(Double a, System::Int32 b)
//{
//	return Double(a._v << b._v);
//}
//System::Double System::Double::op_RightShift(Double a, System::Int32 b)
//{
//	return Double(a._v >> b._v);
//}
System::Boolean System::Double::op_Equality(Double a, Double b)
{
	return (a._v == b._v);
}
System::Boolean System::Double::op_Inequality(Double a, Double b)
{
	return (a._v != b._v);
}
System::Double System::Double::op_Increment(Double a)
{
	return Double(a._v + 1);
}
System::Double System::Double::op_Decrement(Double a)
{
	return Double(a._v - 1);
}
System::Double System::Double::op_UnaryPlus(Double a)
{
	return a;
}
System::Double System::Double::op_UnaryNegation(Double a)
{
	return Double(-a._v);
}
//------------------类型转换-----------------------------------

System::Int64 System::Double::Int64(Double a)
{
	return System::Int64((System::Int64::ValueType)a._v);
}


System::Int16 System::Double::Int16(System::Double  v)
{
	return System::Int16((System::Int16::ValueType)v._v);
}

System::Single System::Double::Single(System::Double  v)
{
	return System::Single((System::Single::ValueType)v._v);
}
System::Byte System::Double::Byte(System::Double  v)
{
	return System::Byte((System::Byte::ValueType)v._v);
}
System::Int32 System::Double::Int32(System::Double  v)
{
	return System::Int32((System::Int32::ValueType)v._v);
}
System::SByte System::Double::SByte(System::Double  v)
{
	return System::SByte((System::SByte::ValueType)v._v);
}
System::UInt16 System::Double::UInt16(System::Double  v)
{
	return System::UInt16((System::UInt16::ValueType)v._v);
}
System::UInt32 System::Double::UInt32(System::Double  v)
{
	return System::UInt32((System::UInt32::ValueType)v._v);
}
System::UInt64 System::Double::UInt64(System::Double  v)
{
	return System::UInt64((System::UInt64::ValueType)v._v);
}

