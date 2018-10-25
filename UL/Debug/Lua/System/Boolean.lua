require "System"
System.Boolean = System.ValueType:new()
function System.Boolean.Parse_System_String(value)
	do
		if(System.String.op_Equality(value,nil))
		do
			throw System.ArgumentNullException:new();
		end
		end
		if(System.String.op_Equality(value,TrueString))
			return true;
		else
			if(System.String.op_Equality(value,FalseString))
				return false;
			else
			do
				throw System.FormatException:new(value);
			end
			end
		end
	end
end
function System.Boolean.TryParse_System_String_System_Boolean(value,v)
	do
		try
		do
			v = System.Boolean.Parse_System_String(value);
			return true;
		end
		catch(System.Exception e)
		do
			v = false;
			return false;
		end
	end
end
require "Boolean_ExtHeader"
