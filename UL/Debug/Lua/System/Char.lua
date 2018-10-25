require "System"
System.Char = System.ValueType:new()
function System.Char.TryParse_System_String_System_Char(value,v)
	do
		try
		do
			v = System.Char.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = '\0';
			return false;
		end
	end
end
require "Char_ExtHeader"
