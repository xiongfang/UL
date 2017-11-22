#include "stdafx.h"
#include "SByte.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Exception.h"
System::SByte System::SByte::MaxValue=127;
System::SByte System::SByte::MinValue=-128;
System::Boolean System::SByte::TryParse(Ref<System::String>  value,System::SByte & v)
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
