require "System"
System.Boolean = class('System.Boolean',System.ValueType)
function System.Boolean.Parse_System_String(value)
	do
		if(System.String.op_Equality(value,nil)) then
		do
			return System.ArgumentNullException.new();
		end
		end
		if(System.String.op_Equality(value,TrueString)) then
			return System.Boolean.new(true);
		else
			if(System.String.op_Equality(value,FalseString)) then
				return System.Boolean.new(false);
			else
			do
				return System.FormatException.new(value);
			end
			end
		end
	end
end
function System.Boolean:ToString()
end
function System.Boolean.TryParse_System_String_System_Boolean(value,v)
	do
		try(
        function()
			do
				v = System.Boolean.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Boolean.new(false);
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
function System.Boolean.op_LogicNot(a)
end
require "Boolean_ExtHeader"
