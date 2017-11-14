#include "stdafx.h"
#include "Int16.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
System::Int16 System::Int16::MaxValue=0x7FFF;
System::Int16 System::Int16::MinValue=0x8000;
System::Boolean System::Int16::TryParse(Ref<System::String>  value,System::Int16  v)
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
