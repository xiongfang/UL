require "System"
System.Int16 = System.ValueType:new()
function System.Int16.TryParse_System_String_System_Int16(value,v)
	do
		try
		do
			v = System.Int16.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.Int16(0);
			return false;
		end
	end
end
