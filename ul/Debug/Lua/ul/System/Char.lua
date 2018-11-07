require "ul.System"
ul.System.Char = class('ul.System.Char',ul.System.ValueType)
function ul.System.Char.Parse_ul_System_String(value)
end
function ul.System.Char:ToString()
end
function ul.System.Char.TryParse_ul_System_String_ul_System_Char(value,v,ref_func)
	do
		local __ret_v = try(
        function()
			do
				v = ul.System.Char.Parse_ul_System_String(value);
				ref_func(v);
				return ul.System.Boolean.new(true);
			end
		end,
		{
			type="ul.System.Exception",
			func= function(e)
				do
					v = ul.System.Char.new('\0');
					ref_func(v);
					return ul.System.Boolean.new(false);
				end
			end
		}
		);
		if __ret_v~= nil then return __ret_v end 
	end
end
require "Char_ExtHeader"
