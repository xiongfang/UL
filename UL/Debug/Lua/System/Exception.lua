require "System"
System.Exception = class('System.Exception',System.Object)
function System.Exception:get_Message()
	do
		return _msg;
	end
end
Exception(msg)
	do
		_msg = msg;
	end
end
Exception()
	do
		_msg = System.String.new("");
	end
end
