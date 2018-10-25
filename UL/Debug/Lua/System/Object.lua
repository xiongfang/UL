require "System"
System.Object = {}
function System.Object.op_Inequality(a,b)
	do
		return System.Boolean.op_LogicNot((System.Object.op_Equality(a,b)));
	end
end
require "Object_ExtHeader"
