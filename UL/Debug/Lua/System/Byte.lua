require "System"
System.Byte = class('System.Byte',System.ValueType)
function System.Byte.Parse_System_String(value)
end
function System.Byte:ToString()
end
function System.Byte.TryParse_System_String_System_Byte(value,v)
	do
		try(
        function()
			do
				v = System.Byte.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.Byte_System_Int32(System.Int32.new(0));
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Byte.op_Addition(a,b)
end
function System.Byte.op_Substraction(a,b)
end
function System.Byte.op_Multiply(a,b)
end
function System.Byte.op_Division(a,b)
end
function System.Byte.op_Modulus(a,b)
end
function System.Byte.op_BitwiseAnd(a,b)
end
function System.Byte.op_BitwiseOr(a,b)
end
function System.Byte.op_GreaterThen(a,b)
end
function System.Byte.op_LessThen(a,b)
end
function System.Byte.op_OnesComplement(a)
end
function System.Byte.op_LeftShift(a,b)
end
function System.Byte.op_RightShift(a,b)
end
function System.Byte.op_Equality(a,b)
end
function System.Byte.op_Inequality(a,b)
end
function System.Byte.op_Increment(a)
end
function System.Byte.op_Decrement(a)
end
function System.Byte.op_UnaryPlus(a)
end
function System.Byte.op_UnaryNegation(a)
end
function System.Byte.Int64_System_Byte(v)
end
function System.Byte.Int32_System_Byte(v)
end
function System.Byte.Single_System_Byte(v)
end
function System.Byte.Double_System_Byte(v)
end
function System.Byte.Int16_System_Byte(v)
end
function System.Byte.SByte_System_Byte(v)
end
function System.Byte.UInt16_System_Byte(v)
end
function System.Byte.UInt32_System_Byte(v)
end
function System.Byte.UInt64_System_Byte(v)
end
require "Byte_ExtHeader"
