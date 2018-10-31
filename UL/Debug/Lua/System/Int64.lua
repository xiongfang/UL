require "System"
System.Int64 = class('System.Int64',System.ValueType)
function System.Int64.Parse_System_String(value)
end
function System.Int64:ToString()
end
function System.Int64.TryParse_System_String_System_Int64(value,v,func)
	do
		try(
        function()
			do
				v = System.Int64.Parse_System_String(value);
				func(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.Int64_System_Int32(System.Int32.new(0));
					func(value);
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Int64.op_Addition(a,b)
end
function System.Int64.op_Substraction(a,b)
end
function System.Int64.op_Multiply(a,b)
end
function System.Int64.op_Division(a,b)
end
function System.Int64.op_Modulus(a,b)
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
require "Int64_ExtHeader"
