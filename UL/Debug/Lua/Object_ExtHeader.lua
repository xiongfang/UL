function System.Object:ctor()
end

function System.Object.ReferenceEquals_System_Object_System_Object(a,b)
	return System.Boolean:new(a==b);
end
function System.Object:ToString()
	return System.String:new('System.Object');
end
function System.Object.op_Equality(a,b)
	return System.Object.ReferenceEquals_System_Object_System_Object(a,b);
end
function System.Object:Equals_System_Object(v)
	return self:op_Equality(v);
end