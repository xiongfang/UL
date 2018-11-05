require "ul_System"
ul.System.Boolean = class('ul.System.Boolean',ul.System.ValueType)
function ul.System.Boolean.Parse_ul_System_String(value)
	do
		if(ul.System.String.op_Equality(value,nil)) then
		do
			return ul.System.ArgumentNullException.new();
		end
		end
		if(ul.System.String.op_Equality(value,TrueString)) then
			return ul.System.Boolean.new(true);
		else
			if(ul.System.String.op_Equality(value,FalseString)) then
				return ul.System.Boolean.new(false);
			else
			do
				return ul.System.FormatException.new(value);
			end
			end
		end
	end
end
function ul.System.Boolean:ToString()
end
function ul.System.Boolean.TryParse_ul_System_String_ul_System_Boolean(value,v,func)
	do
		try(
        function()
			do
				v = ul.System.Boolean.Parse_ul_System_String(value);
				func(value);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Boolean.new(false);
					func(value);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function ul.System.Boolean.op_LogicNot(a)
end
require "Boolean_ExtHeader"
