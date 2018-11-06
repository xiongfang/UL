

function ul.System.Type:ctor(typeName)
	self._type =  _G[typeName+"_Metadata"]
end


function ul.System.Type.GetType_ul_System_String(fullName)
	if(ul.System.Type.__types == nil)
		ul.System.Type.__types = {}
	local type = ul.System.Type.__types[fullName];
	if(type == nil) then
		 type = ul.System.Type:new(fullName)
		 ul.System.Type.__types[fullName] = type;
	end

	return type;
end