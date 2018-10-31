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
		a = System.Int32.op_Addition(clone(a),clone(System.Int32.new(6)));
		System.Console.WriteLine_System_Int32(clone(a));
		a = System.Int32.op_Addition(clone(a),clone(System.Int32.new(7)));
		System.Console.WriteLine_System_Int32(clone(a));
		local b = System.Int32.Int64_System_Int32(a);
		System.Console.WriteLine_System_Int64(clone(b));
		System.Console.WriteLine_System_Int32(clone(System.Int32.op_Substraction(clone(a),clone(System.Int32.new(5)))));
		System.Console.WriteLine_System_Int32(clone(System.Int32.op_Modulus(clone(a),clone(System.Int32.new(5)))));
		System.Console.WriteLine_System_Int32(clone(PostfixUnaryHelper.op_Increment(a,function(v) a = v end)));
		System.Console.WriteLine_System_Int32(clone(PrefixUnaryHelper.op_Increment(a,function(v) a = v end)));
		System.Console.WriteLine_System_Int32(clone(System.Int32.op_UnaryNegation(a)));
		local tempV;
		if(System.Int32.TryParse_System_String_System_Int32(System.String.new("5"),clone(tempV),
function(v)
tempV=v
end
)) then
			System.Console.WriteLine_System_Int32(clone(tempV));
		end
	end
end
function System.Test.TestString()
	do
		local v = System.String.new("ÄãºÃ");
		System.Console.WriteLine_System_String(v);
		System.Console.WriteLine_System_Int32(clone(v:get_Length()));
	end
end
function System.Test.TestDel_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return System.Boolean.new(true);
	end
end
function System.Test.TestEvent()
	do
		System.Test::add_notify(new System.TestDel__Implement<System.Test>(nullptr,Test_notify));
		local Ref<System.TestDel> v = new System.TestDel__Implement<System.Test>(nullptr,System.Test.TestDel);
		System.Test::add_notify(v);
		_notify:Invoke(System.String.new("ÄãºÃ"));
	end
end
function System.Test.Test_notify_System_String(v)
	do
		System.Console.WriteLine_System_String(v);
		return System.Boolean.new(true);
	end
end
