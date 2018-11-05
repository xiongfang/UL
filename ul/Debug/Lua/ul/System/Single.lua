require "ul_System"
ul.System.Single = class('ul.System.Single',ul.System.ValueType)
function ul.System.Single.Parse_ul_System_String(value)
end
function ul.System.Single:ToString()
end
function ul.System.Single.TryParse_ul_System_String_ul_System_Single(value,v,func)
	do
		try(
        function()
			do
				v = ul.System.Single.Parse_ul_System_String(value);
				func(value);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Int32.Single_ul_System_Int32(ul.System.Int32.new(0));
					func(value);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function ul.System.Single.op_Addition(a,b)
end
function ul.System.Single.op_Substraction(a,b)
end
function ul.System.Single.op_Multiply(a,b)
end
function ul.System.Single.op_Division(a,b)
end
function ul.System.Single.op_GreaterThen(a,b)
end
function ul.System.Single.op_LessThen(a,b)
end
function ul.System.Single.op_OnesComplement(a)
end
function ul.System.Single.op_Equality(a,b)
end
function ul.System.Single.op_Inequality(a,b)
end
function ul.System.Single.op_Increment(a)
end
function ul.System.Single.op_Decrement(a)
end
function ul.System.Single.op_UnaryPlus(a)
end
function ul.System.Single.op_UnaryNegation(a)
end
function ul.System.Single.Int64_ul_System_Single(v)
end
function ul.System.Single.Int32_ul_System_Single(v)
end
function ul.System.Single.Int16_ul_System_Single(v)
end
function ul.System.Single.Double_ul_System_Single(v)
end
function ul.System.Single.Byte_ul_System_Single(v)
end
function ul.System.Single.SByte_ul_System_Single(v)
end
function ul.System.Single.UInt16_ul_System_Single(v)
end
function ul.System.Single.UInt32_ul_System_Single(v)
end
function ul.System.Single.UInt64_ul_System_Single(v)
end
require "Single_ExtHeader"
