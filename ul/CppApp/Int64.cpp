#include "stdafx.h"
#include "Int64.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Exception.h"
System::Int64 System::Int64::MaxValue=0x7FFFFFFFFFFFFFFF;
System::Int64 System::Int64::MinValue=0x8000000000000000;
System::Boolean System::Int64::TryParse(Ref<System::String>  value,System::Int64 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Int64(0);
		return false;
	}
}
