require "System"
System.Single = class('System.Single',System.ValueType)
function System.Single.Parse_System_String(value)
end
function System.Single:ToString()
end
function System.Single.TryParse_System_String_System_Single(value,v)
	do
		try(
        function()
			do
				v = System.Single.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Int32.Single_System_Int32(System.Int32.new(0));
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Single.op_Addition(a,b)
end
function System.Single.op_Substraction(a,b)
end
function System.Single.op_Multiply(a,b)
end
function System.Single.op_Division(a,b)
end
function System.Single.op_GreaterThen(a,b)
end
function System.Single.op_LessThen(a,b)
end
function System.Single.op_OnesComplement(a)
end
function System.Single.op_Equality(a,b)
end
function System.Single.op_Inequality(a,b)
end
function System.Single.op_Increment(a)
end
function System.Single.op_Decrement(a)
end
function System.Single.op_UnaryPlus(a)
end
function System.Single.op_UnaryNegation(a)
end
function System.Single.Int64_System_Single(v)
end
function System.Single.Int32_System_Single(v)
end
function System.Single.Int16_System_Single(v)
end
function System.Single.Double_System_Single(v)
end
function System.Single.Byte_System_Single(v)
end
function System.Single.SByte_System_Single(v)
end
function System.Single.UInt16_System_Single(v)
end
function System.Single.UInt32_System_Single(v)
end
function System.Single.UInt64_System_Single(v)
end
require "Single_ExtHeader"
