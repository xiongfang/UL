function ul.System.Boolean:ctor(v)
	self.v = false;
	if(v~= nil ) then
		self.v = v;
	end
end

function ul.System.Boolean:ToString()
	return System.String.new(tostring(self.v));
end
function ul.System.Boolean.op_LogicNot(a)
	return System.Boolean.new(not a.v);
end