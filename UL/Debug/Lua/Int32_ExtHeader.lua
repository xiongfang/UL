function System.Int32:ctor(v)
	if v~= nil then
		self.v = v;
	else
		self.v = 0;
	end
end
function System.Int32.Parse_System_String(value)
end
function System.Int32:ToString()
	return System.String.new(tostring(self.v));
end
function System.Int32.op_Addition(a,b)
	return System.Int32.new(a.v+b.v);
end
function System.Int32.op_Substraction(a,b)
	return System.Int32.new(a.v - b.v)
end
function System.Int32.op_Multiply(a,b)
	return System.Int32.new(a.v * b.v)
end
function System.Int32.op_Division(a,b)
	return System.Int32.new(a.v / b.v)
end
function System.Int32.op_Modulus(a,b)
	return System.Int32.new(a.v % b.v)
end
function System.Int32.op_BitwiseAnd(a,b)

end
function System.Int32.op_BitwiseOr(a,b)
end
function System.Int32.op_GreaterThen(a,b)
end
function System.Int32.op_LessThen(a,b)
end
function System.Int32.op_Equality(a,b)
end
function System.Int32.op_Inequality(a,b)
end
function System.Int32.op_LeftShift(a,b)
end
function System.Int32.op_RightShift(a,b)
end
function System.Int32.op_Increment(a)
	return System.Int32.new(a.v +1);
end
function System.Int32.op_Decrement(a)
	return System.Int32.new(a.v -1);
end
function System.Int32.op_OnesComplement(a)
end
function System.Int32.op_UnaryPlus(a)
end
function System.Int32.op_UnaryNegation(a)
	return System.Int32.new(-a.v);
end
function System.Int32.Int64_System_Int32(v)
	return System.Int64.new(v.v);
end
function System.Int32.Int16_System_Int32(v)
end
function System.Int32.Single_System_Int32(v)
end
function System.Int32.Double_System_Int32(v)
end
function System.Int32.Byte_System_Int32(v)
end
function System.Int32.SByte_System_Int32(v)
end
function System.Int32.UInt16_System_Int32(v)
end
function System.Int32.UInt32_System_Int32(v)
end
function System.Int32.UInt64_System_Int32(v)
	return System.UInt64.new(v.v);
end