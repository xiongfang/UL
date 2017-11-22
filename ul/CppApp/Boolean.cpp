#include "stdafx.h"
#include "Boolean.h"
#include "Object.h"
#include "String.h"
#include "ArgumentNullException.h"
#include "FormatException.h"
#include "Exception.h"
Ref<System::String> System::Boolean::FalseString;
Ref<System::String> System::Boolean::TrueString;
System::Boolean System::Boolean::Parse(Ref<System::String>  value)
{
	if(value == nullptr)
	{
		throw new System::ArgumentNullException();
	}
	if(value == TrueString)
		return true;
	else
		if(value == FalseString)
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
