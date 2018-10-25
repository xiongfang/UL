package.path = package.path ..';Lua\\?.lua';

require "System"
require "System.Object"
function System.Object:new(...)
      local o = {}
      setmetatable(o, self)
      self.__index = self
      o:ctor(...)
      return o
end
function System.Object.ctor()
end

require "System.Console"
function System.Console.Write_System_String(v)
    print(v._v);
end

require "System.String"


function System.String:new(v)
	local o = System.Object:new(v);
    o._v = v;
	return o;
end


require "System.Test"

function  main( ... )
	--System.Console.Write_System_String(System.String:new("込込1"));
	System.Test.Run();
	print("込込");
end