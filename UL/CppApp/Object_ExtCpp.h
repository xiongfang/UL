using namespace System;

HandleManager<Object> Object::Mgr;

Ref<String> Object::ToString()
{
	
	long v = (long)this;
	char str[32];

	sprintf_s(str,32, "%x", v);

	return new String(str);
}


Boolean Object::Equals(Ref<System::Object>  v)
{
	return v.Get() == this;
}

Ref<System::Type> Object::GetType()
{
	return nullptr;
}

System::Boolean System::Object::op_Equality(Ref<System::Object>  a, Ref<System::Object>  b)
{
	return a.v == b.v;
}

System::Boolean System::Object::ReferenceEquals(Ref<System::Object>  a, Ref<System::Object>  b)
{
	bool a_null = a.IsNull();
	bool b_null = b.IsNull();
	if (a_null && b_null)
		return true;
	if (!a_null && !b_null)
	{
		return a.Get() == b.Get();
	}

	return false;
}