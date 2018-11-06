require "ul.System"
ul.System.Exception = class('ul.System.Exception',ul.System.Object)
function ul.System.Exception:get_Message()
	do
		return self._msg;
	end
end
function ul.System.Exception:Exception_ul_System_String(msg)
	do
		self._msg = msg;
	end
end
function ul.System.Exception:Exception()
	do
		self._msg = ul.System.String.new("");
	end
end
