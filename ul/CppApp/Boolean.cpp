#include "stdafx.h"
#include "Boolean.h"
#include "Object.h"
#include "String.h"
Ref<System::String> System::Boolean::FalseString;
Ref<System::String> System::Boolean::TrueString;
System::Boolean System::Boolean::Parse(Ref<System::String>  value)
{
	if(value->op_Equals(nullptr))
	{
	}
	if(value->op_Equals(Ref<System::Object>(TrueString.Get())))
	return true;
	else
	if(value->op_Equals(Ref<System::Object>(FalseString.Get())))
	return false;
	else
	{
	}
}
System::Boolean System::Boolean::TryParse(Ref<System::String>  value,System::Boolean  v)
{
	try
	{
		v.op_Assign(Parse(value));
		return true;
	}
	catch(System::Exception e)
	{
		v.op_Assign(false);
		return false;
	}
}
