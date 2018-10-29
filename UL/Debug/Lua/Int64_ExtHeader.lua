
function System.Int64:ctor(v)
	if v~= nil then
		self.v = v;
	else
		self.v = 0;
	end
end

function System.Int64.Parse_System_String(value)
end
function System.Int64:ToString()
	return System.String.new(tostring(self.v));
end
function System.Int64.op_Addition(a,b)
	return System.Int64.new(a.v+b.v);
end
function System.Int64.op_Substraction(a,b)
	return System.Int64.new(a.v - b.v)
end
function System.Int64.op_Multiply(a,b)
	return System.Int64.new(a.v * b.v)
end
function System.Int64.op_Division(a,b)
	return System.Int64.new(a.v / b.v)
end
function System.Int64.op_Modulus(a,b)
	return System.Int64.new(a.v % b.v)
end
function System.Int64.op_BitwiseAnd(a,b)
end
function System.Int64.op_BitwiseOr(a,b)
end
function System.Int64.op_GreaterThen(a,b)
end
function System.Int64.op_LessThen(a,b)
end
function System.Int64.op_OnesComplement(a)
end
function System.Int64.op_LeftShift(a,b)
end
function System.Int64.op_RightShift(a,b)
end
function System.Int64.op_Equality(a,b)
end
function System.Int64.op_Inequality(a,b)
end
function System.Int64.op_Increment(a)
end
function System.Int64.op_Decrement(a)
end
function System.Int64.op_UnaryPlus(a)
end
function System.Int64.op_UnaryNegation(a)
end
function System.Int64.Int16_System_Int64(v)
end
function System.Int64.Int32_System_Int64(v)
end
function System.Int64.Single_System_Int64(v)
end
function System.Int64.Double_System_Int64(v)
end
function System.Int64.Byte_System_Int64(v)
end
function System.Int64.SByte_System_Int64(v)
end
function System.Int64.UInt16_System_Int64(v)
end
function System.Int64.UInt32_System_Int64(v)
end
function System.Int64.UInt64_System_Int64(v)
end