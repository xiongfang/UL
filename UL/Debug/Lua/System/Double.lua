require "System"
System.Double = class('System.Double',System.ValueType)
function System.Double.Parse_System_String(value)
end
function System.Double:ToString()
end
function System.Double.TryParse_System_String_System_Double(value,v,func)
	do
		try(
        function()
			do
				v = System.Double.Parse_System_String(value);
				func(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.Double_System_Int32(System.Int32.new(0));
					func(value);
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Double.op_Addition(a,b)
end
function System.Double.op_Substraction(a,b)
end
function System.Double.op_Multiply(a,b)
end
function System.Double.op_Division(a,b)
end
function System.Double.op_GreaterThen(a,b)
end
function System.Double.op_LessThen(a,b)
end
function System.Double.op_Equality(a,b)
end
function System.Double.op_Inequality(a,b)
end
function System.Double.op_Increment(a)
end
function System.Double.op_Decrement(a)
end
function System.Double.op_UnaryPlus(a)
end
function System.Double.op_UnaryNegation(a)
end
function System.Double.Int64_System_Double(v)
end
function System.Double.Int32_System_Double(v)
end
function System.Double.Single_System_Double(v)
end
function System.Double.Int16_System_Double(v)
end
function System.Double.Byte_System_Double(v)
end
function System.Double.SByte_System_Double(v)
end
function System.Double.UInt16_System_Double(v)
end
function System.Double.UInt32_System_Double(v)
end
function System.Double.UInt64_System_Double(v)
end
require "Double_ExtHeader"
