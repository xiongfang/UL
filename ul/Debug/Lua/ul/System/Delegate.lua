require "ul.System"
ul.System.Delegate = class('ul.System.Delegate',ul.System.Object)
function ul.System.Delegate:get_Target()
	do
		return self._target;
	end
end
function ul.System.Delegate:Delegate()
	do
		self.list = ul.System.Collections.Generic.List.new();
	end
end
function ul.System.Delegate.Combine_ul_System_Delegate_ul_System_Delegate(a,b)
	do
		if (ul.System.Delegate.op_Equality(a,nil))._v then
			return b;
		end
		if (ul.System.Delegate.op_Equality(b,nil))._v then
			return a;
		end
		return a:CombineImpl_ul_System_Delegate(b);
	end
end
function ul.System.Delegate.Combine_ul_System_ArrayT(delegates)
	do
		local left = nil;
		do
			local 			i = ul.System.Int32.new(0)
			while (ul.System.Int32.op_LessThen(clone(i),clone(delegates:get_Length())))._v
				do
					do
						left = ul.System.Delegate.Combine_ul_System_Delegate_ul_System_Delegate(left,delegates.get_Index(i));
					end
				end
				do
					::for_end::
					PostfixUnaryHelper.op_Increment(i,function(v) i = v end)
				end
			end
		end
		return left;
	end
end
function ul.System.Delegate.Remove_ul_System_Delegate_ul_System_Delegate(source,value)
	do
		if (ul.System.Delegate.op_Equality(source,nil))._v then
			return nil;
		end
		if (ul.System.Delegate.op_Equality(value,nil))._v then
			return source;
		end
		source.list:Remove_ul_System_Delegate(value);
		return source;
	end
end
function ul.System.Delegate.RemoveAll_ul_System_Delegate_ul_System_Delegate(source,value)
	do
		if (ul.System.Delegate.op_Equality(source,nil))._v then
			return nil;
		end
		if (ul.System.Delegate.op_Equality(value,nil))._v then
			return source;
		end
		source.list:RemoveAll_ul_System_Delegate(value);
		return source;
	end
end
function ul.System.Delegate:GetInvocationList()
	do
		return self.list:ToArray();
	end
end
function ul.System.Delegate:CombineImpl_ul_System_Delegate(d)
	do
		self.list:Add_ul_System_Delegate(d);
		return self;
	end
end
function ul.System.Delegate.op_Equality(d1,d2)
	do
		if (ul.System.Object.ReferenceEquals_ul_System_Object_ul_System_Object(d1,d2))._v then
			return ul.System.Boolean.new(true);
		end
		if (ul.System.Object.ReferenceEquals_ul_System_Object_ul_System_Object(d1,nil))._v then
			return ul.System.Boolean.new(false);
		end
		if (ul.System.Object.ReferenceEquals_ul_System_Object_ul_System_Object(d2,nil))._v then
			return ul.System.Boolean.new(false);
		end
		return d1:Equals_ul_System_Object(d2);
	end
end
function ul.System.Delegate.op_Inequality(d1,d2)
	do
		return ul.System.Boolean.op_LogicNot((ul.System.Delegate.op_Equality(d1,d2)));
	end
end
function ul.System.Delegate.op_Addition(d1,d2)
	do
		return ul.System.Delegate.Combine_ul_System_Delegate_ul_System_Delegate(d1,d2);
	end
end
function ul.System.Delegate.op_Substraction(d1,d2)
	do
		return ul.System.Delegate.Remove_ul_System_Delegate_ul_System_Delegate(d1,d2);
	end
end
