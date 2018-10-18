require "System"
System.UInt64 = System.ValueType:new()
function System.UInt64.TryParse_System_String_System_UInt64(value,v)
	do
		try
		do
			v = System.UInt64.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.UInt64(0);
			return false;
		end
	end
end
