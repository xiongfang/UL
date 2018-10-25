require "System"
System.Int32 = System.ValueType:new()
function System.Int32.TryParse_System_String_System_Int32(value,v)
	do
		try
		do
			v = System.Int32.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = 0;
			return false;
		end
	end
end
require "Int32_ExtHeader"
