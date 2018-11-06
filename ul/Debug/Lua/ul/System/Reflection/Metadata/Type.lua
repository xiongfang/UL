require "ul.System.Reflection.Metadata"
ul.System.Reflection.Metadata.Type = class('ul.System.Reflection.Metadata.Type',ul.System.Object)
function ul.System.Reflection.Metadata.Type:get_Name()
	do
		return self._name;
	end
end
function ul.System.Reflection.Metadata.Type:get_Namespace()
	do
		return self._namespace;
	end
end
function ul.System.Reflection.Metadata.Type:get_Comments()
	do
		return self._comments;
	end
end
function ul.System.Reflection.Metadata.Type:get_Modifier()
	do
		return self._modifier;
	end
end
function ul.System.Reflection.Metadata.Type:get_TypeID()
	do
		return self._type;
	end
end
function ul.System.Reflection.Metadata.Type:get_Parent()
	do
		return ul.System.Reflection.Metadata.Type.GetType_ul_System_Reflection_Metadata_TypeSyntax(self._parent);
	end
end
function ul.System.Reflection.Metadata.Type:get_IsAbstract()
	do
		return self._is_abstract;
	end
end
function ul.System.Reflection.Metadata.Type:get_IsGenericTypeDefinition()
	do
		return self._is_generic_type_definition;
	end
end
function ul.System.Reflection.Metadata.Type.GetType_ul_System_Reflection_Metadata_TypeSyntax(typeSyntax)
end
