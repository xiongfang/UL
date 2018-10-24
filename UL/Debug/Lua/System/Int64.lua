require "System"
System.Int64 = System.ValueType:new()
function System.Int64.TryParse_System_String_System_Int64(value,v)
	do
		try
		do
			v = System.Int64.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.Int64(0);
			return false;
		end
	end
end
