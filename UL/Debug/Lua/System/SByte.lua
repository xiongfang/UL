require "System"
System.SByte = System.ValueType:new()
function System.SByte.TryParse_System_String_System_SByte(value,v)
	do
		try
		do
			v = System.SByte.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.SByte(0);
			return false;
		end
	end
end
