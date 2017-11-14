#include "stdafx.h"
#include "Int32.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32_ExtCpp.h"
System::Int32 System::Int32::MaxValue=0x7FFFFFFF;
System::Int32 System::Int32::MinValue=0x80000000;
System::Boolean System::Int32::TryParse(Ref<System::String>  value,System::Int32  v)
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
