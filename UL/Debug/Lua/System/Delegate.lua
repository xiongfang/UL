require "System"
System.Delegate = class('System.Delegate',System.Object)
function System.Delegate:get_Target()
	do
		return _target;
	end
end
Delegate()
	do
		list = System.Collections.Generic.List.new();
	end
end
function System.Delegate.Combine_System_Delegate_System_Delegate(a,b)
	do
		if(System.Delegate.op_Equality(a,nil)) then
			return b;
		end
		if(System.Delegate.op_Equality(b,nil)) then
			return a;
		end
		return a:CombineImpl_System_Delegate(b);
	end
end
function System.Delegate.Combine_System_ArrayT(delegates)
	do
		local left = nil;
		do
			local 			i = System.Int32.new(0)
			while System.Int32.op_LessThen(clone(i),clone(delegates:get_Length()))
				do
					do
						left = System.Delegate.Combine_System_Delegate_System_Delegate(left,delegates.get_Index(i));
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
function System.Delegate.Remove_System_Delegate_System_Delegate(source,value)
	do
		if(System.Delegate.op_Equality(source,nil)) then
			return nil;
		end
		if(System.Delegate.op_Equality(value,nil)) then
			return source;
		end
		source.list:Remove_System_Delegate(value);
		return source;
	end
end
function System.Delegate.RemoveAll_System_Delegate_System_Delegate(source,value)
	do
		if(System.Delegate.op_Equality(source,nil)) then
			return nil;
		end
		if(System.Delegate.op_Equality(value,nil)) then
			return source;
		end
		source.list:RemoveAll_System_Delegate(value);
		return source;
	end
end
function System.Delegate:GetInvocationList()
	do
		return list:ToArray();
	end
end
function System.Delegate:CombineImpl_System_Delegate(d)
	do
		list:Add_System_Delegate(d);
		return self;
	end
end
function System.Delegate.op_Equality(d1,d2)
	do
		if(System.Object.ReferenceEquals_System_Object_System_Object(d1,d2)) then
			return System.Boolean.new(true);
		end
		if(System.Object.ReferenceEquals_System_Object_System_Object(d1,nil)) then
			return System.Boolean.new(false);
		end
		if(System.Object.ReferenceEquals_System_Object_System_Object(d2,nil)) then
			return System.Boolean.new(false);
		end
		return d1:Equals_System_Object(d2);
	end
end
function System.Delegate.op_Inequality(d1,d2)
	do
		return System.Boolean.op_LogicNot((System.Delegate.op_Equality(d1,d2)));
	end
end
function System.Delegate.op_Addition(d1,d2)
	do
		return System.Delegate.Combine_System_Delegate_System_Delegate(d1,d2);
	end
end
function System.Delegate.op_Substraction(d1,d2)
	do
		return System.Delegate.Remove_System_Delegate_System_Delegate(d1,d2);
	end
end
