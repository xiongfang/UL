#include "stdafx.h"
#include "System\Boolean.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\ArgumentNullException.h"
#include "System\FormatException.h"
#include "System\Exception.h"
Ref<System::String> System::Boolean::FalseString;
Ref<System::String> System::Boolean::TrueString;
System::Boolean System::Boolean::Parse(Ref<System::String>  value)
{
	if(System::String::op_Equality(Ref<System::Object>(value),nullptr))
	{
		throw new System::ArgumentNullException();
	}
	if(System::String::op_Equality(Ref<System::Object>(value),Ref<System::Object>(TrueString)))
		return true;
	else
		if(System::String::op_Equality(Ref<System::Object>(value),Ref<System::Object>(FalseString)))
			return false;
		else
		{
			throw new System::FormatException(value);
		}
}
System::Boolean System::Boolean::TryParse(Ref<System::String>  value,System::Boolean & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = false;
		return false;
	}
}
