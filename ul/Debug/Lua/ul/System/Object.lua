require "ul_System"
ul.System.Object = class('ul.System.Object')
function ul.System.Object:GetType()
end
function ul.System.Object:Equals_ul_System_Object(v)
end
function ul.System.Object:ToString()
end
function ul.System.Object.ReferenceEquals_ul_System_Object_ul_System_Object(a,b)
end
function ul.System.Object.op_Equality(a,b)
end
function ul.System.Object.op_Inequality(a,b)
	do
		return ul.System.Boolean.op_LogicNot((ul.System.Object.op_Equality(a,b)));
	end
end
require "Object_ExtHeader"
