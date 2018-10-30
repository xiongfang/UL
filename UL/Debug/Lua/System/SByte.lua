require "System"
System.SByte = class('System.SByte',System.ValueType)
function System.SByte.Parse_System_String(value)
end
function System.SByte:ToString()
end
function System.SByte.TryParse_System_String_System_SByte(value,v)
	do
		try(
        function()
			do
				v = System.SByte.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.SByte_System_Int32(System.Int32.new(0));
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.SByte.op_Addition(a,b)
end
function System.SByte.op_Substraction(a,b)
end
function System.SByte.op_Multiply(a,b)
end
function System.SByte.op_Division(a,b)
end
function System.SByte.op_Modulus(a,b)
end
function System.SByte.op_BitwiseAnd(a,b)
end
function System.SByte.op_BitwiseOr(a,b)
end
function System.SByte.op_GreaterThen(a,b)
end
function System.SByte.op_LessThen(a,b)
end
function System.SByte.op_OnesComplement(a)
end
function System.SByte.op_LeftShift(a,b)
end
function System.SByte.op_RightShift(a,b)
end
function System.SByte.op_Equality(a,b)
end
function System.SByte.op_Inequality(a,b)
end
function System.SByte.op_Increment(a)
end
function System.SByte.op_Decrement(a)
end
function System.SByte.op_UnaryPlus(a)
end
function System.SByte.op_UnaryNegation(a)
end
function System.SByte.Int64_System_SByte(v)
end
function System.SByte.Int32_System_SByte(v)
end
function System.SByte.Single_System_SByte(v)
end
function System.SByte.Double_System_SByte(v)
end
function System.SByte.Byte_System_SByte(v)
end
function System.SByte.Int16_System_SByte(v)
end
function System.SByte.UInt16_System_SByte(v)
end
function System.SByte.UInt32_System_SByte(v)
end
function System.SByte.UInt64_System_SByte(v)
end
require "SByte_ExtHeader"
