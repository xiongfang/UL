require "ul_System"
ul.System.Exception = class('ul.System.Exception',ul.System.Object)
function ul.System.Exception:get_Message()
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
		_msg = ul.System.String.new("");
	end
end
