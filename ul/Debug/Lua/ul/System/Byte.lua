require "ul_System"
ul.System.Byte = class('ul.System.Byte',ul.System.ValueType)
function ul.System.Byte.Parse_ul_System_String(value)
end
function ul.System.Byte:ToString()
end
function ul.System.Byte.TryParse_ul_System_String_ul_System_Byte(value,v,func)
	do
		try(
        function()
			do
				v = ul.System.Byte.Parse_ul_System_String(value);
				func(value);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Int32.Byte_ul_System_Int32(ul.System.Int32.new(0));
					func(value);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function ul.System.Byte.op_Addition(a,b)
end
function ul.System.Byte.op_Substraction(a,b)
end
function ul.System.Byte.op_Multiply(a,b)
end
function ul.System.Byte.op_Division(a,b)
end
function ul.System.Byte.op_Modulus(a,b)
end
function ul.System.Byte.op_BitwiseAnd(a,b)
end
function ul.System.Byte.op_BitwiseOr(a,b)
end
function ul.System.Byte.op_GreaterThen(a,b)
end
function ul.System.Byte.op_LessThen(a,b)
end
function ul.System.Byte.op_OnesComplement(a)
end
function ul.System.Byte.op_LeftShift(a,b)
end
function ul.System.Byte.op_RightShift(a,b)
end
function ul.System.Byte.op_Equality(a,b)
end
function ul.System.Byte.op_Inequality(a,b)
end
function ul.System.Byte.op_Increment(a)
end
function ul.System.Byte.op_Decrement(a)
end
function ul.System.Byte.op_UnaryPlus(a)
end
function ul.System.Byte.op_UnaryNegation(a)
end
function ul.System.Byte.Int64_ul_System_Byte(v)
end
function ul.System.Byte.Int32_ul_System_Byte(v)
end
function ul.System.Byte.Single_ul_System_Byte(v)
end
function ul.System.Byte.Double_ul_System_Byte(v)
end
function ul.System.Byte.Int16_ul_System_Byte(v)
end
function ul.System.Byte.SByte_ul_System_Byte(v)
end
function ul.System.Byte.UInt16_ul_System_Byte(v)
end
function ul.System.Byte.UInt32_ul_System_Byte(v)
end
function ul.System.Byte.UInt64_ul_System_Byte(v)
end
require "Byte_ExtHeader"
