#include "stdafx.h"
#include "Char.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Exception.h"
System::Char System::Char::MaxValue=0xFFFF;
System::Char System::Char::MinValue=0;
System::Boolean System::Char::TryParse(Ref<System::String>  value,System::Char  v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = 0;
		return false;
	}
}
