function ul.System.Int32:ctor(v)
	if v~= nil then
		self.v = v;
	else
		self.v = 0;
	end
end
function ul.System.Int32.Parse_ul_System_String(value)
	local v = tonumber(value._v)
	if(v == nil) then
		error(Construct(ul.System.Exception:new(),"Exception_ul_System_String",ul.System.String.new("Int32.Parse Exception")));
	end

	return ul.System.Int32.new(v);

end
function ul.System.Int32:ToString()
	return ul.System.String.new(tostring(self.v));
end
function ul.System.Int32.op_Addition(a,b)
	return ul.System.Int32.new(a.v+b.v);
end
function ul.System.Int32.op_Substraction(a,b)
	return ul.System.Int32.new(a.v - b.v)
end
function ul.System.Int32.op_Multiply(a,b)
	return ul.System.Int32.new(a.v * b.v)
end
function ul.System.Int32.op_Division(a,b)
	return ul.System.Int32.new(a.v / b.v)
end
function ul.System.Int32.op_Modulus(a,b)
	return ul.System.Int32.new(a.v % b.v)
end
function ul.System.Int32.op_BitwiseAnd(a,b)

end
function ul.System.Int32.op_BitwiseOr(a,b)
end
function ul.System.Int32.op_GreaterThen(a,b)
end
function ul.System.Int32.op_LessThen(a,b)
end
function ul.System.Int32.op_Equality(a,b)
end
function ul.System.Int32.op_Inequality(a,b)
end
function ul.System.Int32.op_LeftShift(a,b)
end
function ul.System.Int32.op_RightShift(a,b)
end
function ul.System.Int32.op_Increment(a)
	return ul.System.Int32.new(a.v +1);
end
function ul.System.Int32.op_Decrement(a)
	return ul.System.Int32.new(a.v -1);
end
function ul.System.Int32.op_OnesComplement(a)
end
function ul.System.Int32.op_UnaryPlus(a)
end
function ul.System.Int32.op_UnaryNegation(a)
	return ul.System.Int32.new(-a.v);
end
function ul.System.Int32.Int64_ul_System_Int32(v)
	return ul.System.Int64.new(v.v);
end
function ul.System.Int32.Int16_ul_System_Int32(v)
end
function ul.System.Int32.Single_ul_System_Int32(v)
end
function ul.System.Int32.Double_ul_System_Int32(v)
end
function ul.System.Int32.Byte_ul_System_Int32(v)
end
function ul.System.Int32.SByte_ul_System_Int32(v)
end
function ul.System.Int32.UInt16_ul_System_Int32(v)
end
function ul.System.Int32.UInt32_ul_System_Int32(v)
end
function ul.System.Int32.UInt64_ul_System_Int32(v)
	return ul.System.UInt64.new(v.v);
end