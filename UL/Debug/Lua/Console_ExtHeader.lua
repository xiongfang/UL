function ul.System.Console.Write_ul_System_String(v)
	if v == nil then
		print(debug.traceback())
	end
	io.write(v._v)
end
