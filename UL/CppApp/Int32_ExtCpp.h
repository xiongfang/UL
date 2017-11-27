
//--------------操作符重载-------------------------------------------
System::Int32 System::Int32::op_Addition(Int32 a, Int32 b)
{
	return Int32(a._v + b._v);
}
System::Int32 System::Int32::op_Substraction(Int32 a, Int32 b)
{
	return Int32(a._v - b._v);
}
System::Int32 System::Int32::op_Multiply(Int32 a, Int32 b)
{
	return Int32(a._v * b._v);
}
System::Int32 System::Int32::op_Modulus(Int32 a, Int32 b)
{
	return Int32(a._v % b._v);
}
System::Int32 System::Int32::op_BitwiseOr(Int32 a, Int32 b)
{
	return Int32(a._v | b._v);
}
System::Boolean System::Int32::op_GreaterThen(Int32 a, Int32 b)
{
	return (a._v > b._v);
}
System::Boolean System::Int32::op_LessThen(Int32 a, Int32 b)
{
	return (a._v < b._v);
}
System::Int32 System::Int32::op_OnesComplement(Int32 a)
{
	return Int32(~a._v);
}
System::Int32 System::Int32::op_LeftShift(Int32 a, Int32 b)
{
	return Int32(a._v << b._v);
}
System::Int32 System::Int32::op_RightShift(Int32 a, Int32 b)
{
	return Int32(a._v >> b._v);
}
System::Boolean System::Int32::op_Equality(Int32 a, Int32 b)
{
	return (a._v == b._v);
}
System::Boolean System::Int32::op_Inequality(Int32 a, Int32 b)
{
	return (a._v != b._v);
}
System::Int32 System::Int32::op_Increment(Int32 a)
{
	return Int32(a._v +1);
}
System::Int32 System::Int32::op_Decrement(Int32 a)
{
	return Int32(a._v -1);
}
//------------------类型转换-----------------------------------

System::Int64 System::Int32::Int64(Int32 a)
{
	return System::Int64(a._v);
}


System::Int16 System::Int32::Int16(System::Int32  v)
{
	return System::Int16(v._v);
}

System::Single System::Int32::Single(System::Int32  v)
{
	return System::Single(v._v);
}
System::Double System::Int32::Double(System::Int32  v)
{
	return System::Double(v._v);
}
System::Byte System::Int32::Byte(System::Int32  v)
{
	return System::Byte(v._v);
}
System::SByte System::Int32::SByte(System::Int32  v)
{
	return System::SByte(v._v);
}
System::UInt16 System::Int32::UInt16(System::Int32  v)
{
	return System::UInt16(v._v);
}
System::UInt32 System::Int32::UInt32(System::Int32  v)
{
	return System::UInt32(v._v);
}
System::UInt64 System::Int32::UInt64(System::Int32  v)
{
	return System::UInt64(v._v);
}
