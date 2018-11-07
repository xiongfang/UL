require "ul.System"
ul.System.Test = class('ul.System.Test',ul.System.Object)
function ul.System.Test.add_notify_ul_System_TestDel(value)
	do
		_notify = ul.System.TestDel.op_Addition(_notify,value);
	end
end
function ul.System.Test.remove_notify_ul_System_TestDel(value)
	do
		_notify = ul.System.TestDel.op_Substraction(_notify,value);
	end
end
function ul.System.Test.Run()
	do
		ul.System.Test.TestInt();
		ul.System.Test.TestString();
		ul.System.Test.TestEvent();
	end
end
function ul.System.Test.TestInt()
	do
		local a = ul.System.Int32.new(5);
		a = ul.System.Int32.op_Addition(clone(a),clone(ul.System.Int32.new(6)));
		ul.System.Console.WriteLine_ul_System_Int32(clone(a));
		a = ul.System.Int32.op_Addition(clone(a),clone(ul.System.Int32.new(7)));
		ul.System.Console.WriteLine_ul_System_Int32(clone(a));
		local b = ul.System.Int32.Int64_ul_System_Int32(a);
		ul.System.Console.WriteLine_ul_System_Int64(clone(b));
		ul.System.Console.WriteLine_ul_System_Int32(clone(ul.System.Int32.op_Substraction(clone(a),clone(ul.System.Int32.new(5)))));
		ul.System.Console.WriteLine_ul_System_Int32(clone(ul.System.Int32.op_Modulus(clone(a),clone(ul.System.Int32.new(5)))));
		ul.System.Console.WriteLine_ul_System_Int32(clone(PostfixUnaryHelper.op_Increment(a,function(v) a = v end)));
		ul.System.Console.WriteLine_ul_System_Int32(clone(PrefixUnaryHelper.op_Increment(a,function(v) a = v end)));
		ul.System.Console.WriteLine_ul_System_Int32(clone(ul.System.Int32.op_UnaryNegation(a)));
		local tempV;
		if (ul.System.Int32.TryParse_ul_System_String_ul_System_Int32(ul.System.String.new("5"),clone(tempV),
function(v)
tempV=v
end
))._v then
			ul.System.Console.WriteLine_ul_System_Int32(clone(tempV));
		end
		if (ul.System.Int32.TryParse_ul_System_String_ul_System_Int32(ul.System.String.new("sadas"),clone(tempV),
function(v)
tempV=v
end
))._v then
			ul.System.Console.WriteLine_ul_System_Int32(clone(tempV));
		else
			ul.System.Console.WriteLine_ul_System_String(ul.System.String.new("TryParse Error"));
		end
	end
end
function ul.System.Test.TestString()
	do
		local v = ul.System.String.new("ÄãºÃ");
		ul.System.Console.WriteLine_ul_System_String(v);
		ul.System.Console.WriteLine_ul_System_Int32(clone(v:get_Length()));
	end
end
function ul.System.Test.TestDel_ul_System_String(v)
	do
		ul.System.Console.WriteLine_ul_System_String(v);
		return ul.System.Boolean.new(true);
	end
end
function ul.System.Test.TestEvent()
	do

	end
end
function ul.System.Test.Test_notify_ul_System_String(v)
	do
		ul.System.Console.WriteLine_ul_System_String(v);
		return ul.System.Boolean.new(true);
	end
end
