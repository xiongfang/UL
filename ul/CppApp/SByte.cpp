#include "stdafx.h"
#include "SByte.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
System::SByte System::SByte::MaxValue=127;
System::SByte System::SByte::MinValue=-128;
System::Boolean System::SByte::TryParse(Ref<System::String>  value,System::SByte  v)
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
