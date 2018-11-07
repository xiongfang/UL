require "ul.System"
ul.System.Boolean = class('ul.System.Boolean',ul.System.ValueType)
function ul.System.Boolean.Parse_ul_System_String(value)
	do
		if (ul.System.String.op_Equality(value,nil))._v then
		do
			return Construct(ul.System.ArgumentNullException.new(),"ArgumentNullException_ul_System_String");
		end
		end
		if (ul.System.String.op_Equality(value,ul.System.Boolean.TrueString))._v then
			return ul.System.Boolean.new(true);
		else
			if (ul.System.String.op_Equality(value,ul.System.Boolean.FalseString))._v then
				return ul.System.Boolean.new(false);
			else
			do
				return Construct(ul.System.FormatException.new(),"FormatException_ul_System_String",value);
			end
			end
		end
	end
end
function ul.System.Boolean:ToString()
end
function ul.System.Boolean.TryParse_ul_System_String_ul_System_Boolean(value,v,ref_func)
	do
		local __ret_v = try(
        function()
			do
				v = ul.System.Boolean.Parse_ul_System_String(value);
				ref_func(v);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function(e)
				do
					v = ul.System.Boolean.new(false);
					ref_func(v);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
		if __ret_v~= nil then return __ret_v end 
	end
end
function ul.System.Boolean.op_LogicNot(a)
end
require "Boolean_ExtHeader"
