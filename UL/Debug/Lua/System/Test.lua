require "System"
System.Test = System.Object:new()
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
		local a = 5;
		a = System.Int32.op_Addition(a,6);
		System.Console.WriteLine_System_Int32(a);
		a = System.Int32.op_Addition(a,7);
		System.Console.WriteLine_System_Int32(a);
		local b = System.Int32.Int64(a);
		System.Console.WriteLine_System_Int64(b);
		System.Console.WriteLine_System_Int32(System.Int32.op_Substraction(a,5));
		System.Console.WriteLine_System_Int32(System.Int32.op_Modulus(a,5));
		System.Console.WriteLine_System_Int32(System.Int32.op_Increment(a));
		System.Console.WriteLine_System_Int32(System.Int32.op_Increment(a));
		System.Console.WriteLine_System_Int32(System.Int32.op_UnaryNegation(a));
	end
end
function System.Test.TestString()
	do
		local v = "你好";
		System.Console.WriteLine_System_String(v);
		System.Console.WriteLine_System_Int32(v.get_Length());
	end
end
function System.Test.TestDel_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return true;
	end
end
function System.Test.TestEvent()
	do
		System.Test::add_notify(new System.TestDel__Implement<System.Test>(nullptr,Test_notify));
		local Ref<System.TestDel> v = new System.TestDel__Implement<System.Test>(nullptr,System.Test.TestDel);
		System.Test::add_notify(v);
		_notify:Invoke("你好");
	end
end
function System.Test.Test_notify_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return true;
	end
end
