require "System"
System.UInt16 = System.ValueType:new()
function System.UInt16.TryParse_System_String_System_UInt16(value,v)
	do
		try
		do
			v = System.UInt16.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.UInt16(0);
			return false;
		end
	end
end
require "UInt16_ExtHeader"
