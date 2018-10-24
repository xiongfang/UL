require "System"
System.Single = System.ValueType:new()
function System.Single.TryParse_System_String_System_Single(value,v)
	do
		try
		do
			v = System.Single.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.Single(0);
			return false;
		end
	end
end
