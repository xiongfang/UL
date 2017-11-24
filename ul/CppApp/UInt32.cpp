#include "stdafx.h"
#include "UInt32.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Exception.h"
System::UInt32 System::UInt32::MaxValue=0xFFFFFFFF;
System::UInt32 System::UInt32::MinValue=0;
System::Boolean System::UInt32::TryParse(Ref<System::String>  value,System::UInt32 & v)
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
