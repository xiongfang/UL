
function System.String:ctor(v)
    self._v = v;
end

function System.String:get_Length()
	return System.Int32.new(string.len(self._v));
end

function System.String:IndexOf_System_Char(value)
	local a,b= string.find(self._v,value.v);
	if a == nil then
		a = -1;
	else
		a = a-1;
	end
	return System.Int32.new(a);
end
function System.String:IndexOf_System_String(value)
	local a,b= string.find(self._v,value.v);
	if a == nil then
		a = -1;
	else
		a = a-1;
	end
	return System.Int32.new(a);
end
function System.String:LastIndexOf_System_Char(value)
end
function System.String:LastIndexOf_System_String(value)
end
function System.String.Format_System_String_System_ArrayT(format,args)
end