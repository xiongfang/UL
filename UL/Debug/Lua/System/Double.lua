require "System"
System.Double = System.ValueType:new()
function System.Double.TryParse_System_String_System_Double(value,v)
	do
		try
		do
			v = System.Double.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = System.Int32.Double(0);
			return false;
		end
	end
end
require "Double_ExtHeader"
