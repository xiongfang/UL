function ul.System.ArrayT:ctor(...)
	self.__objs = {}
end
function ul.System.ArrayT:get_Length()
	return self.__len;
end
function ul.System.ArrayT:get_Index(i)
	return self.__objs[i.v+1]
end
function ul.System.ArrayT:set_Index(i,value)
	self.__objs[i.v+1] = value;
end
function ul.System.ArrayT:ArrayT(len)
	self.__len = len;
end