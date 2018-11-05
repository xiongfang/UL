require "ul_System"
ul.System.Double = class('ul.System.Double',ul.System.ValueType)
function ul.System.Double.Parse_ul_System_String(value)
end
function ul.System.Double:ToString()
end
function ul.System.Double.TryParse_ul_System_String_ul_System_Double(value,v,func)
	do
		try(
        function()
			do
				v = ul.System.Double.Parse_ul_System_String(value);
				func(value);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Int32.Double_ul_System_Int32(ul.System.Int32.new(0));
					func(value);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function ul.System.Double.op_Addition(a,b)
end
function ul.System.Double.op_Substraction(a,b)
end
function ul.System.Double.op_Multiply(a,b)
end
function ul.System.Double.op_Division(a,b)
end
function ul.System.Double.op_GreaterThen(a,b)
end
function ul.System.Double.op_LessThen(a,b)
end
function ul.System.Double.op_Equality(a,b)
end
function ul.System.Double.op_Inequality(a,b)
end
function ul.System.Double.op_Increment(a)
end
function ul.System.Double.op_Decrement(a)
end
function ul.System.Double.op_UnaryPlus(a)
end
function ul.System.Double.op_UnaryNegation(a)
end
function ul.System.Double.Int64_ul_System_Double(v)
end
function ul.System.Double.Int32_ul_System_Double(v)
end
function ul.System.Double.Single_ul_System_Double(v)
end
function ul.System.Double.Int16_ul_System_Double(v)
end
function ul.System.Double.Byte_ul_System_Double(v)
end
function ul.System.Double.SByte_ul_System_Double(v)
end
function ul.System.Double.UInt16_ul_System_Double(v)
end
function ul.System.Double.UInt32_ul_System_Double(v)
end
function ul.System.Double.UInt64_ul_System_Double(v)
end
require "Double_ExtHeader"
