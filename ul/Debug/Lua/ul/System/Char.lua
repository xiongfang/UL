require "ul_System"
ul.System.Char = class('ul.System.Char',ul.System.ValueType)
function ul.System.Char.Parse_ul_System_String(value)
end
function ul.System.Char:ToString()
end
function ul.System.Char.TryParse_ul_System_String_ul_System_Char(value,v,func)
	do
		try(
        function()
			do
				v = ul.System.Char.Parse_ul_System_String(value);
				func(value);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function()
				do
					v = ul.System.Char.new('\0');
					func(value);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
	end
end
require "Char_ExtHeader"
