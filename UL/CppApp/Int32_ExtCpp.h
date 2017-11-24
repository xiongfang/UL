

System::Int32 System::Int32::op_Addition(Int32 a, Int32 b)
{
	return Int32(a._v + b._v);
}

System::Int64 System::Int32::Int64(Int32 a)
{
	return System::Int64(a._v);
}