function System.Boolean:ctor(v)
	self.v = false;
	if(v~= nil ) then
		self.v = v;
	end
end

function System.Boolean:ToString()
	return System.String.new(tostring(self.v));
end
function System.Boolean.op_LogicNot(a)
	return System.Boolean.new(not a.v);
end