#include "stdafx.h"
#include "UInt16.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
System::UInt16 System::UInt16::MaxValue=0xFFFF;
System::UInt16 System::UInt16::MinValue=0;
System::Boolean System::UInt16::TryParse(Ref<System::String>  value,System::UInt16  v)
{
	try
	{
		v.op_Assign(Parse(value));
		return true;
	}
	catch(System::Exception e)
	{
		v.op_Assign(0);
		return false;
	}
}
