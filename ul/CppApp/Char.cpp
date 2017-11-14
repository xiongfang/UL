#include "stdafx.h"
#include "Char.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
System::Char System::Char::MaxValue=0xFFFF;
System::Char System::Char::MinValue=0;
System::Boolean System::Char::TryParse(Ref<System::String>  value,System::Char  v)
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
