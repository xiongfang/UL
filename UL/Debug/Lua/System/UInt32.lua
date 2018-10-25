require "System"
System.UInt32 = System.ValueType:new()
function System.UInt32.TryParse_System_String_System_UInt32(value,v)
	do
		try
		do
			v = System.UInt32.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.UInt32(0);
			return false;
		end
	end
end
require "UInt32_ExtHeader"
