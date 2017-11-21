#include "stdafx.h"
#include "Byte.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Exception.h"
System::Byte System::Byte::MaxValue=255;
System::Byte System::Byte::MinValue=0;
System::Boolean System::Byte::TryParse(Ref<System::String>  value,System::Byte  v)
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
