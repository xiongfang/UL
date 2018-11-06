function ul.System.Object:ctor()
end

function ul.System.Object:GetType()
	if self.__type == nil then
		self.__type = ul.System.Type.GetType_ul_System_String(self.__cname)
	end
	return self.__type;
end
function ul.System.Object:Equals_ul_System_Object(v)
end
function ul.System.Object:ToString()
	return ul.System.String:new( self.__cname);
end
function ul.System.Object.ReferenceEquals_ul_System_Object_ul_System_Object(a,b)
	return ul.System.Boolean:new(a==b);
end

function ul.System.Object.op_Equality(a,b)
	return ul.System.Object.ReferenceEquals_System_Object_System_Object(a,b);
end
function ul.System.Object:Equals_System_Object(v)
	return self:op_Equality(v);
end