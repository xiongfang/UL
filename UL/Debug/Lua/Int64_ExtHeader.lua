
function ul.System.Int64:ctor(v)
	if v~= nil then
		self.v = v;
	else
		self.v = 0;
	end
end

function ul.System.Int64.Parse_System_String(value)
	return ul.System.String:new(tostring(self.v));
end
function ul.System.Int64:ToString()
	return ul.System.String.new(tostring(self.v));
end
function ul.System.Int64.op_Addition(a,b)
	return ul.System.Int64.new(a.v+b.v);
end
function ul.System.Int64.op_Substraction(a,b)
	return ul.System.Int64.new(a.v - b.v)
end
function ul.System.Int64.op_Multiply(a,b)
	return ul.System.Int64.new(a.v * b.v)
end
function ul.System.Int64.op_Division(a,b)
	return ul.System.Int64.new(a.v / b.v)
end
function ul.System.Int64.op_Modulus(a,b)
	return ul.System.Int64.new(a.v % b.v)
end
function ul.System.Int64.op_BitwiseAnd(a,b)
end
function ul.System.Int64.op_BitwiseOr(a,b)
end
function ul.System.Int64.op_GreaterThen(a,b)
end
function ul.System.Int64.op_LessThen(a,b)
end
function ul.System.Int64.op_OnesComplement(a)
end
function ul.System.Int64.op_LeftShift(a,b)
end
function ul.System.Int64.op_RightShift(a,b)
end
function ul.System.Int64.op_Equality(a,b)
end
function ul.System.Int64.op_Inequality(a,b)
end
function ul.System.Int64.op_Increment(a)
end
function ul.System.Int64.op_Decrement(a)
end
function ul.System.Int64.op_UnaryPlus(a)
end
function ul.System.Int64.op_UnaryNegation(a)
end
function ul.System.Int64.Int16_ul_System_Int64(v)
end
function ul.System.Int64.Int32_ul_System_Int64(v)
end
function ul.System.Int64.Single_ul_System_Int64(v)
end
function ul.System.Int64.Double_ul_System_Int64(v)
end
function ul.System.Int64.Byte_ul_System_Int64(v)
end
function ul.System.Int64.SByte_ul_System_Int64(v)
end
function ul.System.Int64.UInt16_ul_System_Int64(v)
end
function ul.System.Int64.UInt32_ul_System_Int64(v)
end
function ul.System.Int64.UInt64_ul_System_Int64(v)
end