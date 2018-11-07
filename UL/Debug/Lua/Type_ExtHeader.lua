
function ul.System.Type:ctor(typeName)
	self._type =  ul.System.Reflection.Metadata.Type.new(typeName)
end


function ul.System.Type.GetType_ul_System_String(fullName)
	if(fullName==nil or fullName._v == "ul.System.void") then
		return nil
	end
	if(ul.System.Type.__types == nil) then
		ul.System.Type.__types = {}
	end
	local type = ul.System.Type.__types[fullName._v];
	if(type == nil) then
		 type = ul.System.Type.new(fullName._v)
		 ul.System.Type.__types[fullName._v] = type;
	end

	return type;
end