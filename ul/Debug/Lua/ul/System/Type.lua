require "ul_System"
ul.System.Type = class('ul.System.Type',ul.System.Object)
function ul.System.Type:get_BaseType()
	do
		return ul.System.Type.GetType_ul_System_String(ul.System.String.op_Addition(ul.System.String.op_Addition(self._type:get_Namespace(),ul.System.String.new(".")),self._type:get_Name()));
	end
end
function ul.System.Type:get_FullName()
	do
		return ul.System.String.op_Addition(ul.System.String.op_Addition(self._type:get_Namespace(),ul.System.String.new(".")),self._type:get_Name());
	end
end
function ul.System.Type:get_IsAbstract()
	do
		return self._type:get_IsAbstract();
	end
end
function ul.System.Type:get_IsClass()
	do
		return ul.System.Int32.op_Equality(clone(self._type:get_TypeID()),clone(ul.System.Int32.new(2)));
	end
end
function ul.System.Type:get_IsEnum()
	do
		return ul.System.Int32.op_Equality(clone(self._type:get_TypeID()),clone(ul.System.Int32.new(3)));
	end
end
function ul.System.Type:get_IsGenericType()
	do
		return self._type:get_IsGenericTypeDefinition();
	end
end
function ul.System.Type:get_IsInterface()
	do
		return ul.System.Int32.op_Equality(clone(self._type:get_TypeID()),clone(ul.System.Int32.new(1)));
	end
end
function ul.System.Type:get_IsPublic()
	do
		return ul.System.Int32.op_Equality(clone(self._type:get_Modifier()),clone(ul.System.Int32.new(0)));
	end
end
function ul.System.Type:get_IsValueType()
	do
		return ul.System.Int32.op_Equality(clone(self._type:get_TypeID()),clone(ul.System.Int32.new(0)));
	end
end
function ul.System.Type:get_Name()
	do
		return self._type:get_Name();
	end
end
function ul.System.Type:get_Namespace()
	do
		return self._type:get_Namespace();
	end
end
function ul.System.Type.GetType_ul_System_String(fullName)
end
function ul.System.Type:IsChildOf_ul_System_Type(type)
	do
		local baseType = BaseType;
		while(ul.System.Type.op_Inequality(baseType,nil))
		do
		do
			if(ul.System.Type.op_Equality(baseType,type)) then
				return ul.System.Boolean.new(true);
			end
			baseType = baseType:get_BaseType();
		end
		end
		return ul.System.Boolean.new(false);
	end
end
