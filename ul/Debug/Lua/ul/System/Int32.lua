require "ul.System"
ul.System.Int32 = class('ul.System.Int32',ul.System.ValueType)
function ul.System.Int32.Parse_ul_System_String(value)
end
function ul.System.Int32:ToString()
end
function ul.System.Int32.TryParse_ul_System_String_ul_System_Int32(value,v,ref_func)
	do
		local __ret_v = try(
        function()
			do
				v = ul.System.Int32.Parse_ul_System_String(value);
				ref_func(v);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Int32.new(0);
					ref_func(v);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
		if __ret_v~= nil then return __ret_v end 
	end
end
function ul.System.Int32.op_Addition(a,b)
end
function ul.System.Int32.op_Substraction(a,b)
end
function ul.System.Int32.op_Multiply(a,b)
end
function ul.System.Int32.op_Division(a,b)
end
function ul.System.Int32.op_Modulus(a,b)
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
end
function ul.System.Int32.op_Decrement(a)
end
function ul.System.Int32.op_OnesComplement(a)
end
function ul.System.Int32.op_UnaryPlus(a)
end
function ul.System.Int32.op_UnaryNegation(a)
end
function ul.System.Int32.Int64_ul_System_Int32(v)
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
end
require "Int32_ExtHeader"
