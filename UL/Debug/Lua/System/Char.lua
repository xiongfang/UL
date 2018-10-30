require "System"
System.Char = class('System.Char',System.ValueType)
function System.Char.Parse_System_String(value)
end
function System.Char:ToString()
end
function System.Char.TryParse_System_String_System_Char(value,v)
	do
		try(
        function()
			do
				v = System.Char.Parse_System_String(value);
				return System.Boolean.new(true);
			end
		end,
		{
			type="System.Exception",
			func= function()
				do
					v = System.Char.new('\0');
					return System.Boolean.new(false);
				end
			end
		}
		);
	end
end
require "Char_ExtHeader"
