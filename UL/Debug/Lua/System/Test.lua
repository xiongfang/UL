require "System"
System.Test = class('System.Test',System.Object)
function System.Test.add_notify_System_TestDel(value)
	do
		_notify = System.TestDel.op_Addition(_notify,value);
	end
end
function System.Test.remove_notify_System_TestDel(value)
	do
		_notify = System.TestDel.op_Substraction(_notify,value);
	end
end
function System.Test.Run()
	do
		System.Test.TestInt();
		System.Test.TestString();
		System.Test.TestEvent();
	end
end
function System.Test.TestInt()
	do
		local a = System.Int32.new(5);
		a = System.Int32.op_Addition(a,System.Int32.new(6));
		System.Console.WriteLine_System_Int32(a);
		a = System.Int32.op_Addition(a,System.Int32.new(7));
		System.Console.WriteLine_System_Int32(a);
		local b = System.Int32.Int64_System_Int32(a);
		System.Console.WriteLine_System_Int64(b);
		System.Console.WriteLine_System_Int32(System.Int32.op_Substraction(a,System.Int32.new(5)));
		System.Console.WriteLine_System_Int32(System.Int32.op_Modulus(a,System.Int32.new(5)));
		System.Console.WriteLine_System_Int32(System.Int32.op_Increment(a));
		System.Console.WriteLine_System_Int32(System.Int32.op_Increment(a));
		System.Console.WriteLine_System_Int32(System.Int32.op_UnaryNegation(a));
	end
end
function System.Test.TestString()
	do
		local v = System.String.new("ÄãºÃ");
		System.Console.WriteLine_System_String(v);
		System.Console.WriteLine_System_Int32(v:get_Length());
	end
end
function System.Test.TestDel_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return System.Boolean.new(true);
	end
end
function System.Test.TestEvent()

end
function System.Test.Test_notify_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return System.Boolean.new(true);
	end
end
