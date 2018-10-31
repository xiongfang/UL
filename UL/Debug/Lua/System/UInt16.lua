require "System"
System.UInt16 = class('System.UInt16',System.ValueType)
function System.UInt16.Parse_System_String(value)
end
function System.UInt16:ToString()
end
function System.UInt16.TryParse_System_String_System_UInt16(value,v,func)
	do
		try(
        function()
			do
				v = System.UInt16.Parse_System_String(value);
				func(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.UInt16_System_Int32(System.Int32.new(0));
					func(value);
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.UInt16.op_Addition(a,b)
end
function System.UInt16.op_Substraction(a,b)
end
function System.UInt16.op_Multiply(a,b)
end
function System.UInt16.op_Division(a,b)
end
function System.UInt16.op_Modulus(a,b)
end
function System.UInt16.op_BitwiseAnd(a,b)
end
function System.UInt16.op_BitwiseOr(a,b)
end
function System.UInt16.op_GreaterThen(a,b)
end
function System.UInt16.op_LessThen(a,b)
end
function System.UInt16.op_OnesComplement(a)
end
function System.UInt16.op_LeftShift(a,b)
end
function System.UInt16.op_RightShift(a,b)
end
function System.UInt16.op_Equality(a,b)
end
function System.UInt16.op_Inequality(a,b)
end
function System.UInt16.op_Increment(a)
end
function System.UInt16.op_Decrement(a)
end
function System.UInt16.op_UnaryPlus(a)
end
function System.UInt16.Int64_System_UInt16(v)
end
function System.UInt16.Int32_System_UInt16(v)
end
function System.UInt16.Single_System_UInt16(v)
end
function System.UInt16.Double_System_UInt16(v)
end
function System.UInt16.Byte_System_UInt16(v)
end
function System.UInt16.SByte_System_UInt16(v)
end
function System.UInt16.Int16_System_UInt16(v)
end
function System.UInt16.UInt32_System_UInt16(v)
end
function System.UInt16.UInt64_System_UInt16(v)
end
require "UInt16_ExtHeader"
