require "ul_System"
ul.System.Console = class('ul.System.Console',ul.System.Object)
function ul.System.Console.Write_ul_System_String(value)
end
function ul.System.Console.Write_ul_System_Object(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Char(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Boolean(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Int32(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Int64(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Single(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Double(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.Write_ul_System_Byte(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
	end
end
function ul.System.Console.WriteLine()
	do
		ul.System.Console.Write_ul_System_String(ul.System.String.new("\r\n"));
	end
end
function ul.System.Console.WriteLine_ul_System_Char(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Boolean(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Int32(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Int64(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Single(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Double(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Byte(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_String(value)
	do
		ul.System.Console.Write_ul_System_String(value);
		ul.System.Console.WriteLine();
	end
end
function ul.System.Console.WriteLine_ul_System_Object(value)
	do
		ul.System.Console.Write_ul_System_String(value:ToString());
		ul.System.Console.WriteLine();
	end
end
require "Console_ExtHeader"
