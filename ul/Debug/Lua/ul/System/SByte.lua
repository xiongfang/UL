require "ul.System"
ul.System.SByte = class('ul.System.SByte',ul.System.ValueType)
function ul.System.SByte.Parse_ul_System_String(value)
end
function ul.System.SByte:ToString()
end
function ul.System.SByte.TryParse_ul_System_String_ul_System_SByte(value,v,ref_func)
	do
		local __ret_v = try(
        function()
			do
				v = ul.System.SByte.Parse_ul_System_String(value);
				ref_func(v);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function(e)
				do
					v = ul.System.Int32.SByte_ul_System_Int32(ul.System.Int32.new(0));
					ref_func(v);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
		if __ret_v~= nil then return __ret_v end 
	end
end
function ul.System.SByte.op_Addition(a,b)
end
function ul.System.SByte.op_Substraction(a,b)
end
function ul.System.SByte.op_Multiply(a,b)
end
function ul.System.SByte.op_Division(a,b)
end
function ul.System.SByte.op_Modulus(a,b)
end
function ul.System.SByte.op_BitwiseAnd(a,b)
end
function ul.System.SByte.op_BitwiseOr(a,b)
end
function ul.System.SByte.op_GreaterThen(a,b)
end
function ul.System.SByte.op_LessThen(a,b)
end
function ul.System.SByte.op_OnesComplement(a)
end
function ul.System.SByte.op_LeftShift(a,b)
end
function ul.System.SByte.op_RightShift(a,b)
end
function ul.System.SByte.op_Equality(a,b)
end
function ul.System.SByte.op_Inequality(a,b)
end
function ul.System.SByte.op_Increment(a)
end
function ul.System.SByte.op_Decrement(a)
end
function ul.System.SByte.op_UnaryPlus(a)
end
function ul.System.SByte.op_UnaryNegation(a)
end
function ul.System.SByte.Int64_ul_System_SByte(v)
end
function ul.System.SByte.Int32_ul_System_SByte(v)
end
function ul.System.SByte.Single_ul_System_SByte(v)
end
function ul.System.SByte.Double_ul_System_SByte(v)
end
function ul.System.SByte.Byte_ul_System_SByte(v)
end
function ul.System.SByte.Int16_ul_System_SByte(v)
end
function ul.System.SByte.UInt16_ul_System_SByte(v)
end
function ul.System.SByte.UInt32_ul_System_SByte(v)
end
function ul.System.SByte.UInt64_ul_System_SByte(v)
end
require "SByte_ExtHeader"
