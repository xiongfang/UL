require "System"
System.Byte = System.ValueType:new()
function System.Byte.TryParse_System_String_System_Byte(value,v)
	do
		try
		do
			v = System.Byte.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.Byte(0);
			return false;
		end
	end
end
