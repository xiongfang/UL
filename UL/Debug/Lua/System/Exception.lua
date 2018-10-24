require "System"
System.Exception = System.Object:new()
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
		_msg = "";
	end
end
