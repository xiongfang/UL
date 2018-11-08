function ul.System.Collections.Generic.List:ctor(...)
	self.__objs = {}
end

function ul.System.Collections.Generic.List:get_Count()
	return ul.System.Int32.new(#self.__objs);
end
function ul.System.Collections.Generic.List:get_Index(index)
	return self.__objs[index.v+1];
end
function ul.System.Collections.Generic.List:set_Index(index,value)
	self.__objs[index.v+1] = value;
end
function ul.System.Collections.Generic.List:Add(item)
	self.__objs[self:get_Count().v+1] = item;
end
function ul.System.Collections.Generic.List:Remove(item)
	for i,v in ipairs(self.__objs) do
		if( v:Equals_ul_System_Object(item).v) then
			table.remove(self.__objs,i);
			return
		end
	end
end
function ul.System.Collections.Generic.List:RemoveAll(item)
	for i,v in ipairs(self.__objs) do
		if( v:Equals_ul_System_Object(item).v) then
			table.remove(self.__objs,i);
			return
		end
	end
end
function ul.System.Collections.Generic.List:Clear()
	self.__objs = {}
end
function ul.System.Collections.Generic.List:ToArray()
	local array = Construct(ul.System.ArrayT.new(),ul.System.ArrayT.ArrayT,self:get_Count())
end