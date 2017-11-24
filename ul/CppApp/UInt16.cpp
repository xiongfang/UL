#include "stdafx.h"
#include "UInt16.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Exception.h"
System::UInt16 System::UInt16::MaxValue=0xFFFF;
System::UInt16 System::UInt16::MinValue=0;
System::Boolean System::UInt16::TryParse(Ref<System::String>  value,System::UInt16 & v)
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
