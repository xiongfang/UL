
function ul.System.String:ctor(v)
    self._v = v;
end

function ul.System.String:get_Length()
	return ul.System.Int32.new(string.len(self._v));
end

function ul.System.String:IndexOf_ul_System_Char(value)
	local a,b= string.find(self._v,value.v);
	if a == nil then
		a = -1;
	else
		a = a-1;
	end
	return ul.System.Int32.new(a);
end
function ul.System.String:IndexOf_ul_System_String(value)
	local a,b= string.find(self._v,value.v);
	if a == nil then
		a = -1;
	else
		a = a-1;
	end
	return ul.System.Int32.new(a);
end
function ul.System.String:LastIndexOf_ul_System_Char(value)
end
function ul.System.String:LastIndexOf_ul_System_String(value)
end
function ul.System.String.Format_ul_System_String_ul_System_ArrayT(format,args)
end