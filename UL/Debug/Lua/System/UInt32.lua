require "System"
System.UInt32 = class('System.UInt32',System.ValueType)
function System.UInt32.Parse_System_String(value)
end
function System.UInt32:ToString()
end
function System.UInt32.TryParse_System_String_System_UInt32(value,v)
	do
		try(
        function()
			do
				v = System.UInt32.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.UInt32_System_Int32(System.Int32.new(0));
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.UInt32.op_Addition(a,b)
end
function System.UInt32.op_Substraction(a,b)
end
function System.UInt32.op_Multiply(a,b)
end
function System.UInt32.op_Division(a,b)
end
function System.UInt32.op_Modulus(a,b)
end
function System.UInt32.op_BitwiseAnd(a,b)
end
function System.UInt32.op_BitwiseOr(a,b)
end
function System.UInt32.op_GreaterThen(a,b)
end
function System.UInt32.op_LessThen(a,b)
end
function System.UInt32.op_OnesComplement(a)
end
function System.UInt32.op_LeftShift(a,b)
end
function System.UInt32.op_RightShift(a,b)
end
function System.UInt32.op_Equality(a,b)
end
function System.UInt32.op_Inequality(a,b)
end
function System.UInt32.op_Increment(a)
end
function System.UInt32.op_Decrement(a)
end
function System.UInt32.op_UnaryPlus(a)
end
function System.UInt32.Int64_System_UInt32(v)
end
function System.UInt32.Int32_System_UInt32(v)
end
function System.UInt32.Single_System_UInt32(v)
end
function System.UInt32.Double_System_UInt32(v)
end
function System.UInt32.Byte_System_UInt32(v)
end
function System.UInt32.SByte_System_UInt32(v)
end
function System.UInt32.UInt16_System_UInt32(v)
end
function System.UInt32.UInt64_System_UInt32(v)
end
function System.UInt32.Int16_System_UInt32(v)
end
require "UInt32_ExtHeader"
