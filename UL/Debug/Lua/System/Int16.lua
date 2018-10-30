require "System"
System.Int16 = class('System.Int16',System.ValueType)
function System.Int16.Parse_System_String(value)
end
function System.Int16:ToString()
end
function System.Int16.TryParse_System_String_System_Int16(value,v)
	do
		try(
        function()
			do
				v = System.Int16.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.Int16_System_Int32(System.Int32.new(0));
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Int16.op_Addition(a,b)
end
function System.Int16.op_Substraction(a,b)
end
function System.Int16.op_Multiply(a,b)
end
function System.Int16.op_Division(a,b)
end
function System.Int16.op_Modulus(a,b)
end
function System.Int16.op_BitwiseAnd(a,b)
end
function System.Int16.op_BitwiseOr(a,b)
end
function System.Int16.op_GreaterThen(a,b)
end
function System.Int16.op_LessThen(a,b)
end
function System.Int16.op_OnesComplement(a)
end
function System.Int16.op_LeftShift(a,b)
end
function System.Int16.op_RightShift(a,b)
end
function System.Int16.op_Equality(a,b)
end
function System.Int16.op_Inequality(a,b)
end
function System.Int16.op_Increment(a)
end
function System.Int16.op_Decrement(a)
end
function System.Int16.Int64_System_Int16(v)
end
function System.Int16.Int32_System_Int16(v)
end
function System.Int16.Single_System_Int16(v)
end
function System.Int16.Double_System_Int16(v)
end
function System.Int16.Byte_System_Int16(v)
end
function System.Int16.SByte_System_Int16(v)
end
function System.Int16.UInt16_System_Int16(v)
end
function System.Int16.UInt32_System_Int16(v)
end
function System.Int16.UInt64_System_Int16(v)
end
require "Int16_ExtHeader"
