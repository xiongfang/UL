require "System"
System.Object = class('System.Object')
function System.Object:GetType()
end
function System.Object:Equals_System_Object(v)
end
function System.Object:ToString()
end
function System.Object.ReferenceEquals_System_Object_System_Object(a,b)
end
function System.Object.op_Equality(a,b)
end
function System.Object.op_Inequality(a,b)
	do
		return System.Boolean.op_LogicNot((System.Object.op_Equality(a,b)));
	end
end
require "Object_ExtHeader"
