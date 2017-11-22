using namespace System;
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