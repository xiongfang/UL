require "System"
System.Console = System.Object:new()
function System.Console.Write_System_Object(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Char(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Boolean(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Int32(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Int64(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Single(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Double(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.Write_System_Byte(value)
	do
		System.Console.Write_System_String(value:ToString());
	end
end
function System.Console.WriteLine()
	do
		System.Console.Write_System_String("\r\n");
	end
end
function System.Console.WriteLine_System_Char(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Boolean(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Int32(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Int64(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Single(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Double(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Byte(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_String(value)
	do
		System.Console.Write_System_String(value);
		System.Console.WriteLine();
	end
end
function System.Console.WriteLine_System_Object(value)
	do
		System.Console.Write_System_String(value:ToString());
		System.Console.WriteLine();
	end
end
