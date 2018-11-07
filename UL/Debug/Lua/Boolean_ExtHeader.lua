function ul.System.Boolean:ctor(v)
	self._v = false;
	if(v~= nil ) then
		self._v = v;
	end
end

function ul.System.Boolean:ToString()
	return ul.System.String.new(tostring(self._v));
end
function ul.System.Boolean.op_LogicNot(a)
	return ul.System.Boolean.new(not a._v);
end